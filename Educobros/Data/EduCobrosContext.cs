using EduCobros.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduCobros.Data;

// Cambiamos DbContext por IdentityDbContext
public class EduCobrosContext : IdentityDbContext
{
    public EduCobrosContext(DbContextOptions<EduCobrosContext> options)
        : base(options) { }

    public DbSet<Estudiante> Estudiantes { get; set; }
    public DbSet<Pago> Pagos { get; set; }
}