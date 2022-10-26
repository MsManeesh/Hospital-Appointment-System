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
    public class PatientController : Controller
    {
        // GET: Patient
        PatientDbHandler patientDb=new PatientDbHandler();
        UserDbHandler userDb=new UserDbHandler();
        AppointmentDbHandler appointmentDb=new AppointmentDbHandler();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            try
            {
                string currentUser = HttpContext.User.Identity.Name;
                User currentuser = userDb.GetSingleUserDetails(currentUser);
                Patient patient = new Patient();
                patient.CreatedBy = currentuser.Id;
                return View(patient);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Server Error");
                return View();

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {

            try
            {
               
                
               
                if (ModelState.IsValid)
                {
                    
                    if (patientDb.AddPatient(patient))
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Server Error");
                    
                }
                return View(patient);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View(patient);
            }

        }
       
        public ActionResult Edit(int id)
        {

            Patient patient = patientDb.GetPatient(id);
            if (patient != null)
            {
                string x = HttpContext.User.Identity.Name;
                ViewBag.admin = false;
                ViewBag.admin = userDb.GetSingleUserDetails(x).Admin;
                return View(patient);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    if (patientDb.UpdatePatient(patient))
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Server Error");
                }
                return View(patient);


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View(patient);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {

                return View(patientDb.GetPatient(id));
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
        public ActionResult Delete(int id, Patient patient)
        {
            try
            {
                if (appointmentDb.DeleteAppointmenForPatients(id))
                {
                    if (patientDb.DeletePatient(id))
                    {
                        TempData["msg"] = "<script>alert('Patient and there Appointments are Deleted Successfully')</script>";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Unadle to Delete");
                    
                }
                return View(patient);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.StackTrace);
                return View(patient);
            }

        }
        [HttpPost]
        public ActionResult PatientsData()
        {
            try
            {
                var start = Convert.ToInt32(Request.Form["start"]);
                var length = Convert.ToInt32(Request.Form["length"]);
                var searchValue = Convert.ToString(Request.Form["search[value]"]);
                var sortColumnDirection = Request.Form["order[0][dir]"];
                var sortColumn = Request.Form["order[0][column]"];
                var count = patientDb.GetPatientsCount();
                if (length < 0)
                    length = count;
                List<Patient> users = patientDb.GetPatients(start, length, searchValue, sortColumn, sortColumnDirection);
                return Json(new { data = users, recordsTotal = count, recordsFiltered = count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return Json(null);
            }
        }
    }
}