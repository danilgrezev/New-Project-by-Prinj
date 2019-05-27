using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class CarPart
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CarGradeId { get; set; }
        public CarGrade CarGrade { get; set; }

        public ICollection<CarDetail> CarDetails { get; set; }

        [Required]
        public String PartPath { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }
    }
}
