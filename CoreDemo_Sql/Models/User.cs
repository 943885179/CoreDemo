using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Sql.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        //public int RoleId { get; set; }
    }
}
