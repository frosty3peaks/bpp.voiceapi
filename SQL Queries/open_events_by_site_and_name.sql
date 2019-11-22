
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'voice_sp_open_events_by_site_and_name')
DROP PROCEDURE voice_sp_open_events_by_site_and_name
GO

CREATE PROCEDURE voice_sp_open_events_by_site_and_name
	@sitename as nvarchar(100),
	@username as nvarchar(100)
AS

select 'There are ' + Convert(varchar(50),Count(RecordID)) + ' open events at ' + @sitename + ', that were reported by ' + @username
from v10_Warehouse_Rocket.dbo.EventTrackingBase etb
where 
	etb.CurrentStatusIndex < 2900 -- 2900 = Closed
	and
	etb.CreatedByName = @username
	and 
	etb.ReportingPointName = @sitename

GO