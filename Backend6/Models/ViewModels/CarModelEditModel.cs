using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Backend6.Models.ViewModels
{
    public class CarModelEditModel
    {       
        public IFormFile ModelPath { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

    }
}
