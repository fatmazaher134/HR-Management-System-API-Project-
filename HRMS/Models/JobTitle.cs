using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models;

    public class JobTitle
    {
        public int JobTitleID { get; set; }

        [Required, MaxLength(150)]
        public required string TitleName { get; set; }

        // Relations
        public ICollection<Employee>? Employees { get; set; } = new List<Employee>();
    }

