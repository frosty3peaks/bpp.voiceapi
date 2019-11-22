
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'voice_sp_open_events_by_site')
DROP PROCEDURE voice_sp_open_events_by_site
GO
CREATE PROCEDURE voice_sp_open_events_by_site
	@sitename as nvarchar(100)
AS

select 'There are ' + Convert(varchar(50),Count(RecordID)) + ' open events at ' + @sitename
from v10_Warehouse_Rocket.dbo.EventTrackingBase etb
where 
	etb.CurrentStatusIndex < 2900 -- 2900 = Closed
	and
	etb.ReportingPointName = @sitename

GO