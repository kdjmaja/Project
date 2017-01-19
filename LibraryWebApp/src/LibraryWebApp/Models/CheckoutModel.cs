using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{
    public class CheckoutModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Adress { get; set; }

        public List<Posudba> Kosarica { get; set; }

        [Required]
        public string Phone { get; set; }


        public int Quantity { get; set; }

    }
}
