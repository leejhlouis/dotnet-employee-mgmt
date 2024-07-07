using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotnetEmployeeMgmt.Data;
using SMKaryawan.Models;
using Microsoft.AspNetCore.Authorization;

namespace DotnetEmployeeMgmt.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        [Authorize]
        public async Task<IActionResult> Index(string nama, string departemen, string jenisKelamin, DateTime? tanggalLahir, string jabatan)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
            }

            var filteredEmployees = _context.Employee.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nama))
            {
                filteredEmployees = filteredEmployees.Where(e => e.Nama.ToLower().Contains(nama.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(departemen))
            {
                filteredEmployees = filteredEmployees.Where(e => e.Departemen.Equals(departemen));
            }
            if (!string.IsNullOrWhiteSpace(jenisKelamin))
            {
                filteredEmployees = filteredEmployees.Where(e => e.JenisKelamin.Equals(jenisKelamin));
            }
            if (tanggalLahir.HasValue)
            {
                tanggalLahir = tanggalLahir.Value.ToUniversalTime();
                filteredEmployees = filteredEmployees.Where(e => e.TanggalLahir.HasValue && e.TanggalLahir.Value.Date == tanggalLahir.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(jabatan))
            {
                filteredEmployees = filteredEmployees.Where(e => e.Jabatan.Equals(jabatan));
            }

            return View(await filteredEmployees.ToListAsync());
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NIK,Nama,Alamat,TanggalLahir,JenisKelamin,Departemen,Jabatan")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.TanggalLahir = employee.TanggalLahir?.ToUniversalTime();
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NIK,Nama,Alamat,TanggalLahir,JenisKelamin,Departemen,Jabatan")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employee.TanggalLahir = employee.TanggalLahir?.ToUniversalTime();
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employee?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
