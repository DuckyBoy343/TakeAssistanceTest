using SQLite;

namespace AssistanceApp.Models
{
    public class Grade
    {
        [PrimaryKey]
        public int GradeId { get; set; }
        public string? Name { get; set; }
        [Indexed]
        public int SchoolId { get; set; }
    }
}
