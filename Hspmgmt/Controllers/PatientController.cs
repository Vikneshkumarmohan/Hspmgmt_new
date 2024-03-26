using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hspmgmt.Models;
using Hspmgmt.Data;


namespace Hspmgmt.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        //private readonly ILogger<PatientController> _logger;
        private readonly ApplicationDbContext _context;
        //public PatientController(ILogger<PatientController> logger)
        public PatientController(ApplicationDbContext context)
        {
            // _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<Patient> patients = _context.Patients.ToList();
            // Serialize patients to JSON
            string json = JsonConvert.SerializeObject(patients);

            // Deserialize JSON back to list of patients
            List<Patient> deserializedPatients = JsonConvert.DeserializeObject<List<Patient>>(json);

            return View(patients);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult HomePage()
        {
            return Content("This is homepage");
        }
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [Authorize(Policy = "RequireAdminRole")] // Requires "Admin" role for this action
        public IActionResult AdminAction()
        {
            // Action for administrators
            return View();
        }
    }
}
