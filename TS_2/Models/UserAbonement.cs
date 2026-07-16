using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_2.Models
{
    public class UserAbonement
    {
        [Key]
        public int UserAbonementID { get; set; }

        public int UserID { get; set; }

        public int AbonementID { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int RemainingVisits { get; set; }
    }
}
