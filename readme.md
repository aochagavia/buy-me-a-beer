# BuyMeABeer

This repository showcases a basic ASP.NET Core application that resembles something you could build
for a real-life use-case. It is called BuyMeABeer, because all it does is letting people buy you a
beer, optionally leaving behind a message.

From a technical standpoint, this all involves being able to:

* Process payments
* Persist user messages
* Logging in as admin to moderate user messages

Additionally, to keep mental sanity in the development process we must be able to:

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

TODO: explain our usage of docker-compose

TODO: explain how to set up Auth0 with dotnet secret

## Authentication

Followed this tutorial to set up Auth0: https://auth0.com/docs/quickstart/webapp/aspnet-core-3

## Client-side library management

Followed this tutorial to set up Webpack (with modifications because I wanted the most recent versions):
* https://medium.com/@lars.willemsens/managing-asp-net-core-mvc-front-end-dependencies-with-npm-and-webpack-part-1-1f6a8b8a1289
* https://medium.com/@lars.willemsens/managing-asp-net-core-mvc-front-end-dependencies-with-npm-and-webpack-part-2-330ccbe89e0a

## TODO

Must do:

* Finish the basic functionality around buying me a beer:
    * Fix client-side validation when posting a comment
    * Fix client-side validation when posting an order with a custom price
    * Use other beer prices and names (e.g. normal beer 2.40, abbey beer 5.20)
* Polish stripe integration:
    * Figure out which payment providers to accept (can we do all?)
* Cleanup and sanity checks:
    * Extract a BeerProductRepository out of the BeerOrderService
    * Figure out how to stop using the master database and use a custom one instead
    * Consolidate migrations
    * We should cache the comments so we survive HN's hug of death
    * We should have a WebsiteTest project where we at least test our services
    * Use a beer emoji as favicon
* Set up Auth0 properly from Pulumi:
    * Signing in through Google should be disabled
    * Registering should be disabled
* Find and resolve remaining TODOs

Could do (but won't, at the moment):

* Completely migrate to `azure-nextgen` Pulumi resource provider instead of `azure`
* Figure out how to enable "managed identities for azure resources" so our app service can talk to the database without having credentials in the connection string (my initial attempt based on examples from the internet failed so right now we are storing the connection string in the key vault)
* Currently it is technically possible to post a comment _without_ having paid, though you will need to read the source code to find out ;). We _could_ make sure this is impossible, but then that would be another TODO on the list and this project would never be released
* Use role-based authorization instead of letting any user access the admin dashboard
