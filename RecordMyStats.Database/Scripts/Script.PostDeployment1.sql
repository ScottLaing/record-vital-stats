/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\PostDeployment\BloodPressure.sql
:r .\PostDeployment\BloodSugar.sql
:r .\PostDeployment\Member.sql
:r .\PostDeployment\Note.sql
:r .\PostDeployment\Session.sql
:r .\PostDeployment\StatisticEntry.sql


 --EXEC sp_generate_merge 'BloodPressure'
 --EXEC sp_generate_merge 'BloodSugar'
 --EXEC sp_generate_merge 'Member'

 --EXEC sp_generate_merge 'Note'
 --EXEC sp_generate_merge 'Session'
 --EXEC sp_generate_merge 'StatisticEntry'

