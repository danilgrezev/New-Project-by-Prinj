using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class CarModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CarBrandId { get; set; }
        public CarBrand CarBrand { get; set; }

        public ICollection<CarGrade> CarGrades { get; set; }

        [Required]
        public String ModelPath { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }
    }
}
