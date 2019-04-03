using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.ViewModels
{
    public class CustomerVm
    {
        public Customer Customer { get; set; }
        public Car Car { get; set; }
        public Rental Rental { get; set; }
        public IEnumerable<Rental> GetRentalsByCustomerId { get; set; }
    }
}
