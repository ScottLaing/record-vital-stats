﻿--MERGE generated by 'sp_generate_merge' stored procedure
--Originally by Vyas (http://vyaskn.tripod.com/code): sp_generate_inserts (build 22)
--Adapted for SQL Server 2008+ by Daniel Nolan (https://twitter.com/dnlnln)

SET NOCOUNT ON

SET IDENTITY_INSERT [StatisticEntry] ON

DECLARE @mergeOutput TABLE ( [DMLAction] VARCHAR(6) );
MERGE INTO [StatisticEntry] AS [Target]
USING (VALUES
  (1,2004,'2022-07-04T15:50:05.4533333',1.100000000000000e+002,3.050000000000000e+002,135,85,90,N'lb',N'mg/dL')
 ,(2,2004,'2022-07-04T15:50:24.6666667',1.300000000000000e+002,2.900000000000000e+002,140,80,90,N'lb',N'mg/dL')
 ,(3,2004,'2022-07-04T17:43:50.9333333',1.100000000000000e+002,3.050000000000000e+002,135,85,90,N'lb',N'mg/dL')
) AS [Source] ([Id],[MemberId],[CreateDate],[BloodSugar],[Weight],[BPSystolic],[BPDiastolic],[HeartRate],[WeightUnits],[BSUnits])
ON ([Target].[Id] = [Source].[Id])
WHEN MATCHED AND (
	NULLIF([Source].[MemberId], [Target].[MemberId]) IS NOT NULL OR NULLIF([Target].[MemberId], [Source].[MemberId]) IS NOT NULL OR 
	NULLIF([Source].[CreateDate], [Target].[CreateDate]) IS NOT NULL OR NULLIF([Target].[CreateDate], [Source].[CreateDate]) IS NOT NULL OR 
	NULLIF([Source].[BloodSugar], [Target].[BloodSugar]) IS NOT NULL OR NULLIF([Target].[BloodSugar], [Source].[BloodSugar]) IS NOT NULL OR 
	NULLIF([Source].[Weight], [Target].[Weight]) IS NOT NULL OR NULLIF([Target].[Weight], [Source].[Weight]) IS NOT NULL OR 
	NULLIF([Source].[BPSystolic], [Target].[BPSystolic]) IS NOT NULL OR NULLIF([Target].[BPSystolic], [Source].[BPSystolic]) IS NOT NULL OR 
	NULLIF([Source].[BPDiastolic], [Target].[BPDiastolic]) IS NOT NULL OR NULLIF([Target].[BPDiastolic], [Source].[BPDiastolic]) IS NOT NULL OR 
	NULLIF([Source].[HeartRate], [Target].[HeartRate]) IS NOT NULL OR NULLIF([Target].[HeartRate], [Source].[HeartRate]) IS NOT NULL OR 
	NULLIF([Source].[WeightUnits], [Target].[WeightUnits]) IS NOT NULL OR NULLIF([Target].[WeightUnits], [Source].[WeightUnits]) IS NOT NULL OR 
	NULLIF([Source].[BSUnits], [Target].[BSUnits]) IS NOT NULL OR NULLIF([Target].[BSUnits], [Source].[BSUnits]) IS NOT NULL) THEN
 UPDATE SET
  [Target].[MemberId] = [Source].[MemberId], 
  [Target].[CreateDate] = [Source].[CreateDate], 
  [Target].[BloodSugar] = [Source].[BloodSugar], 
  [Target].[Weight] = [Source].[Weight], 
  [Target].[BPSystolic] = [Source].[BPSystolic], 
  [Target].[BPDiastolic] = [Source].[BPDiastolic], 
  [Target].[HeartRate] = [Source].[HeartRate], 
  [Target].[WeightUnits] = [Source].[WeightUnits], 
  [Target].[BSUnits] = [Source].[BSUnits]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([Id],[MemberId],[CreateDate],[BloodSugar],[Weight],[BPSystolic],[BPDiastolic],[HeartRate],[WeightUnits],[BSUnits])
 VALUES([Source].[Id],[Source].[MemberId],[Source].[CreateDate],[Source].[BloodSugar],[Source].[Weight],[Source].[BPSystolic],[Source].[BPDiastolic],[Source].[HeartRate],[Source].[WeightUnits],[Source].[BSUnits])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE
OUTPUT $action INTO @mergeOutput;

DECLARE @mergeError int
 , @mergeCount int, @mergeCountIns int, @mergeCountUpd int, @mergeCountDel int
SELECT @mergeError = @@ERROR
SELECT @mergeCount = COUNT(1), @mergeCountIns = SUM(IIF([DMLAction] = 'INSERT', 1, 0)), @mergeCountUpd = SUM(IIF([DMLAction] = 'UPDATE', 1, 0)), @mergeCountDel = SUM (IIF([DMLAction] = 'DELETE', 1, 0)) FROM @mergeOutput
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [StatisticEntry]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[StatisticEntry] rows affected by MERGE: ' + CAST(COALESCE(@mergeCount,0) AS VARCHAR(100)) + ' (Inserted: ' + CAST(COALESCE(@mergeCountIns,0) AS VARCHAR(100)) + '; Updated: ' + CAST(COALESCE(@mergeCountUpd,0) AS VARCHAR(100)) + '; Deleted: ' + CAST(COALESCE(@mergeCountDel,0) AS VARCHAR(100)) + ')' ;
 END
GO



SET IDENTITY_INSERT [StatisticEntry] OFF
SET NOCOUNT OFF
GO
