﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Facultad.Models;
using System.Security.Claims;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Maestro, Auxiliar, Admin")]
    public class AsistenciaStaffsController : Controller
    {
        private readonly BdfflContext _context;

        public AsistenciaStaffsController(BdfflContext context)
        {
            _context = context;
        }

        // GET: AsistenciaStaffs
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.AsistenciaStaffs.Include(a => a.IdBimestreNavigation).Include(a => a.IdLeccionNavigation).Include(a => a.IdMesaNavigation).Include(a => a.IdStaffNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: AsistenciaStaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaStaff = await _context.AsistenciaStaffs
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdAsistenciaStaff == id);
            if (asistenciaStaff == null)
            {
                return NotFound();
            }

            return View(asistenciaStaff);
        }

        // GET: AsistenciaStaffs/Create
        public IActionResult Create()
        {
            // Aquí obtienes el nombre del usuario autenticado.
            var userName = User.Identity.Name;

            // Verificar si obtenemos el nombre de usuario autenticado.
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar el usuario basado en su nombre.
            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);

            // Verificar si el usuario existe en la tabla Usuarios.
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Ahora, usa el IdUsuario para buscar el staff (maestro).
            var staff = _context.Staff.FirstOrDefault(s => s.IdUsuario == usuario.IdUsuario);

            // Verificar si el staff existe en la tabla Staff.
            if (staff == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Si todo va bien, continuamos con el código normal.
            var mesasAsignadas = _context.AsignacionMaestros
                .Where(am => am.IdStaff == staff.IdStaff)
                .Select(am => new
                {
                    am.IdMesa,
                    MesaDescripcion = $"{am.IdMesaNavigation.IdMesa} - {am.IdMesaNavigation.NombreSedeNavigation.NombreSede} - {am.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {am.IdMesaNavigation.IdJornadaNavigation.Horario}"
                })
                .ToList();

            // Enviar el nombre del maestro a la vista.
            ViewData["NombreMaestro"] = $"{staff.PrimerNombreStaff} {staff.PrimerApellidoStaff}";

            // Crear un SelectList con las mesas asignadas.
            ViewData["IdMesa"] = new SelectList(mesasAsignadas, "IdMesa", "MesaDescripcion");

            // Otros datos necesarios para el formulario.
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion");
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");

            return View();
        }


        // POST: AsistenciaStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsistenciaStaff,IdStaff,FechaClase,IdMesa,IdLeccion,IdBimestre,Ausencia")] AsistenciaStaff asistenciaStaff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asistenciaStaff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Asistencia Maestro creada con exito.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", asistenciaStaff.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", asistenciaStaff.IdLeccion);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asistenciaStaff.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "PrimerNombreStaff", asistenciaStaff.IdStaff);
            return View(asistenciaStaff);
        }

        // GET: AsistenciaStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaStaff = await _context.AsistenciaStaffs.FindAsync(id);
            if (asistenciaStaff == null)
            {
                return NotFound();
            }
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", asistenciaStaff.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", asistenciaStaff.IdLeccion);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asistenciaStaff.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "PrimerNombreStaff", asistenciaStaff.IdStaff);
            return View(asistenciaStaff);
        }

        // POST: AsistenciaStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsistenciaStaff,IdStaff,FechaClase,IdMesa,IdLeccion,IdBimestre,Ausencia")] AsistenciaStaff asistenciaStaff)
        {
            if (id != asistenciaStaff.IdAsistenciaStaff)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistenciaStaff);
                    await _context.SaveChangesAsync();
                    TempData["ErrorMessage"] = "Datos actualizados correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaStaffExists(asistenciaStaff.IdAsistenciaStaff))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Se produjo un error al actualizar datos.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", asistenciaStaff.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", asistenciaStaff.IdLeccion);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asistenciaStaff.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "PrimerNombreStaff", asistenciaStaff.IdStaff);
            return View(asistenciaStaff);
        }

        // GET: AsistenciaStaffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaStaff = await _context.AsistenciaStaffs
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdAsistenciaStaff == id);
            if (asistenciaStaff == null)
            {
                return NotFound();
            }

            return View(asistenciaStaff);
        }

        // POST: AsistenciaStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asistenciaStaff = await _context.AsistenciaStaffs.FindAsync(id);
            if (asistenciaStaff != null)
            {
                _context.AsistenciaStaffs.Remove(asistenciaStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsistenciaStaffExists(int id)
        {
            return _context.AsistenciaStaffs.Any(e => e.IdAsistenciaStaff == id);
        }
    }
}
