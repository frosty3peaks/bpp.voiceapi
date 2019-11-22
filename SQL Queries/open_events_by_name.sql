
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'voice_sp_open_events_by_name')
DROP PROCEDURE voice_sp_open_events_by_name
GO

CREATE PROCEDURE voice_sp_open_events_by_name
	@username as nvarchar(100)
AS

select 'There are ' + Convert(varchar(50),Count(RecordID)) + ' open events for ' + @username
from EventTrackingBase etb
where 
	etb.CurrentStatusIndex < 2900 -- 2900 = Closed
	and
	etb.CreatedByName = @username

GO