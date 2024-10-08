﻿--MERGE generated by 'sp_generate_merge' stored procedure
--Originally by Vyas (http://vyaskn.tripod.com/code): sp_generate_inserts (build 22)
--Adapted for SQL Server 2008+ by Daniel Nolan (https://twitter.com/dnlnln)

SET NOCOUNT ON 

SET IDENTITY_INSERT [BloodSugar] ON

DECLARE @mergeOutput TABLE ( [DMLAction] VARCHAR(6) );
MERGE INTO [BloodSugar] AS [Target]
USING (VALUES
  (1,2004,1.100000000000000e+002,N'mg/dL',N'Fasting (overnite)',NULL,NULL,NULL,1,'2022-07-30T05:30:35.6633333','2022-07-30T05:30:35.6633333')
 ,(2,2004,1.130000000000000e+002,N'mg/dL',N'2 Hours After Meal',NULL,NULL,NULL,1,'2022-07-31T06:11:42.5433333','2022-07-31T06:11:42.5400000')
 ,(3,2004,1.210000000000000e+002,N'mg/dL',N'1 Hour After Meal',NULL,NULL,NULL,1,'2022-07-31T06:17:10.5033333','2022-07-31T06:17:10.5033333')
 ,(4,3041,1.100000000000000e+002,N'mg/dL',N'Fasting (overnite)',NULL,NULL,NULL,1,'2023-01-07T07:05:19.4000000','2023-01-07T07:05:13.8700000')
) AS [Source] ([Id],[MemberId],[Value],[Units],[WhenTaken],[Notes],[MoodLevel],[CustomTime],[IsActive],[CreateDate],[RecordingDate])
ON ([Target].[Id] = [Source].[Id])
WHEN MATCHED AND (
	NULLIF([Source].[MemberId], [Target].[MemberId]) IS NOT NULL OR NULLIF([Target].[MemberId], [Source].[MemberId]) IS NOT NULL OR 
	NULLIF([Source].[Value], [Target].[Value]) IS NOT NULL OR NULLIF([Target].[Value], [Source].[Value]) IS NOT NULL OR 
	NULLIF([Source].[Units], [Target].[Units]) IS NOT NULL OR NULLIF([Target].[Units], [Source].[Units]) IS NOT NULL OR 
	NULLIF([Source].[WhenTaken], [Target].[WhenTaken]) IS NOT NULL OR NULLIF([Target].[WhenTaken], [Source].[WhenTaken]) IS NOT NULL OR 
	NULLIF([Source].[Notes], [Target].[Notes]) IS NOT NULL OR NULLIF([Target].[Notes], [Source].[Notes]) IS NOT NULL OR 
	NULLIF([Source].[MoodLevel], [Target].[MoodLevel]) IS NOT NULL OR NULLIF([Target].[MoodLevel], [Source].[MoodLevel]) IS NOT NULL OR 
	NULLIF([Source].[CustomTime], [Target].[CustomTime]) IS NOT NULL OR NULLIF([Target].[CustomTime], [Source].[CustomTime]) IS NOT NULL OR 
	NULLIF([Source].[IsActive], [Target].[IsActive]) IS NOT NULL OR NULLIF([Target].[IsActive], [Source].[IsActive]) IS NOT NULL OR 
	NULLIF([Source].[CreateDate], [Target].[CreateDate]) IS NOT NULL OR NULLIF([Target].[CreateDate], [Source].[CreateDate]) IS NOT NULL OR 
	NULLIF([Source].[RecordingDate], [Target].[RecordingDate]) IS NOT NULL OR NULLIF([Target].[RecordingDate], [Source].[RecordingDate]) IS NOT NULL) THEN
 UPDATE SET
  [Target].[MemberId] = [Source].[MemberId], 
  [Target].[Value] = [Source].[Value], 
  [Target].[Units] = [Source].[Units], 
  [Target].[WhenTaken] = [Source].[WhenTaken], 
  [Target].[Notes] = [Source].[Notes], 
  [Target].[MoodLevel] = [Source].[MoodLevel], 
  [Target].[CustomTime] = [Source].[CustomTime], 
  [Target].[IsActive] = [Source].[IsActive], 
  [Target].[CreateDate] = [Source].[CreateDate], 
  [Target].[RecordingDate] = [Source].[RecordingDate]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([Id],[MemberId],[Value],[Units],[WhenTaken],[Notes],[MoodLevel],[CustomTime],[IsActive],[CreateDate],[RecordingDate])
 VALUES([Source].[Id],[Source].[MemberId],[Source].[Value],[Source].[Units],[Source].[WhenTaken],[Source].[Notes],[Source].[MoodLevel],[Source].[CustomTime],[Source].[IsActive],[Source].[CreateDate],[Source].[RecordingDate])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE
OUTPUT $action INTO @mergeOutput;

DECLARE @mergeError int
 , @mergeCount int, @mergeCountIns int, @mergeCountUpd int, @mergeCountDel int
SELECT @mergeError = @@ERROR
SELECT @mergeCount = COUNT(1), @mergeCountIns = SUM(IIF([DMLAction] = 'INSERT', 1, 0)), @mergeCountUpd = SUM(IIF([DMLAction] = 'UPDATE', 1, 0)), @mergeCountDel = SUM (IIF([DMLAction] = 'DELETE', 1, 0)) FROM @mergeOutput
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [BloodSugar]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[BloodSugar] rows affected by MERGE: ' + CAST(COALESCE(@mergeCount,0) AS VARCHAR(100)) + ' (Inserted: ' + CAST(COALESCE(@mergeCountIns,0) AS VARCHAR(100)) + '; Updated: ' + CAST(COALESCE(@mergeCountUpd,0) AS VARCHAR(100)) + '; Deleted: ' + CAST(COALESCE(@mergeCountDel,0) AS VARCHAR(100)) + ')' ;
 END
GO



SET IDENTITY_INSERT [BloodSugar] OFF
SET NOCOUNT OFF
GO

