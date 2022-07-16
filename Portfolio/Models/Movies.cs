using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string Title { get; set; }

        //指定 DataType 為 Date，只需要顯示日期(時間不用)
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}
