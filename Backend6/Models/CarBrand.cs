using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class CarBrand
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public ICollection<CarModel> CarModels { get; set; }

        [Required]
        public String PathBrand { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }


    }
}