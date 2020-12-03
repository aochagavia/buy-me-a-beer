# Provisioning infrastructure

Auth0 requires setting up some configuration before running `pulumi up`. You have to first create
a "Machine to machine" application in Auth0's dashboard, which will be used by Pulumi to manage
Auth0 for us. Then you have to configure Pulumi to use the application's keys:

```
pulumi config set auth0:domain <your-domain>
pulumi config set auth0:clientId <your-client-id> --secret
pulumi config set auth0:clientSecret <your-client-secret> --secret
```
