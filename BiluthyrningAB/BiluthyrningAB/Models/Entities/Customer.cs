using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BiluthyrningAB.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Du måste ange förnamn")]
        [Display(Name = "Förnamn")]
        public string FirstName
        {
            get; set;
           
        }

        [Required(ErrorMessage = "Du måste ange efternamn")]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Du måste ange kundens personnummer")]
        [Display(Name = "Kunds personnummer")]
        [RegularExpression("^[0-9]{8}-[0-9]{4}$", ErrorMessage = "Måste med 10 siffror och anges som följer: YYYYMMDD-XXXX")]
        [ValidateOfAge]
        public string SocSecNumber { get; set; }

        [Display(Name = "Kundens tidigare och nuvarande uthyrningar")]
        public List<Rental> Rentals { get; set; } //En kund kan ha flera bokningar

        internal class ValidateOfAge : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string RegExForValidation = @"^(?<date>\d{6}|\d{8})[-\s]?\d{4}$";
                string date = Regex.Match((string)value, RegExForValidation).Groups["date"].Value;
                DateTime dt;
                if (DateTime.TryParseExact(date, new[] { "yyMMdd", "yyyyMMdd" }, new CultureInfo("sv-SE"), DateTimeStyles.None, out dt))
                    if (IsOfAge(dt))
                        return ValidationResult.Success;
                    else
                    {
                        return new ValidationResult("Personen som hyr bil måste vara över 18 år");

                    }
                return new ValidationResult("Personnummer anges med 10 siffror (YYYYMMDD-XXXX)");
            }

            public bool IsOfAge(DateTime birthdate)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - birthdate.Year;
                if (birthdate > today.AddYears(-age))
                    age--;
                return age < 18 ? false : true;
            }
        }

    }
}
