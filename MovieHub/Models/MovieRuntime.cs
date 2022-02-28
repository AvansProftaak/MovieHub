using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Models;

public class MovieRuntime
{
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime StartAt { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime EndAt { get; set; }

        [DataType(DataType.Time)]
        [Column(TypeName = "Time")]
        public TimeSpan Time { get; set; }

        [ForeignKey("MovieId")] 
        public virtual Movie Movie { get; set; } = null!;
        [ForeignKey("HallId")] 
        public virtual Hall Hall { get; set; } = null!;

        public MovieRuntime()
        {
                
        }

}