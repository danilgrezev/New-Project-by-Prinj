using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class Basket
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public ICollection<CarDetail> CarDetails { get; set; }

        [Required]
        public String CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        [Required]
        public Int32 FullPrice { get; set; }

    }
}
