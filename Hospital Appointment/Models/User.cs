using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hospital_Appointment.Models
{
    public class User
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [RegularExpression("[A-Za-z .]{3,50}", ErrorMessage = "Name Not Valid")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of birth is Required")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm//yyyy}")]
        
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Plese Select")]
        public string Department { get; set; }
        [Required(ErrorMessage = "Plese Select")]
        public string Designation { get; set; }
        public string Specialisation { get; set; }
        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString ="{0:dd/mm//yyyy}")]
        public DateTime DateofJoining { get; set; }


        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression("[0-9 ]{10}", ErrorMessage = "Phone number is Not Valid")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("Password", ErrorMessage = "Passwords are not matching")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public Boolean Admin { get; set; }
    }
}