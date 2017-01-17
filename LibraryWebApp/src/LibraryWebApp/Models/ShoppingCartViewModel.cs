using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{
    public class ShoppingCartViewModel
    {
        [Required]
        public List<Posudba> InCart { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        public double DeliveryPrice { get; set; }
        [Required]
        public double SumOfBooksPrices { get; set; }
        [Required]
        public double Sum { get; set; }
    }
}
