import * as pulumi from "@pulumi/pulumi";
import * as azure from "@pulumi/azure";

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

    const secret = new azure.keyvault.Secret('ApplicationInsights--InstrumentationKey', {
        keyVaultId: vault.id,
        value: appInsights.instrumentationKey,
        name: 'ApplicationInsights--InstrumentationKey', // Needed to prevent pulumi from appending a random string
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
        // TODO: uncommenting the code below leads stuff to fail. For now we have to manually configure the key vault name.
        // appSettings: {
        //     'KeyVaultName': vault.name,
        // },
    });

    // Taken from: https://github.com/pulumi/examples/blob/21a0f83c2b630610ba8bc36bc2002fe8079e5538/azure-ts-msi-keyvault-rbac/index.ts
    // Work around a preview issue https://github.com/pulumi/pulumi-azure/issues/192
    const principalId = appService.identity.apply(id => id.principalId || "11111111-1111-1111-1111-111111111111");
    const appServicePolicy = new azure.keyvault.AccessPolicy(`${prefix}-access-policy`, {
        keyVaultId: vault.id,
        tenantId: clientConfig.tenantId,
        objectId: principalId,
        secretPermissions: ['get', 'list'],
    });
}
