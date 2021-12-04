using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RR_hookah.Models
{
    public class ApplicationType
    {
        // объяснение в Category.cs
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
