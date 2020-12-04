import * as pulumi from "@pulumi/pulumi";
import * as random from "@pulumi/random";
import * as azure from "@pulumi/azure";
import * as auth0 from "@pulumi/auth0";

const prefix = "buy-me-a-beer";

export = async () => {
    const resourceGroup = new azure.core.ResourceGroup(`${prefix}-rg`);

    const appInsights = new azure.appinsights.Insights(`${prefix}-ai`, {
        resourceGroupName: resourceGroup.name,
        applicationType: 'web',
    });

    const clientConfig = await azure.core.getClientConfig();
    const vault = new azure.keyvault.KeyVault(`${prefix}-kv`, {
        resourceGroupName: resourceGroup.name,
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
        value: appInsights.instrumentationKey,
        name: 'ApplicationInsights--InstrumentationKey',
    });

    const administratorLoginUsername = new random.RandomPassword("sqlAdminUsername", { length: 32, special: true }).result;
    const administratorLoginPassword = new random.RandomPassword("sqlAdminPassword", { length: 32, special: true }).result;
    const sqlServer = new azure.sql.SqlServer(`${prefix}-sql`, {
        resourceGroupName: resourceGroup.name,
        administratorLogin: administratorLoginUsername,
        administratorLoginPassword: administratorLoginPassword,
        version: "12.0",
    });

    const database = new azure.sql.Database(`${prefix}-db`, {
        resourceGroupName: resourceGroup.name,
        serverName: sqlServer.name,
        requestedServiceObjectiveName: "S0",
    });

    const dbConnectionString = pulumi
        .all([sqlServer.name, database.name, administratorLoginUsername, administratorLoginPassword])
        .apply(([server, db, username, password]) =>
            `Server=tcp:${server}.database.windows.net;Database=${db};User ID=${username};Password=${password};`);

    const sqlConnectionString = new azure.keyvault.Secret('Data--DbContext--ConnectionString', {
        keyVaultId: vault.id,
        value: dbConnectionString,
        name: 'Data--DbContext--ConnectionString',
    });

    const appServicePlan = new azure.appservice.Plan(`${prefix}-plan`, {
        resourceGroupName: resourceGroup.name,
        sku: {
            capacity: 1,
            size: 'D1', // See https://azure.microsoft.com/en-us/pricing/details/app-service/windows/ for size names
            tier: 'Shared',
        }
    });

    const appService = new azure.appservice.AppService(`${prefix}-app`, {
        appServicePlanId: appServicePlan.id,
        resourceGroupName: resourceGroup.name,
        httpsOnly: true,
        enabled: true,
        identity: {
            type: 'SystemAssigned',
        },
        appSettings: {
            'KeyVaultName': vault.name,
            'Auth0:Domain': new pulumi.Config('auth0').require('domain'),
        },
    });

    // Work around a preview issue https://github.com/pulumi/pulumi-azure/issues/192
    const principalId = appService.identity.apply(id => id.principalId || "11111111-1111-1111-1111-111111111111");
    const appServicePolicy = new azure.keyvault.AccessPolicy(`${prefix}-access-policy`, {
        keyVaultId: vault.id,
        tenantId: clientConfig.tenantId,
        objectId: principalId,
        secretPermissions: ['get', 'list'],
    });

    // Add SQL firewall exceptions
    // TODO: are these rules necessary? I am almost sure the appservice will have access regardless of these rules
    const firewallRules = appService.outboundIpAddresses.apply(
        ips => ips.split(",").map(
            ip => new azure.sql.FirewallRule(`FR${ip}`, {
                resourceGroupName: resourceGroup.name,
                startIpAddress: ip,
                endIpAddress: ip,
                serverName: sqlServer.name,
            }),
        ));

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
}
