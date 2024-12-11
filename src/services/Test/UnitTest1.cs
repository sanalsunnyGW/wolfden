using WolfDen.Domain.Entity;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void AttendanceCloseRecordAdd()
        {
        DateOnly PreviousAttendanceClosed = new DateOnly(2024,10,12);
        DateOnly AttendanceClosedDate = new DateOnly(2024, 10, 12);
        bool IsClosed = false;
        AttendenceClose attendanceClose = new AttendenceClose(AttendanceClosedDate,IsClosed,PreviousAttendanceClosed);
        Assert.Equal(PreviousAttendanceClosed, attendanceClose.PreviousAttendanceClosed);
        Assert.Equal(IsClosed, attendanceClose.IsClosed);
        Assert.Equal(AttendanceClosedDate, attendanceClose.AttendanceClosedDate);
        }
    }
}