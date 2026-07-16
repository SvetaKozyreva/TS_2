using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_2.Models
{
    public class Abonement
    {
        [Key]
        public int AbonementID { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Visits { get; set; }

        public int DurationDays { get; set; }
    }
}
