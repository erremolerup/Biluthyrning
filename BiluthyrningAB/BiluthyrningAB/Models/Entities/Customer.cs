using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Du måste ange förnamn")]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Du måste ange efternamn")]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Du måste ange kundens personnummer")]
        [Display(Name = "Kunds personnummer")]
        [RegularExpression("^[0-9]{8}-[0-9]{4}$", ErrorMessage = "Måste anges som följer: YYYYMMDD-XXXX")]
        public string SocSecNumber { get; set; }

        public List<Rental> Rentals { get; set; } //En kund kan ha flera bokningar
    }
}
