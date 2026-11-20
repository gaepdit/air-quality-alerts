# Air Quality Alerts

The Air Protection Branch operates ambient air quality monitors throughout the state. These monitors send automated
notifications to a centralized APB mailbox when monitored values exceed certain thresholds. The APB has requested a new
application to take those notifications, translate them into understandable, actionable messages for the public, and
send them to various opt-in subscription lists.

This project is required by EPA as part of EPD's ozone attainment maintenance plan for Atlanta. We are implementing it
statewide to provide a better understanding of real-time air quality.

[![Georgia EPD-IT](https://raw.githubusercontent.com/gaepdit/gaepd-brand/main/blinkies/blinkies.cafe-gaepdit.gif)](https://github.com/gaepdit)
[![.NET Test](https://github.com/gaepdit/air-quality-alerts/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/gaepdit/air-quality-alerts/actions/workflows/dotnet-test.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit.air-quality-alerts&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit.air-quality-alerts)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit.air-quality-alerts&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit.air-quality-alerts)

## Solution organization

The solution contains the following projects along with their corresponding unit test projects.


* **Domain** — A class library defining the data models and service interfaces
* **Services** — A class library implementing various services required by the apps
    * *MailRetrievalService* — A service to retrieve unprocessed emails from the mailbox
    * *ParserService* — A service to parse emails from the ambient monitors
    * *TemplatingService* — A service to generate messages for the public from the parsed emails
    * *DeliveryService* — A service to deliver the messages to the public
    * *NotificationService* — A service to notify administrators of failures or errors
* **Repository** — A class library implementing the repositories and data stores using Entity Framework
* **DaemonApp** — A Windows service accomplishing the main functionality
* **WebApp** — A web application for use by APB staff to monitor the status and past actions taken by the application

### Project dependency diagram

```mermaid
flowchart TB
    D[DaemonApp]
    W[WebApp]
    S[Services]
    R[Repository]
    C[Domain]
    D --> R
    D --> S
    W --> R
    W --> S
    R --> C
    S --> C
```
