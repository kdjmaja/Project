using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{
    public class BookViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Text { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstNameWritter { get; set; }

        [Required]
        [MaxLength(25)]
        public string LastNameWritter { get; set; }

        [Required]
        public int Counter { get; set; }

        [Required]
        public string About { get; set; }
        
        public Genres Genre { get; set; }

        public SelectList StatusList { get; set; }
    }
}
