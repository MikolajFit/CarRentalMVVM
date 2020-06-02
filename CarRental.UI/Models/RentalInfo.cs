using System;
using System.Collections.Generic;
using System.Text;
using DDD.Base.DomainModelLayer.Models;

namespace CarRental.UI.Models
{
    public class RentalInfo
    {
        public Guid RentalId { get; set; }
        public string SelectedCar { get; set; }
        public decimal PricePerMinute { get; set; }
    }
}
