using WolfDen.Application.Models;

namespace WolfDen.Application.Constants
{
    public static class SyncConfigs
    {
        public static Dictionary<string, TableSyncConfig> GetSyncConfigs() => new()
        {
            ["Employee"] = new TableSyncConfig
            {
                SourceQuery = @"                   
                    SELECT NumericCode AS EmployeeCode, EmployeeRFIDNumber as RFId , EmployeeName AS FirstName 
                    FROM Employees
                    WHERE NumericCode > 1000
                    AND ISNUMERIC(EmployeeCode) = 1",
                DestinationTable = "EmployeeStaging",
                TruncateDestination = true,
                PreSyncDestQuery = @"
                        SET IDENTITY_INSERT wolfden.Device  ON 
                        ;with cte AS 
                        (
                         SELECT 14 AS DeviceId,	'FRONT ENTRANCE' AS [Name]
                        UNION ALL SELECT 20,	'FRONT EXIT'
                        UNION ALL SELECT 24,	'PANTRY EXIT'
                        UNION ALL SELECT 25,	'PANTRY ENTRANCE'	
                        )
                        INSERT INTO wolfden.Device (DeviceId,[Name])
                        SELECT c.* FROM cte c 
                        LEFT JOIN wolfden.Device d 
	                        ON c.DeviceId = d.DeviceId
                        WHERE d.DeviceId IS NULL 

                        SET IDENTITY_INSERT wolfden.Device  OFF
                        IF OBJECT_ID('EmployeeStaging' ) IS  NULL
                        BEGIN 
	                        CREATE TABLE EmployeeStaging (
	                        [EmployeeCode] [int] NULL,
	                        [RFId] [nvarchar](100) NULL,
	                        [FirstName] [nvarchar](100) NULL,
                        ) 
                        END
                    ",
                PostSyncQuery = @"
                    MERGE INTO wolfden.Employee AS Target
                    USING EmployeeStaging AS Source
                    ON Target.EmployeeCode = Source.EmployeeCode
                    WHEN MATCHED THEN
                        UPDATE SET 
                            Target.RFID = Source.RFID
                    WHEN NOT MATCHED BY TARGET THEN
                        INSERT (EmployeeCode, RFID, FirstName)
                        VALUES (Source.EmployeeCode, Source.RFID, Source.FirstName);
                    "
            },
            ["DailyAttendance"] = new TableSyncConfig
            {
                SourceQuery = @"                   
                    SELECT UserId
                        ,[Date]
                        ,FirstInTime
                        ,LastOutTime
                        ,InsideOfficeHrs
                        ,Pantry
                        ,OutOff
                        ,MissedPunches 
                FROM DailyAttentandceReport
               -- WHERE CreatedOn = CAST (GETDATE() AS DATE )                  
",
                DestinationTable = "DailyAttentandceStaging",
                TruncateDestination = true,
                PreSyncSrcQuery = @"
                    DECLARE @dt DATETIME = GETDATE() -1;
                    EXEC GetAttendance @dt
                    SET @dt = @dt + 1 
                    EXEC GetAttendance @dt  
                ",
                PreSyncDestQuery = @"
                    IF OBJECT_ID('DailyAttentandceStaging' ) IS  NULL
                    BEGIN 
	                    CREATE TABLE DailyAttentandceStaging (
	                    [UserId] [int] NULL,
	                    [Date] [date] NULL,
	                    [FirstInTime] [datetime] NULL,
	                    [LastOutTime] [datetime] NULL,
	                    [InsideOfficeHrs] [int] NULL,
	                    [Pantry] [int] NULL,
	                    [OutOff] [int] NULL,
	                    [MissedPunches] [nvarchar](max) NULL,
                    ) 
                    END
                    ",
                PostSyncQuery = @"
                   MERGE INTO wolfden.DailyAttendence AS Target
                USING (
                    SELECT 
                        e.Id AS EmployeeId,
                        s.Date,
                        s.FirstInTime,
                        s.LastOutTime,
                        s.InsideOfficeHrs,
                        s.Pantry,
                        s.OutOff,
                        s.MissedPunches
                    FROM DailyAttentandceStaging AS s
                    INNER JOIN wolfden.Employee AS e
                        ON s.UserId = e.EmployeeCode
                ) AS Source
                ON Target.EmployeeId = Source.EmployeeId AND Target.Date = Source.Date
                WHEN MATCHED THEN
                    UPDATE SET 
                        Target.ArrivalTime = Source.FirstInTime,
                        Target.DepartureTime = Source.LastOutTime,
                        Target.InsideDuration = Source.InsideOfficeHrs,
                        Target.OutsideDuration = Source.OutOff,
                        Target.PantryDuration = Source.Pantry,
                        Target.MissedPunch = Source.MissedPunches
                WHEN NOT MATCHED BY TARGET THEN
                    INSERT (
                        EmployeeId, 
                        Date, 
                        ArrivalTime, 
                        DepartureTime, 
                        InsideDuration, 
                        OutsideDuration, 
                        PantryDuration, 
                        MissedPunch
                    )
                    VALUES (
                        Source.EmployeeId, 
                        Source.Date, 
                        Source.FirstInTime, 
                        Source.LastOutTime, 
                        Source.InsideOfficeHrs, 
                        Source.OutOff, 
                        Source.Pantry, 
                        Source.MissedPunches
                    );

                    "
            },
        };
    }
}
