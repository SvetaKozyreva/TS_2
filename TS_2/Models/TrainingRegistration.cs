using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_2.Models
{
    public class TrainingRegistration
    {
        [Key]
        public int RegistrationID { get; set; }

        public int UserID { get; set; }

        public int TrainingID { get; set; }

        public string RegistrationDate { get; set; }

        public string Status { get; set; }
    }
}
