using EduCobros.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduCobros.Data;

namespace EduCobros.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly EduCobrosContext _context;

        public PagosController(EduCobrosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var pagos = _context.Pagos
                .Include(p => p.Estudiante)
                .ToList();

            return View(pagos);
        }

        [Authorize(Roles = "Admin,Secretaria")]
        public IActionResult Create()
        {
            ViewBag.EstudianteId = new SelectList(_context.Estudiantes, "Id", "Nombre");
            return View(new Pago { Fecha = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Secretaria")]
        public async Task<IActionResult> Create(Pago pago)
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

                ViewBag.EstudianteId = new SelectList(_context.Estudiantes, "Id", "Nombre", pago.EstudianteId);
                return View(pago);
            }

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}