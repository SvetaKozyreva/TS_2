using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_2.Models
{
    public class Training
    {
        [Key]
        public int TrainingID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public int MaxPlaces { get; set; }

        public string Description { get; set; }

        public int TrainerID { get; set; }
    }
}
