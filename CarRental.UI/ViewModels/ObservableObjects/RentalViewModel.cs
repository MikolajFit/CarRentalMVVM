using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.UI.ViewModels.ObservableObjects
{
    public class RentalViewModel
    {
        public Guid RentalId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? StopDateTime { get; set; }
        public decimal Total { get; set; }
        public string DriverName { get; set; }
        public string PricePerMinute { get; set; }
    }
}
