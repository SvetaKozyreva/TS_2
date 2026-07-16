using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_2.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Date { get; set; }

        public int MaxParticipants { get; set; }

        public double Price { get; set; }
    }
}
