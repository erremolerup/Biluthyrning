using Microsoft.AspNetCore.Mvc.Rendering;
using BiluthyrningAB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models.ViewModels
{
    public class CreateRentalVm
    {
        //public IEnumerable<SelectListItem> CarTypes { get; set; }
        //public Rental Rental { get; set; }

        public Rental Rental { get; set; }
        public List<SelectListItem> Cars { get; set; }
        public List<SelectListItem> Customers { get; set; }

    }
}
