using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models
{
    public class Car
    {
        public Guid CarId { get; set; }

        [Required(ErrorMessage = "Du måste ange biltyp")]
        [Display(Name = "Biltyp")]
        public CarType CarType { get; set; }

        [Required(ErrorMessage = "Du måste ange registreringsnummer")]
        [RegularExpression("[A-ZÅÄÖ]{3}[0-9]{3}", ErrorMessage = "Måste anges i formatet ABC123")]
        [Display(Name = "Registreringsnummer")]
        public string NumberPlate { get; set; }

        [Display(Name = "Totalt antal körda mil")]
        public int NumberOfDrivenKm { get; set; }

        public List<Rental> Rentals { get; set; } //En bil kan ha flera bokningar
    }

    public enum CarType
    {
        Small,
        Van,
        MiniBus
    };
}
