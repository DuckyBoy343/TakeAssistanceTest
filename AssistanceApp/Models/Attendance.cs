using SQLite;

namespace AssistanceApp.Models
{
    public class Attendance
    {
        [PrimaryKey, AutoIncrement]
        public int AttendanceId { get; set; }

        [Indexed]
        public int StudentId { get; set; }

        public DateTime ClassDate { get; set; }
        public bool IsPresent { get; set; }
    }
}
