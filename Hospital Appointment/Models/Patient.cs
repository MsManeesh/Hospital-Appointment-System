using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hospital_Appointment.Models
{
    public class Patient
    {
        public int Id { get; set; }

        public string PatientId { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [RegularExpression("[A-Za-z ]{3,50}", ErrorMessage = "Name Not Valid")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of birth is Required")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm//yyyy}")]

        public DateTime Dob { get; set; }
        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Adress is Required")]
        public string Address { get; set; }
       
        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression("[0-9 ]{10}", ErrorMessage = "Phone number is Not Valid")]
        public string PhoneNo { get; set; }

        
        [EmailAddress]
        public string Email { get; set; }
        public int CreatedBy { get; set; }
       

        
    }
}