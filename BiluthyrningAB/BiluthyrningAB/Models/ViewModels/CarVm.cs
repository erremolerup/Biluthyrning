using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using BiluthyrningAB.Models.ViewModels;

namespace BiluthyrningAB.Models.ViewModels
{
    public class CarVm
    {
        public Car Car { get; set; }
        public List<SelectListItem> CarTypes { get; set; }
    }
}
