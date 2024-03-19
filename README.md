# eShopping
## Purpose
The project represents by itself a web service developed for international shopping, which is popular in Ukraine, with a target on buyers and announcers. Buyers offer theirs customers buying goods from abroad and use announces to stimulate customers to ordering something. Announcers offer announces to buyers so buyers could focus on the managing of ordering & delivery processes.

This web service aims to sutisfy buyer's, announcer's and, in future, customer's needs and currently offers delivery of announces to the buyer's audience according buyer's preferences though Viber & Telegram.

## 
This project is splitted on frontend SPA application and backend services that were built based on microservices architecture. The frontend application is built with React/Redux and the microservices are built using:
- *.NET 7*
- *Ocelot Gateway* - used as a single entrypoint for the frontend and all the microservices
- *Duende Identity Sever* - used as an identity provider
- *SignalR* - for real-time notifications
- *Azure Cosmos Db* - as a data storage
- *MassTransit* - as a framework used over RabbitMq or Azure Service Bus
- *Azure Storage Blob* - as a file storage
- *Azure CDN* - for quick access to the file storage
- *Azure DNS* - for hosting all the services under the same domain
- *Telegram SDK* - for managing Telegram bot
- *Viber SDK* - for managing Viber bot
There are also some frameworks built on top on Telegram and Viber SDKs to make a work with them a little bit easier for the purposes of this project.

eShopping service is hosted on Azure infrastracture using App Services (with configured CD using Azure Pipelines for frontend application and App Service's built in deployment system for backend-services) however it's being currently shut down.
