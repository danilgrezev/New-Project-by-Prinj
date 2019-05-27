using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class CarGrade
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CarModelId { get; set; }
        public CarModel CarModel { get; set; }

        public ICollection<CarPart> CarParts { get; set; }

        [Required]
        public String GradePath { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }
    }
}
