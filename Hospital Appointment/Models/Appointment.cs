using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital_Appointment.Models
{
    public class Appointment
    {
        public int Id { get; set; }
		public string AppointmentId { get; set; }
        
        public string PatientId { get; set; }
		public string PatientName { get; set; }
		
		[Display(Name = "Start time")]
		[Required]
		public string startDate { get; set; }
		[Display(Name = "End time")]
		[Required]
		public string EndDate { get; set; }
		[Required(ErrorMessage = "Phone Number is required")]
		[RegularExpression("[0-9 ]{10}", ErrorMessage = "Phone number is Not Valid")]
		public string PhoneNo { get; set; }
		public string Address { get; set; }
		[Required(ErrorMessage = "please select")]
		[Display(Name = "Doctor Name")]
		public string DoctorName { get; set; }
		public string Description { get; set; }
		public SelectList Doctors { get; set; }
        public bool Attented { get; set; }
		public int patientNo { get; set; }
		public int DoctorId { get; set; }
		public int CreatedBy { get; set; }


		
	}
}