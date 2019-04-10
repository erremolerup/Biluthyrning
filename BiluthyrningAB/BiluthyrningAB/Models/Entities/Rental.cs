using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models
{
    public class Rental
    {
        [Display(Name = "Bokningsnr")]
        public Guid RentalId { get; set; }

        [Display(Name = "Uthyrningsdatum")]
        [Required(ErrorMessage = "Ange uthyrningsdatum")]
        [FutureDate(ErrorMessage = "Datumet måste vara senare än dagens datum")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Återlämningsdatum")]
        [Required(ErrorMessage = "Ange återlämningsdatum")]
        public DateTime EndDate { get; set; }


        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                return value != null && (DateTime)value > DateTime.Now;
            }
        }
        public decimal NumberOfDays
        {
            get
            {
                return Convert.ToDecimal((EndDate - StartDate).TotalDays);
            }
        }

        [Display(Name = "Bilens antal körda km")]
        public int NewNumberKmDriven { get; set; }

        public Car Car { get; set; } //En bokning innehåller en bil
        public Guid CarId { get; set; }
        public Customer Customer { get; set; } //En bokning innehåller en customer
        public Guid CustomerId { get; set; }


        [Display(Name = "Kunden har kört i km")]
        public int NumberOfKm
        {
            get
            {
                if (Car == null)
                {
                    return 0;
                }

                return NewNumberKmDriven - Car.NumberOfDrivenKm;
            }
        }

        [Display(Name = "Pågående uthyrning")]
        public bool Ongoing { get; set; }

        private readonly decimal baseDayRental = 500;
        private readonly decimal kmPrice = 10;

        [Display(Name = "Pris")]
        public decimal Price
        {
            get
            {
                if (Car == null)
                {
                    return 0;
                }

                string[] carTypes = Enum.GetNames(typeof(CarType));

                if (carTypes.Single(x => x == "Small") == Car.CarType.ToString())
                {
                    return NumberOfDays * baseDayRental;
                    //return price;
                }
                else if (carTypes.Single(x => x == "Van") == Car.CarType.ToString())
                {
                    return (NumberOfDays * baseDayRental * 1.2m) + (kmPrice * NumberOfKm);
                    //return price;
                }
                else if (carTypes.Single(x => x == "MiniBus") == Car.CarType.ToString())
                {
                    return (NumberOfDays * baseDayRental * 1.7m) + (kmPrice * NumberOfKm * 1.5m);
                    //return price;
                }


                throw new Exception("Bilmodellen finns inte");
            }
        }

    }
}
