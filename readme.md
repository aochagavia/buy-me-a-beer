# BuyMeABeer

This repository showcases a basic ASP.NET Core application that resembles something you could build
for a real-life use-case. It is called BuyMeABeer, because all it does is letting people buy you a
beer, optionally leaving behind a message.

From a technical standpoint, this all involves being able to:

* Process payments
* Persist user messages
* Logging in as admin to moderate user messages

Additionally, to keep mental sanity in the development process we should be able to:

* Easily deploy the application
* Easily run the application locally
* Monitor performance and health of the application
* Manage secrets in a proper way

## SaaS approach

One of the goals when building this application is to minimize the amount of maintenance work needed
to keep it running. At least on paper, it looks like the way to go are SaaS providers for the different
components. For instance, if we were going for Azure, we could use:

* An App Service to deploy the ASP.NET Core application
* Some Azure database for data persistence
* Application Insights for monitoring
* Azure Key Vault for secrets

Besides this, we could use:

* Stripe for payments
* Auth0 for Authorization

### What about Kubernetes?

Kubernetes seems to offer a viable alternative to the Azure services mentioned above. I assume it is
technically possible to set up a Kubernetes cluster with a database, monitoring, key vault and the app
itself. However, at a first glance it seems to me that all this would require more maintenance than
the SaaS approach mentioned above.

For instance, we would probably need to figure out ourselves what to do about database backups, instead
of relying on the SaaS provider doing that for us. The same goes for database updates, which would require
manual updates every time a new version comes out. Maybe I am not well informed, but in that case I hope
some random person on the internet cares enough to tell me about an alternative.

### Which SaaS provider to use?

TODO: good question...

## Local development and deployment

TODO: write out how to develop and deploy this

## TODO

* Provision a database on Azure using Pulumi and make sure the website connects to it (check out https://github.com/pulumi/examples/blob/master/azure-cs-msi-keyvault-rbac/AppStack.cs in case we want to use AD auth instead of connection strings)
* Add authentication to the website using Auth0 or another provider (check whether it is possible to provision it using Pulumi)
* Cleanup the website backend (e.g. use EF Core and auth best practices)
* Cleanup the website frontend (e.g. use MVC best practices, use npm for frontend dependencies)
* Create the pages we actually want to have
* Integrate with a payments provider
