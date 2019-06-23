using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Backend6.Models.ViewModels
{
    public class CarDetailEditModel
    {
        public IFormFile DetailPath { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Int32 Cost { get; set; }
    }
}
