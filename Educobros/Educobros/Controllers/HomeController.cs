using Educobros.Models;
using EduCobros.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace Educobros.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EduCobrosContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, EduCobrosContext context, IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            if (!_cache.TryGetValue("dashboard_stats", out DashboardVM? stats))
            {
                stats = new DashboardVM
                {
                    TotalEstudiantes = await _context.Estudiantes.CountAsync(),
                    Pendientes = await _context.Estudiantes.CountAsync(e => e.MesesDebidos > 0),
                    TotalRecaudado = await _context.Pagos.AnyAsync()
                        ? await _context.Pagos.SumAsync(p => p.Monto)
                        : 0,
                    Mora = await _context.Estudiantes
                        .Where(e => e.MesesDebidos > 0)
                        .SumAsync(e => e.Mensualidad * e.MesesDebidos),
                    UltimosPagos = await _context.Pagos
                        .Include(p => p.Estudiante)
                        .OrderByDescending(p => p.Fecha)
                        .Take(5)
                        .ToListAsync()
                };

                _cache.Set("dashboard_stats", stats, TimeSpan.FromMinutes(5));
            }

            return View(stats);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}