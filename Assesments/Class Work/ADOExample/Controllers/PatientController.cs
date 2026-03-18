using ADOExample.DAL;
using ADOExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADOExample.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientCRUD? _patientCRUD;

        public PatientController(PatientCRUD patientCRUD)
        {
            _patientCRUD = patientCRUD;
        }

        public IActionResult Index()
        {
            List<Patient> patients = _patientCRUD.GetPatients();
            return View(patients);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Patient p)
        {
            _patientCRUD.AddPatient(p);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Patient p = _patientCRUD.GetPatientById(Id);
            return View(p);
        }

        [HttpPost]
        public IActionResult Edit(Patient p)
        {
            _patientCRUD.UpdatePatient(p);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int Id) 
        { 
            Patient p = _patientCRUD.GetPatientById(Id);
            return View(p);

        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePatientConfirmed(int Id)
        {
            _patientCRUD.DeletePatient(Id);
            return RedirectToAction("Index");
        }

    }
}
