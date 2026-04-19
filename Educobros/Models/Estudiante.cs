using System.ComponentModel.DataAnnotations;

namespace EduCobros.Models
{
    public class Estudiante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Entre 3 y 100 caracteres")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El grado es obligatorio")]
        public string Grado { get; set; } = "";

        [Required]
        [Range(0, 50000, ErrorMessage = "Entre RD$0 y RD$50,000")]
        [Display(Name = "Mensualidad (RD$)")]
        public decimal Mensualidad { get; set; }

        [Range(0, 12)]
        [Display(Name = "Meses Debidos")]
        public int MesesDebidos { get; set; }

        // Propiedad calculada (no se guarda en BD)
        public string Estado => MesesDebidos == 0 ? "✅ Al día"
            : MesesDebidos <= 2 ? $"⚠️ Debe {MesesDebidos} meses"
            : $"❌ Debe {MesesDebidos} meses";

        // Agregar al final de la clase Estudiante, antes del cierre }

        // Relación: Un estudiante tiene MUCHOS pagos
        public List<Pago> Pagos { get; set; } = new List<Pago>();
    }
}