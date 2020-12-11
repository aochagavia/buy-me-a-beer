import * as pulumi from "@pulumi/pulumi";
import * as random from "@pulumi/random";
import * as azure from "@pulumi/azure";
import * as auth0 from "@pulumi/auth0";

const prefix = 'buy-me-a-beer';
function makeResourceName(name: string): string {
    return `${prefix}-${name}`;
}

export = async () => {
    const azureConfig = new pulumi.Config('azure');
    const location = azureConfig.require('location');

    const rgName = makeResourceName('rg');
    const rg = new azure.core.ResourceGroup(rgName);

    const aiName = makeResourceName('ai');
    const ai = new azure.appinsights.Insights(aiName, {
        resourceGroupName: rg.name,
        applicationType: 'web',
    });

    const clientConfig = await azure.core.getClientConfig();
    const kvName = makeResourceName('kv');
    const vault = new azure.keyvault.KeyVault(kvName, {
        resourceGroupName: rg.name,
        tenantId: clientConfig.tenantId,
        skuName: 'standard',
        softDeleteEnabled: true,
        accessPolicies: [
            // Access for pulumi deployments and for ourselves through the Azure UI
            {
                tenantId: clientConfig.tenantId,
                objectId: clientConfig.objectId,
                secretPermissions: ['delete', 'get', 'list', 'set', 'purge'],
            },
        ],
    });

    const aiSecret = new azure.keyvault.Secret('ApplicationInsights--InstrumentationKey', {
        keyVaultId: vault.id,
        value: ai.instrumentationKey,
        name: 'ApplicationInsights--InstrumentationKey',
    });

    const administratorLoginUsername = new random.RandomPassword("sqlAdminUsername", { length: 32, special: true }).result;
    const administratorLoginPassword = new random.RandomPassword("sqlAdminPassword", { length: 32, special: true }).result;
    const sqlServerName = makeResourceName('sql');
    const sqlServer = new azure.sql.SqlServer(sqlServerName, {
        resourceGroupName: rg.name,
        administratorLogin: administratorLoginUsername,
        administratorLoginPassword: administratorLoginPassword,
        version: "12.0",
    });

    const dbName = makeResourceName('db');
    const db = new azure.sql.Database(dbName, {
        resourceGroupName: rg.name,
        serverName: sqlServer.name,
        requestedServiceObjectiveName: "S0",
    });

    const dbConnectionString = pulumi
        .all([sqlServer.name, db.name, administratorLoginUsername, administratorLoginPassword])
        .apply(([server, db, username, password]) =>
            `Server=tcp:${server}.database.windows.net;Database=${db};User ID=${username};Password=${password};`);

    const sqlConnectionString = new azure.keyvault.Secret('Data--DbContext--ConnectionString', {
        keyVaultId: vault.id,
        value: dbConnectionString,
        name: 'Data--DbContext--ConnectionString',
    });

    const planName = makeResourceName('plan');
    const planSize = azureConfig.require('appServicePlanSize');
    const tier = azureConfig.require('appServiceTier');
    const appServicePlan = new azure.appservice.Plan(planName, {
        resourceGroupName: rg.name,
        sku: {
            size: planSize,
            tier,
        },
        reserved: true,
        kind: 'Linux',
    });

    const appServiceName = makeResourceName('app');
    const appService = new azure.appservice.AppService(appServiceName, {
        appServicePlanId: appServicePlan.id,
        resourceGroupName: rg.name,
        httpsOnly: true,
        enabled: true,
        identity: {
            type: 'SystemAssigned',
        },
        appSettings: {
            'KeyVaultName': vault.name,
            'Auth0__Domain': new pulumi.Config('auth0').require('domain'),
            'Stripe__PublicKey': new pulumi.Config('stripe').require('publicKey'),
        },
        siteConfig: {
            linuxFxVersion: 'DOTNET |5.0',
            use32BitWorkerProcess: true,
        }
    });

    // TODO: find a way to put this in appsettings instead of abusing the key vault
    new azure.keyvault.Secret('Deployment--BaseUrl', {
        keyVaultId: vault.id,
        value: appService.defaultSiteHostname.apply(name => `https://${name}`),
        name: 'Deployment--BaseUrl',
    });

    // TODO: automatically configure custom domain name in AppService
    // * Create a TXT record to validate
    // * Create a CNAME record that points to the app service
    // * Create an AppService Managed certificate (see https://docs.microsoft.com/en-us/azure/app-service/configure-ssl-certificate#create-a-free-certificate-preview for the manual version)
    // * Add binding for the certificate

    // Work around a preview issue https://github.com/pulumi/pulumi-azure/issues/192
    const principalId = appService.identity.apply(id => id?.principalId || '11111111-1111-1111-1111-111111111111');
    const appServicePolicy = new azure.keyvault.AccessPolicy(`app-access-policy`, {
        keyVaultId: vault.id,
        tenantId: clientConfig.tenantId,
        objectId: principalId,
        secretPermissions: ['get', 'list'],
    });

    const firewallRuleName = 'Allow Azure services';
    const firewallRule = new azure.sql.FirewallRule(firewallRuleName, {
        name: firewallRuleName,
        resourceGroupName: rg.name,
        startIpAddress: '0.0.0.0',
        endIpAddress: '0.0.0.0',
        serverName: sqlServer.name,
    });

    const auth0Client = new auth0.Client('BuyMeABeer', {
        name: 'BuyMeABeer',
        appType: 'regular_web',
        allowedLogoutUrls: [
            'http://localhost:3000',
            appService.defaultSiteHostname.apply(hostName => `https://${hostName}`),
        ],
        callbacks: [
            'http://localhost:3000/callback',
            appService.defaultSiteHostname.apply(hostName => `https://${hostName}/callback`),
        ],
        jwtConfiguration: {
            alg: 'RS256',
        },
    });

    // TODO: figure out how to turn off the login via Google for the newly created client, which is turned on by default
    // Check out this: https://www.pulumi.com/docs/reference/pkg/auth0/connection/
    // const auth0GoogleConnection = new auth0.Connection('Google', {

    // });

    const auth0ClientId = new azure.keyvault.Secret('Auth0--ClientId', {
        keyVaultId: vault.id,
        value: auth0Client.clientId,
        name: 'Auth0--ClientId',
    });

    const auth0ClientSecret = new azure.keyvault.Secret('Auth0--ClientSecret', {
        keyVaultId: vault.id,
        value: auth0Client.clientSecret,
        name: 'Auth0--ClientSecret',
    });

    const stripeSecretKey = new azure.keyvault.Secret('Stripe--SecretKey', {
        keyVaultId: vault.id,
        value: new pulumi.Config('stripe').require('secretKey'),
        name: 'Stripe--SecretKey',
    });
}
