# Provisioning infrastructure

BuyMeABeer requires setting up some configuration before running `pulumi up`.

## Auth0 config

First create a "Machine to machine" application in Auth0's dashboard, which will be used by
Pulumi to manage Auth0 for us. Then you configure Pulumi to use the application's keys:

```
pulumi config set auth0:domain <your-domain>
pulumi config set auth0:clientId <your-client-id> --secret
pulumi config set auth0:clientSecret <your-client-secret> --secret
```

## Stripe config

Configure Pulumi to use your Stripe keys:

```
pulumi config set stripe:publicKey <your-public-key>
pulumi config set stripe:secretKey <your-secret-key> --secret
```
