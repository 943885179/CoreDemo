using System.ComponentModel.DataAnnotations;

namespace CoreDemo_Sql.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}