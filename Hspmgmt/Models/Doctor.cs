using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hspmgmt.Models
{
    public class Doctor
    {
        [Key]
        public int DocId { get; set; }
        public string FirstName { get; set; }
    }
}