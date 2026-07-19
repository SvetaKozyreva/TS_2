using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TS_2.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Login { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string Role { get; set; } = "";

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public string? Photo { get; set; }

        public string? Description { get; set; }

        public string? Specialization { get; set; }
    }
}
