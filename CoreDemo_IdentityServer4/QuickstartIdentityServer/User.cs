using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickstartIdentityServer
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string SubjectId { get; set; }
        public string Username { get; set; }
        public string Passworld { get; set; }

       // public bool IsActive { get; set; }//是否可用

       // public virtual ICollection<Claims> Claims { get; set; }
    }
}