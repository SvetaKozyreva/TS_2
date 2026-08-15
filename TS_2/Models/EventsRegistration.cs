using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace TS_2.Models
{
    public class EventsRegistration
    {
        [Key]
        public int EventsRegistrationID { get; set; }

        public int UserID { get; set; }

        public int EventID { get; set; }

        public string RegistrationDate { get; set; }
    }
}
