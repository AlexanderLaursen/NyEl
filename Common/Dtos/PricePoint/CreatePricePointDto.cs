using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.PricePoint
{
    public class CreatePricePointDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal PricePerKwh { get; set; }
    }
}
