using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{
    public class PosudbaViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string DateBorrowed { get; set; }
        [MaxLength(50)]
        public string ReturnDay { get; set; }



    }
}
