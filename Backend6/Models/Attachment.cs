using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class Attachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CarDetailId { get; set; }
        public CarDetail CarDetail { get; set; }

        [Required]
        public String FilePath { get; set; }
    }
}
