using System;
using System.Collections.Generic;
using System.Linq;

namespace UnosquareAspNetCore.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        /// <summary>The full name of the employee.</summary>
        public string Name { get; set; }

        /// <summary>The job title of the employee.</summary>
        public string Title { get; set; }
    }
}
