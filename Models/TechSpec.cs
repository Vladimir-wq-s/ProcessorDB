using System.ComponentModel.DataAnnotations;

namespace ProcessorDB.Models
{
    public class TechSpec
    {
        public int Id { get; set; }
        public int ProcessorId { get; set; }

        [Required]
        public string TechProcess { get; set; }

        public double Frequency { get; set; }

        [Required]
        public string CacheL3 { get; set; }

        public int Cores { get; set; }

        [Required]
        public string Slot { get; set; }
    }
}
