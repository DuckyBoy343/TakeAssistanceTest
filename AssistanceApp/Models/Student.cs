using SQLite;

namespace AssistanceApp.Models
{
    public class Student
    {
        [PrimaryKey]
        public int StudentId { get; set; }
        public string? FullName { get; set; }
        [Indexed]
        public int GradeId { get; set; }
    }
}
