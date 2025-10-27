using System.ComponentModel.DataAnnotations;

namespace ProcessorDB.Models
{
    public class Processor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Model { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int ReleaseYear { get; set; }

        public TechSpec TechSpec { get; set; }
        public ProductionInfo ProductionInfo { get; set; }
    }
}
