
# Record-Vital-Stats project

## Project Description

Record-Vital-Stats project is a windows desktop project to consume a web api, to save and retrieve vital stats.  Both WPF front end and
web api back end are included in the project code.  
The project is written in C# and uses a SQL Server or Postgres database.  
The project is being developed in Visual Studio 2022.  It is being developed in C# .net core 8.0.  
This is currently a windows application only, WPF front end. A later version may support Angular or web front end also.

## Table of Contents

* [Project Description](#project-description)
* [Features](#features)
* [Installation](#installation)
* [Environment strings](#env)
* [Usage](#usage)
* [Contributing](#contributing)
* [License](#license)

## Project Description  <a name="project-description"></a>

The project is a windows desktop project to consume a web api, to save and retrieve vital stats. Early version of the project, lots of mods ongoing.  

## Features  <a name="features"></a>

* List of features 
* ---------------- 
* New User add window.
* Login window.
* Main window with menu.
* Blood sugar entry window.
* Blood sugar list window.
* Combined stats entry window.
* Combined stats list window.
* Notes entry page.
* Notes list page.
* Authentication tracking with JWT token.
* Profile window.
* Logout functionality.


## Installation and Setup  <a name="installation"></a>

Dev Notes for installation and setup. 

* Using Visual Studio full version is recommended, currently Visual Studio 2022.
* nuget libraries should self install during the build process.
* Install sql server or postgres for your database, depending on preference. Can use LocalDb for development also.
* Create a local database in SQL Server or Postgres.  See the database scripts in the Database project.

## Usage  <a name="usage"></a>

For local development you will need to:

* Modify the properties in your visual studio solution to have these as your startup projects: RecordMyStats (wpf), RecordMyStats.WebApi2 (backend web api).
* With this solution setup you can use the "Start" button in Visual Studio to run the project.


## Environment Variables  <a name="env"></a>

You will need to setup the following environment strings for development.  Add these from windows using Edit the system environment variables.  

(You can add these to a file for tracking but they should not be committed to your branch.  Add such a file to .gitignore.)

RMS_JWT_TokenPhrase - create a long string for the unique token phrase.  This is used to create the JWT token.
RMSAzureDBConnString - connection string for the Azure SQL Server database.
RMSAzureWebApiAddress - the address of the web api on Azure.
RMSLocalDbConnString - connection string for the local SQL Server database.
RMSLocalPostgresConnString - connection string for the local Postgres database.
RMSDefaultSalt - 36 char salt string.
RMSDefaultEncryptionKey - 36 char encryption key string.

## Contributing  <a name="contributing"></a>

* Contributions welcome.  Please fork the project and submit a pull request.

## License  <a name="license"></a>

MIT License

## Contact

If you have any questions about this project, feel free to contact me at scottlaing@gmail.com.
