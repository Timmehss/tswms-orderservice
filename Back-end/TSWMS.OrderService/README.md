# Order Service Docker Setup

This document provides essential commands for managing the Docker setup of the Order Service application. Below are the commands listing and an overview of the project's structure.

## Project Structure

- **TSMWS.OrderService.Api**: The main entry point for the Order Service application, hosting the API interfaces.
- **TSMWS.OrderService.Business**: Contains business logic and services used by the application.
- **TSMWS.OrderService.Data**: Manages data access and interactions with the underlying database.
- **TSMWS.OrderService.Configurations**: Holds configuration settings and service setup extensions.
- **TSMWS.OrderService.Shared**: Contains shared models and utilities used across different projects.

### Tests Folder

- **TSMWS.OrderService.Api.IntegrationTests**: Integration tests for the API project, ensuring end-to-end functionality.
- **TSMWS.OrderService.Business.UnitTests**: Unit tests for the Business layer, verifying business logic correctness.
- **TSMWS.OrderService.Data.UnitTests**: Unit tests for the Data layer, ensuring correct data access behavior.

## Docker Commands

### 1. Docker Compose Up with Rebuild

```bash
docker-compose -p tswms-orderservice-stack up --build
```
