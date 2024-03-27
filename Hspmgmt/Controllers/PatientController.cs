using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hspmgmt.Models;
using Hspmgmt.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hspmgmt.Controllers
{

    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        //constructor
        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients.ToListAsync();

            // Serialize the list of patients to JSON
            var json = JsonSerializer.Serialize(patients);

            // Deserialize the JSON back to a list of patients
            var deserializedPatients = JsonSerializer.Deserialize<List<Patient>>(json);

            return View(patients);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Age,Gender,PhoneNo,Address,DocId")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        /// GET: Patient/Edit/5
        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            // Fetch the list of doctors from the database
            var doctors = await _context.Doctors.ToListAsync();

            // Create a SelectList using the list of doctors
            // Assuming Doctor model has properties Id and Name
            ViewBag.DoctorId = new SelectList(doctors, "Id", "Name", patient.DocId);

            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Age,Gender,PhoneNo,Address,DocId")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            // Fetch the list of doctors from the database
            var doctors = await _context.Doctors.ToListAsync();

            // Create a SelectList using the list of doctors
            // Assuming Doctor model has properties Id and Name
            ViewBag.DoctorId = new SelectList(doctors, "Id", "Name", patient.DocId);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
        // API method to create a new patient
        [AllowAnonymous] // Allow anonymous access to this API
        [HttpPost]
        [Route("api/patient")]
        public async Task<IActionResult> CreatePatient([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);

        }

        // API method to update an existing patient
        [AllowAnonymous] // Allow anonymous access to this API
        [HttpPut]
        [Route("api/patient/{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExistsInDatabase(id)) // Renamed the method to avoid conflicts
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // API method to delete a patient
        [AllowAnonymous] // Allow anonymous access to this API
        [HttpDelete]
        [Route("api/patient/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExistsInDatabase(int id) // Renamed the method to avoid conflicts
        {
            return _context.Patients.Any(e => e.Id == id);
        }

    }
}
