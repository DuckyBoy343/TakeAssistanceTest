using SQLite;

namespace AssistanceApp.Models
{
    public class School
    {
        [PrimaryKey]
        public int SchoolId { get; set; }
        public string? Name { get; set; }
    }
}
