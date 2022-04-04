using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class MovieRuntime
{
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime StartAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime EndAt { get; set; }

        [DisplayFormat(DataFormatString="{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [Column(TypeName = "Time")]
        public TimeSpan Time { get; set; }

        [ForeignKey("MovieId")] 
        public virtual Movie Movie { get; set; } = null!;
        [ForeignKey("HallId")] 
        public virtual Hall Hall { get; set; } = null!;

}