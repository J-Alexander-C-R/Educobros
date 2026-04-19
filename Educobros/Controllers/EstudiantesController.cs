using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EduCobros.Data;
using EduCobros.Models;

namespace EduCobros.Controllers
{
    [Authorize] // Todo el controlador requiere login
    public class EstudiantesController : Controller
    {
        private readonly EduCobrosContext _context;

        // DI: ASP.NET inyecta el contexto automáticamente
        public EstudiantesController(EduCobrosContext context)
        {
            _context = context;
        }

        // GET: /Estudiantes
        // Todos los logueados pueden ver la lista
        public async Task<IActionResult> Index()
        {
            var estudiantes = await _context.Estudiantes.ToListAsync();
            return View(estudiantes);
        }

        // GET: /Estudiantes/Create
        // Solo Admin y Secretaria pueden entrar al formulario
        [Authorize(Roles = "Admin,Secretaria")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Estudiantes/Create
        // Solo Admin y Secretaria pueden guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Secretaria")]
        public async Task<IActionResult> Create(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"Campo: {item.Key} - Error: {error.ErrorMessage}");
                    }
                }

                return View(estudiante);
            }

            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Estudiantes/Edit/5
        // Solo Admin y Secretaria pueden editar
        [Authorize(Roles = "Admin,Secretaria")]
        public async Task<IActionResult> Edit(int id)
        {
            var est = await _context.Estudiantes.FindAsync(id);
            if (est == null)
                return NotFound();

            return View(est);
        }

        // POST: /Estudiantes/Edit/5
        // Solo Admin y Secretaria pueden guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Secretaria")]
        public async Task<IActionResult> Edit(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Estudiantes.Update(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(estudiante);
        }

        // POST: /Estudiantes/Delete/5
        // Solo Admin puede eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var est = await _context.Estudiantes.FindAsync(id);
            if (est == null)
                return NotFound();

            _context.Estudiantes.Remove(est);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}