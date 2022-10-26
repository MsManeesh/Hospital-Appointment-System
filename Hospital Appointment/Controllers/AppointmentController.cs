using Hospital_Appointment.DAL;
using Hospital_Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital_Appointment.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        // GET: Appointment
        PatientDbHandler patientDb = new PatientDbHandler();
        UserDbHandler userDb = new UserDbHandler();
        AppointmentDbHandler appointmentDb = new AppointmentDbHandler();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int Id)
        {
            try
            {
                Patient currentPatient = patientDb.GetPatient(Id);

                User user = userDb.GetSingleUserDetails(HttpContext.User.Identity.Name);
                Appointment appointment = new Appointment();
                appointment.PatientId = currentPatient.PatientId;
                appointment.patientNo = currentPatient.Id;
                appointment.PatientName = currentPatient.Name;
                appointment.PhoneNo = currentPatient.PhoneNo;
                appointment.Address = currentPatient.Address;
                appointment.CreatedBy = user.Id;

                appointment.Doctors = new SelectList(getdropdown(), "Value", "Text");

                return View(appointment);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Appointment appointment)
        {

            try
            {
                appointment.DoctorId = Convert.ToInt32(appointment.DoctorName);
                if (ModelState.IsValid)
                {
                    if (appointmentDb.AddAppointment(appointment))
                    {
                        ModelState.Clear();
                        return RedirectToAction("Dasboard", "Home");
                    }
                    ModelState.AddModelError("", "Server Error");


                }
                appointment.Doctors = new SelectList(getdropdown(), "Value", "Text");
                return View(appointment);

            }
            catch (Exception ex)
            {
                appointment.Doctors = new SelectList(getdropdown(), "Value", "Text");
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View(appointment);
            }

        }
        private List<SelectListItem> getdropdown()
        {
            List<User> doctors = userDb.GetDoctors();
            List<SelectListItem> dropdown = new List<SelectListItem>();

            foreach (var doctor in doctors)
            {
                var item = new SelectListItem { Text = doctor.Name, Value = doctor.Id.ToString() };
                dropdown.Add(item);
            }
            return dropdown;
        }

        public JsonResult GetEvents()
        {

            List<Appointment> appointments = appointmentDb.GetAppointments();
            return new JsonResult { Data = appointments.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult AppointmentData()
        {
            try
            {
                var start = Convert.ToInt32(Request.Form["start"]);
                var length = Convert.ToInt32(Request.Form["length"]);
                var searchValue = Convert.ToString(Request.Form["search[value]"]);
                var sortColumnDirection = Request.Form["order[0][dir]"];
                var sortColumn = Request.Form["order[0][column]"];
                var count = appointmentDb.GetAppointmentCount();
                if (length < 0)
                    length = count;
                List<Appointment> Appointments = appointmentDb.GetAppointmentsDatatable(start, length, searchValue, sortColumn, sortColumnDirection);
                foreach (Appointment appointment in Appointments)
                {
                    appointment.PatientName = patientDb.GetPatient(appointment.patientNo).Name;
                    appointment.DoctorName = userDb.GetUser(appointment.DoctorId).Name;
                }
                return Json(new { data = Appointments, recordsTotal = count, recordsFiltered = count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return Json(null);
            }



        }

        public ActionResult Edit(int id)
        {
            try
            {
                Appointment appointment = new Appointment();
                appointment = appointmentDb.GetAppointment(id);
                appointment.Doctors = new SelectList(getdropdown(), "Value", "Text");
                if (appointment != null)
                {
                    Patient patientcurrent = patientDb.GetPatient(appointment.patientNo);
                    appointment.PatientId = patientcurrent.PatientId;
                    appointment.PatientName = patientcurrent.Name;
                    
                    return View(appointment);
                }

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Appointment appointment)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (appointmentDb.UpdateAppointment(appointment))
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index","Appointment");
                    }
                    ModelState.AddModelError("", "Server Error");
                }
                appointment.Doctors = new SelectList(getdropdown(), "Value", "Text");
                return View(appointment);


            }
            catch (Exception ex)
            {
                appointment.Doctors = new SelectList(getdropdown(), "Value", "Text");
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View(appointment);
            }




        }

        public ActionResult Delete(int id)
        {
            try
            {

                return View(appointmentDb.GetAppointment(id));
            }
            catch (Exception ex)
            {

                TempData["error"] = "<script>alert(" + ex.Message + " " + ex.StackTrace + ")</script>";
                return RedirectToAction("index");
            }

        }

        // POST: Patient/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Appointment appointment)
        {
            try
            {
                if (appointmentDb.DeleteAppointment(id))
                {
                     TempData["msg"] = "<script>alert('Patient and there Appointments are Deleted Successfully')</script>";
                     return RedirectToAction("Index");
                    
                    

                }
                ModelState.AddModelError("", "Unadle to Delete");
                return View(appointment);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View(appointment);
            }

        }

    }
}