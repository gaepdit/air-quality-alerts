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

![Repobeats analytics image](https://repobeats.axiom.co/api/embed/c81151b9faabd47046c88434ab30890a34d12505.svg)

## Solution organization

The solution contains the following projects along with their corresponding unit test projects.

* **Services** — Class library/libraries implementing various services required by the apps
    * *MailRetrievalService* — A service to retrieve unprocessed emails from the mailbox
    * *MailParserService* — A service to parse emails from the ambient monitors and generate templated messages for the
      public
    * *MessageDeliveryService* — A service to deliver the messages to the public
    * *RepositoryService* — A service to store the results of mail retrieval, mail parsing, and message delivery
    * *NotificationService* — A service to notify administrators of failures or errors
* **Daemon** — A Windows service that periodically retrieves and disseminates ambient monitoring data
* **WebApp** — A web application for use by APB and EPD-IT staff to monitor the status and past actions taken by the
  daemon
    * `/Status` — Status of daemon
    * `/RunLog` — Log of attempts to retrieve emails (from *RepositoryService*)
    * `/MessageLog` — Log of messages parsed and delivered (from *RepositoryService*)
    * `/TemplateTest` — Page for testing the parsing and translating service (using *MailParserService*)

### Project dependency diagram

```mermaid
flowchart LR
    D[Daemon]
    W[WebApp]
    M[Mail Retrieval Service]
    P[Mail Parser Service]
    V[Message Delivery Service]
    R[Repository Service]
    N[Notification Service]
    D --> M
    D --> V
    D --> N
    D --> P
    D --> R
    W --> P
    W --> R
```
