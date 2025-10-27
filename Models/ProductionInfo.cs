using System;

namespace ProcessorDB.Models
{
    public class ProductionInfo
    {
        public int Id { get; set; }
        public int ProcessorId { get; set; }

        public DateTime ProductionDate { get; set; }
        public int WarrantyPeriod { get; set; }
        public decimal Price { get; set; }
        public int Points { get; set; }
        public bool Promotion { get; set; }
    }
}
