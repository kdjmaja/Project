using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{
    public class ShoppingCart
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
       // public List<Posudba> Cart { get; set; }
        public double DeliveryPrice { get; set; }
        public double SumOfBooksPrices { get; set; }
        public double Sum { get; set; }

        public ShoppingCart(Guid userid,string username)
        {
            UserId = userid;
            Username = username;
            //Cart=new List<Posudba>();
            DeliveryPrice = 10;
            SumOfBooksPrices = 0;
            Sum = 0;
        }
    }
}
