using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using Proyecto_Facultad.ViewModels;

namespace Proyecto_Facultad.Controllers
{

    [Authorize(Roles = "Coordinador, Admin")]
    public class AlumnoController : Controller
    {
        private readonly BdfflContext _context;

        public AlumnoController(BdfflContext context)
        {
            _context = context;
        }
        // GET: Alumno
        public async Task<IActionResult> Index()
        {
            return View(await _context.Alumnos());
        }

        // GET: Alumno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(m => m.IdAlumno == id);
            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // GET: Alumno/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAlumno,PrimerNombreAlumno,SegundoNombreAlumno,OtrosNombresAlumno,PrimerApellidoAlumno,SegundoApellidoAlumno,ApellidoCasado,FechaNacimientoAlumno,FechaBautizoAlumno,Direccion,Telefono,Dpi,Nit,EstadoCivil,Genero,NumeroCelula,CodigoFrater,Becado,Retiro,ReunionesEnConfianza,Otros,EstatusAlumno")] Alumno alumno)
        {
            //Validacion de que no existe un codigo frater
            var existingAlumno = await _context.Alumnos.FirstOrDefaultAsync(a =>
            a.CodigoFrater == alumno.CodigoFrater);

            if (existingAlumno != null)
            {
                // Agregar un error de validación si el código está duplicado
                ModelState.AddModelError("CodigoFrater", "El Código Frater ya está en uso.");
            }

            if (ModelState.IsValid)
            {
                alumno.EstatusAlumno = true;
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        // GET: Alumno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }
            return View(alumno);
        }

        // POST: Alumno/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAlumno,PrimerNombreAlumno,SegundoNombreAlumno,OtrosNombresAlumno,PrimerApellidoAlumno,SegundoApellidoAlumno,ApellidoCasado,FechaNacimientoAlumno,FechaBautizoAlumno,Direccion,Telefono,Dpi,Nit,EstadoCivil,Genero,NumeroCelula,CodigoFrater,Becado,Retiro,ReunionesEnConfianza,Otros,EstatusAlumno")] Alumno alumno)
        {
            if (id != alumno.IdAlumno)
            {
                return NotFound();
            }

            var existingAlumno = await _context.Alumnos
            .FirstOrDefaultAsync(a => a.CodigoFrater == alumno.CodigoFrater && a.IdAlumno != alumno.IdAlumno); //valida que el id no exista, al mismo tiempo que el cod.frater no se repita en 2 usuarios sino solo 1

            if (existingAlumno != null)
            {
                // Agregar un error de validación si el código está duplicado
                ModelState.AddModelError("CodigoFrater", "El Código Frater ya está en uso.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnoExists(alumno.IdAlumno))
                    {
                        TempData["ErrorMessage"] = "El alumno no se encontró.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Se produjo un error al cambiar el estatus.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        // GET: Alumno/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var alumno = await _context.Alumnos
        //        .FirstOrDefaultAsync(m => m.IdAlumno == id);
        //    if (alumno == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(alumno);
        //}

        [HttpPost]
        public async Task<IActionResult> Deactivate(int? id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                TempData["ErrorMessage"] = "El alumno no se encontró.";
                return NotFound();
            }

            // Cambiar el estatus
            alumno.EstatusAlumno = !alumno.EstatusAlumno;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = alumno.EstatusAlumno ? "Alumno activado correctamente" : "Alumno desactivado correctamente";

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(alumno.IdAlumno))
                {
                    TempData["ErrorMessage"] = "Error al actualizar el estatus del alumno.";
                    return NotFound();

                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
            //return View(alumno);

        }


        //// POST: Alumno/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var alumno = await _context.Alumnos.FindAsync(id);
        //    if (alumno != null)
        //    {
        //        _context.Alumnos.Remove(alumno);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool AlumnoExists(int id)
        {
            return _context.Alumnos.Any(e => e.IdAlumno == id);
        }

        public async Task<IActionResult> ReporteAlumnosInscritos()
        {
            return View(await _context.Alumnos.Where(a => a.EstatusAlumno).
                ToListAsync());
        }

        public IActionResult ReporteGraduados()
        {
            // Obtener alumnos graduados en mesas de Nivel 1
            var alumnosGraduadosNivel1 = _context.Notas
                .Where(n => n.IdBimestre == 4 && n.NotaObtenida > 80) // Condiciones de bimestre y nota
                .Select(nota => new
                {
                    Asignacion = nota.IdAsignacionalumnosNavigation, // Acceder a AsignacionAlumno
                    Alumno = nota.IdAsignacionalumnosNavigation.IdAlumnoNavigation, // Acceder al Alumno a través de AsignacionAlumno
                    Mesa = nota.IdAsignacionalumnosNavigation.IdMesaNavigation, // Acceder a Mesa a través de AsignacionAlumno
                    NotaObtenida = nota.NotaObtenida, // Obtener la nota
                    AvanceMesa = nota.IdAsignacionalumnosNavigation.IdMesaNavigation.AvanceMesas
                        .Where(am => am.IdMesa == nota.IdAsignacionalumnosNavigation.IdMesa && am.IdNivel == 1) // Condición de Nivel 1
                        .FirstOrDefault() // Obtener el primer avance de mesa de Nivel 1
                })
                .Where(x => x.AvanceMesa != null) // Asegurarse de que haya un AvanceMesa en Nivel 1
                .Select(x => new AlumnoReporteViewModel
                {
                    NombreCompleto = $"{x.Alumno.PrimerNombreAlumno} {x.Alumno.SegundoNombreAlumno} {x.Alumno.PrimerApellidoAlumno} {x.Alumno.SegundoApellidoAlumno}",
                    Telefono = x.Alumno.Telefono,
                    NotaObtenida = x.NotaObtenida,
                    Nivel = x.AvanceMesa.IdNivelNavigation.NombreNivel // Acceder al nombre del nivel desde la navegación
                })
                .ToList();

            // Pasar los datos a la vista
            var viewModel = new ReporteAlumnosGraduadosViewModel
            {
                AlumnosGraduados = alumnosGraduadosNivel1
            };

            return View(viewModel);
        }

        public IActionResult ReporteGraduadosNivel2()
        {
            var alumnosGraduados = _context.Notas
                .Where(n => n.IdBimestre == 4 && n.NotaObtenida > 80) // Condición de bimestre y nota
                .Select(nota => new
                {
                    Asignacion = nota.IdAsignacionalumnosNavigation,
                    Alumno = nota.IdAsignacionalumnosNavigation.IdAlumnoNavigation,
                    Mesa = nota.IdAsignacionalumnosNavigation.IdMesaNavigation,
                    NotaObtenida = nota.NotaObtenida,
                    AvanceMesa = nota.IdAsignacionalumnosNavigation.IdMesaNavigation.AvanceMesas
                        .FirstOrDefault(am => am.IdNivel == 2) // Solo alumnos del Nivel 2
                })
                .Where(x => x.AvanceMesa != null) // Asegurarse de que el AvanceMesa existe
                .Select(x => new AlumnoReporteViewModel
                {
                    NombreCompleto = $"{x.Alumno.PrimerNombreAlumno} {x.Alumno.SegundoNombreAlumno} {x.Alumno.PrimerApellidoAlumno} {x.Alumno.SegundoApellidoAlumno}",
                    Telefono = x.Alumno.Telefono,
                    NotaObtenida = x.NotaObtenida,
                    Nivel = x.AvanceMesa.IdNivelNavigation.NombreNivel // Mostrar el nivel real (que será 2)
                })
                .ToList();

            var viewModel = new ReporteAlumnosGraduadosViewModel
            {
                AlumnosGraduados = alumnosGraduados
            };

            return View("ReporteGraduadosNivel2", viewModel); // Especifica la vista aparte
        }

        // Acción para mostrar la vista BautizadosPorMes
        public IActionResult BautizadosPorMes(string nombre, DateOnly? fechaInicio, DateOnly? fechaFin)
        {
            // Inicializar la consulta base
            var query = _context.Alumnos.Where(a => a.FechaBautizoAlumno.HasValue);

            // Aplicar filtro por nombre si existe (evaluado en el lado del cliente)
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.AsEnumerable().Where(a => (a.PrimerNombreAlumno + " " + a.PrimerApellidoAlumno)
                                                         .Contains(nombre, StringComparison.OrdinalIgnoreCase))
                                            .AsQueryable();
            }

            // Aplicar filtro por fecha de inicio si se ha proporcionado
            if (fechaInicio.HasValue)
            {
                query = query.Where(a => a.FechaBautizoAlumno >= fechaInicio.Value);
            }

            // Aplicar filtro por fecha de fin si se ha proporcionado
            if (fechaFin.HasValue)
            {
                query = query.Where(a => a.FechaBautizoAlumno <= fechaFin.Value);
            }

            // Obtener la lista final filtrada
            var alumnosFiltrados = query.ToList();

            // Enviar los resultados de la búsqueda al ViewBag
            ViewBag.ResultadosBusqueda = alumnosFiltrados;

            // Enviar el modelo a la vista
            var alumnosBautizadosPorMes = alumnosFiltrados
                .GroupBy(a => a.FechaBautizoAlumno.Value.Month)
                .Select(g => new AlumnoBautizadoPorMesViewModel
                {
                    Mes = g.Key,
                    Cantidad = g.Count()
                }).ToList();

            return View(alumnosBautizadosPorMes);
        }



    }
}
