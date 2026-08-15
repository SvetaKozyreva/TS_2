using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_2.Models
{
    public class Event
    {
        [Key]
        public int EventsID { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        // Дата хранится как string, как у тебя в БД
        public string Date { get; set; } = "";

        public int MaxParticipants { get; set; }

        // Вартість події у гривнях
        public int Price { get; set; }

        public string Place { get; set; } = "";

        // Ці поля не зберігаються в БД,
        // вони потрібні тільки для відображення на сторінці
        [NotMapped]
        public string ParticipantsInfo { get; set; } = "";

        [NotMapped]
        public string PriceText { get; set; } = "";

        [NotMapped]
        public string ButtonText { get; set; } = "";
    }
}
