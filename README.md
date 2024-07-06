# OutOfOffice

The app was created as a test project for the vacancy of Junior CRM Developer. Project was developed by Valentyn Renhach

## Table of Contents



## Overview

The app allows to manage employees, leave requests, approval requests and projects.

## Authorization and roles opportunities

Authorization is implemented using cookies, which store information about a user's role (position) and ID. There are four roles:

- Employee
- HR Manager
- Project Manager
- Administrator

Separation of roles ensures that sensitive data and tools are only accessible to those who need them, reducing the risk of misuse.

### Opportunities for Employee
As an Employee, you have the following opportunities:
- Project management
  - Sort, filter and search in the list of projects that you are a part of
  - View details of projects where you are participating
- Leave request management
  - Sort, filter and search in the list of leave requests that you created
  - Create new leave requests
  - Cancel your leave requests
  - View detailed information about your leave request
- Approval request management
  - Sort, filter and search the list of approval requests that are related to your leave requests
  - View detailed information about approval request that is related to your leave request

### Opportunities for HR Manager
As an HR Manager, you have the following opportunities:
- Employee management
  - Sort, filter and search in the list of employees that you have under your supervision
  - Add new employees
  - Update employees who are under your supervision
  - Deactivate employees under your supervision
  - View detailed information about your employee under your supervision
- Project management
  - Sort, filter and search in the list of projects that your subordinates are part of
  - View detailed information about project that your subordinates are part of
- Leave request management
  - Sort, filter and search in the list of leave requests that you created and that are related to your subordinates
  - Create new leave requests
  - Cancel your leave requests
  - View detailed information about your leave request or your subordinates.
- Approval request management
  - Sort, filter and search the list of approval requests that are related to your leave requests and your subordinates
  - View detailed information about approval requests that are related to your leave request or your subordinates
  - Create approval requests that are meant to resolve leave requests of your subordinates

### Opportunities for Project Manager
As a Project Manager, you have the following opportunities:
- Employee management
  - Sort, filter and search in the list of employees that are part of your project
  - Assign new employees to your project
  - Remove employees from your project
  - View detailed information about employees that are part of your project
- Project management
  - Sort, filter and search in the list of projects that you are part of
  - Create new projects
  - Update your projects
  - Delete projects that you are part of
  - Deactivate your project
  - View detailed information about project that you are part of
- Leave request management
  - Sort, filter and search in the list of leave requests that you created and that are related to employees who are participating in your projects
  - Create new leave requests
  - Cancel your leave requests
  - View detailed information about your leave request and employees who are participating in your projects
- Approval request management
  - Sort, filter and search the list of approval requests that are related to your leave requests and employees who are participating in your project
  - View detailed information about approval requests that are related to your leave request or participants of your project
  - Create approval requests that are meant to resolve leave requests of participants of your project
 
### Opportunities for Administrator
As an Administrator, you have opportunities of the previous roles combined.

## Getting Started

### Prerequisites

To run the project you need following tools:
- PostgreSQL
- Visual Studio IDE that supports .NET 8 and have ASP.NET expansion installed

### Installation and startup
1. Clone the repository
2. Execute this script [this SQL script](./SQL_script.sql) to create database
3. Open solution with Visual Studio IDE
4. Open `appsettings.json` and replace database connection string with your values
5. Run the solution

## Gallery
### Table relation diagram

![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/cd6638af-a11f-4d16-be2d-e42c0813470b)

