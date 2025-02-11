# OutOfOffice

The app was created as a test project for the vacancy of Junior CRM Developer. Project was developed by Valentyn Renhach

## Table of Contents
- [Overview](#overview)
- [Authorization and roles opportunities](#authorization-and-roles-opportunities)
  - [Opportunities for Employee](#opportunities-for-employee)
  - [Opportunities for HR Manager](#opportunities-for-hr-manager)
  - [Opportunities for Project Manager](#opportunities-for-project-manager)
  - [Opportunities for Administrator](#opportunities-for-administrator)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation and startup](#installation-and-startup)
- [Gallery](#gallery)
  - [Entity Relationship diagram](#entity-relationship-diagram)
  - [Program screenshots](#program-screenshots)

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
  - View detailed information about employee under your supervision
- Project management
  - Sort, filter and search in the list of projects that your subordinates are part of
  - View detailed information about project that your subordinates are part of
- Leave request management
  - Sort, filter and search in the list of leave requests that you created and that are related to your subordinates
  - Create new leave requests
  - Cancel your leave requests
  - View detailed information about your leave request and your subordinates.
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
  - View detailed information about approval requests that are related to your leave request and participants of your project
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

> [!NOTE]  
> Due to the requirements for the program, there are no signup instruments. To use the app, we need to create an employee with an administrator position.
> 
> To do that you can execute this SQL script:
> `INSERT INTO public.employees (id, fullname, email, "password", salt, subdivision, "position", status, people_partner_id, out_of_office_balance) VALUES(1, 'Admin', 'admin@gmail.com', 'A12A92B450938BD7EA10061776CBAA5D7DB32F8D830024D982533B5D342D9508', 'A8A0ADFFF9A1DBF5F00321FB2692F33827439A6F320D75BBA8D3F28EED11100C', 0, 2, true, NULL, 84);`
> 
> It will create an administrator with the following credentials:
> - Email: admin@gmail.com
> - Password: 1111
> 
> You can use these credentials to login into the app.

## Gallery
### Entity Relationship diagram

![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/cd6638af-a11f-4d16-be2d-e42c0813470b)

### Program screenshots

Index page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/73f728cc-daee-4b11-940c-5b99d9a91f4e)

Login page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/34fb837e-be2d-4183-8ed0-cdd8d6bd5cc1)

Login page behaviour when wrong login credentials were entered
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/0963f597-fc1f-4101-b1e2-76380f065be6)

Employees page(Administrator view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/c2eb424a-ab45-4cab-807b-d055c9cbe5b5)

Employees page(HR manager view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/c751dcbf-4ad6-4a6a-bade-820ce3520d5f)

Employees page(Project manager view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/3d69ff67-76b9-4e69-943e-59a13d60447b)

> [!NOTE]  
> Project manager doesn't have same instruments as HR Manager and Administator

Employees page with applied filter
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/b49bef41-6d36-4f61-b7dd-b4ae3b1ae280)

Employees page with applied search
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/01102602-3f2b-4b51-8ced-fecbe1557a1a)

Employees page with applied sorting by descending for balance column
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/22165fdf-6b57-44fd-980d-94b1efff3d23)

Add employee page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/436c21ec-0e6a-442b-a63a-2f161a318380)

Add employee page with validation errors shown
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/00939b46-a44b-4407-a640-828f92705d5d)

Edit employee page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/7600d9c1-1e49-4f20-b65d-53def4632d6f)
> [!NOTE]  
> Form was autofilled with previous data

Employee details page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/c6c024c5-c1bd-4d8d-9e04-d482c75ea20f)

Projects page(Administrator view) 
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/545d80a3-b5f9-44a0-95ce-ade2b27c8ec5)

Projects page(Project manager view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/fcac87ab-b959-4e94-803e-409ca398ed74)

Projects page(HR manager view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/6537d74a-8188-4c50-989e-84f8daaed60e)

Projects page(Employee view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/07ef8638-3dec-4f14-b2bb-54501a7e1917)

> [!NOTE]  
> HR manager and Employee don't have same instruments as Administator and Project Manager

Projects page with applied filter
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/db12fd6d-ccf1-44dc-b3b1-b63554ca9b9b)

Projects page with applied search
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/04501899-5497-4203-9203-b685e5f23ab6)

Projects page with applied sorting by ascending for Start date column
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/c0be9cfc-901e-4852-8989-707c7c6113c7)

Create project page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/0734a98f-c02b-48b1-94f7-b3553467287d)

Create project with validation errors shown
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/6edb3242-b5c6-4c60-b3cc-766a0809ff3d)

Edit project page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/4c9b9721-039f-423f-bcb1-aeff1e612276)
> [!NOTE]  
> Form was autofilled with previous data

Project details page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/e7560aee-88b0-44a2-b251-7b896b45f936)

Employee assignment
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/d9f4f869-12cd-4b0f-a7f4-3dddb4cf132d)

Leave requests page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/74fc8358-161f-4bcc-abb8-1c91cf470fdb)

Leave requests page with applied filter
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/76614e68-b080-4c35-9772-af75f8089dd5)

Leave requests page with applied search
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/1fcb4fa5-a120-4171-a556-57f8db5de9f7)

Leave requests page with applied sorting by descending for status column
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/cbc6cb97-e77e-45b9-94e6-95b2202efe5d)

Create leave request page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/37c39868-8163-44c8-b3d7-a4ba06bd561e)

Create leave request page behaviour when requested leave days are greater that out of office balance
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/45a1b673-8036-4696-b928-d3997ca4dafe)

Approval requests page(Administrator view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/4b007eb5-78dc-4e70-abbc-12c4196d48c1)

Approval requests page(HR manager view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/7ea0890d-f2bf-4cf2-95b7-2268adf95a6e)

Approval requests page(Project manager view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/3d1d53d6-f122-4258-bc08-d7f7d1fc16e4)

Approval requests page(Employee view)
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/3e50d7fd-464a-406d-a9ef-d6a570ac984e)

Approval requests page with appllied filter
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/48d73aaf-1346-46d9-9d63-666b1a4f62d4)

Approval request page with applied search
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/3b6a8751-1481-4367-9373-18216790abdd)

Approval request page with applied sorting by ascending for end status column
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/37bdc470-82d1-4028-8e4e-941215f240c9)

Create approval request page
![image](https://github.com/Right9lt/OutOfOffice/assets/40607069/9bb40609-456f-42f9-880c-f0064a2d914b)
