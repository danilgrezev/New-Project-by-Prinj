using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class CarDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CarPartId { get; set; }
        public CarPart CarPart { get; set; }

        public Guid BasketId { get; set; }

        public Basket Basket { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        [Required]
        public String DetailPath { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        [Required]
        public Int32 Cost { get; set; }

    }
}
