using System.ComponentModel.DataAnnotations;

namespace EduCobros.Models
{
    public class Pago
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecciona un estudiante")]
        [Display(Name = "Estudiante")]
        public int EstudianteId { get; set; }

        [Required]
        public string Concepto { get; set; } = "";

        [Required]
        [Range(1, 100000, ErrorMessage = "El monto debe ser mayor a 0")]
        [Display(Name = "Monto (RD$)")]
        public decimal Monto { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Método de Pago")]
        public string Metodo { get; set; } = "";

        public string? Notas { get; set; }

        // Agregar al final de la clase Pago, antes del cierre }

        // Relación: Cada pago pertenece a UN estudiante
        public Estudiante? Estudiante { get; set; }
    }
}