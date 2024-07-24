
# Record-Vital-Stats project

## Project Description

Record-Vital-Stats project is a windows desktop project to consume a web api, to save and retrieve vital stats. Both WPF front end and web api back end projects are included in the solution. The project is written in C# and can use a SQL Server or Postgres database for the stats. The project is being developed in Visual Studio 2022.  Most projects are using .net core 6.0. Note: this is currently a windows application only, with a WPF front end. A later version may support Angular or a web front end also.

## Table of Contents

* [Project Description](#project-description)
* [Features](#features)
* [Installation and Setup](#installation)
* [Environment strings](#env)
* [Usage](#usage)
* [Contributing](#contributing)
* [License](#license)

## Project Description  <a name="project-description"></a>

The project is a windows desktop project to consume a web api, to save and retrieve vital stats. Early version of the project, lots of mods ongoing.  

## Features (not all-inclusive) <a name="features"></a>

* New User add/register window. (WPF)
* Login window. (WPF) 
* Main window with menu.  (WPF)
* Blood sugar entry window.  (WPF)
* Blood sugar entries list window.  (WPF)
* Blood pressure entry window.  (WPF)
* Blood pressure entries list window.  (WPF)
* Oxygen level entry window.  (WPF)
* Oxygen level entries list window.  (WPF)
* Notes entry page.  (WPF)
* Notes list page.  (WPF)
* Back end controllers for login and authentication. (asp.net, Web Api, .net core)
* Back end controllers for new entries. (asp.net, Web Api, .net core)
* Back end controllers getting previous entries lists. (asp.net, Web Api, .net core)
* Back end database inserts and lookups, CRUD. (C# library)
* Database tracking projects. (C# library)
* Site authentication tracking logic using JWT token. (asp.net Web Api, .net core)
* Profile window.  (WPF)
* Logout functionality.  (WPF)


## Installation and Setup  <a name="installation"></a>

Dev Notes for installation and setup. 

* Using Visual Studio full version is recommended, currently Visual Studio 2022.
* nuget libraries should self install during the build process.
* Install sql server or postgres for your database, depending on preference. Can use LocalDb for development also.
* Create a local database in SQL Server or Postgres.  See the database scripts in the Database project.

## Usage  <a name="usage"></a>

For local development you will need to:

* Modify the properties in your visual studio solution to have these as your startup projects: **RecordMyStats** (wpf front end), **RecordMyStats.WebApi2** (backend web api).
* With this solution setup you can just use the "Start" button in Visual Studio to run the project.


## Environment Variables  <a name="env"></a>

You will need to setup the following environment strings for development.  Add these from windows using Edit the system environment variables.  

(You can add these to a file for tracking but they should not be committed to your branch.  Add such a file to .gitignore.)

* **RMS_JWT_TokenPhrase** - create a long string for the unique token phrase.  This is used to create the JWT token.
* **RMSAzureDBConnString** - connection string for the Azure SQL Server database.
* **RMSAzureWebApiAddress** - the address of the web api on Azure.
* **RMSLocalDbConnString** - connection string for the local SQL Server database.
* **RMSLocalPostgresConnString** - connection string for the local Postgres database.
* **RMSDefaultSalt** - 36 char salt string.
* **RMSDefaultEncryptionKey** - 36 char encryption key string.

## Contributing  <a name="contributing"></a>

* Contributions welcome.  Please fork the project and submit a pull request.

## License  <a name="license"></a>

MIT License

## Contact

If you have any questions about this project, feel free to contact me at <a href="mailto:scottlaing@gmail.com">scottlaing@gmail.com</a>.
