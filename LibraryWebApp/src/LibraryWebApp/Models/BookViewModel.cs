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


        [MaxLength(25)]
        public string FirstNameWritter { get; set; }

        [MaxLength(25)]
        public string LastNameWritter { get; set; }



    }
}
