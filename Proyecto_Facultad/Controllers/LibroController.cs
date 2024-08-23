using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Proyecto_Facultad.Models;

namespace Proyecto_Facultad.Controllers
{
    public class LibroController : Controller
    {
        private readonly BdfflContext _context;

        public LibroController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Libro
        public async Task<IActionResult> Index()
        {
            return View(await _context.Libros.ToListAsync());
        }

        // GET: Libro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
               .FirstOrDefaultAsync(m => m.IdLibro == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libro/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreLibro")] Libro libro)
        {

            try
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
                return View(libro);
            }
        }
       // GET: Leccion/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var libro = await _context.Libros.FindAsync(id);
    if (libro == null)
    {
        return NotFound();
    }
    return View(libro);
}

// POST: Leccion/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("IdLibro,NombreLibro")] Libro libro)
{
    if (id != libro.IdLibro)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(libro);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Datos actualizados correctamente";
        }
        catch (DbUpdateConcurrencyException)
        {
            
                TempData["ErrorMessage"] = "Se produjo un error al actualizar los datos.";
                throw;
            
        }
        return RedirectToAction(nameof(Index));
    }
    return View(libro);
}


    }
}