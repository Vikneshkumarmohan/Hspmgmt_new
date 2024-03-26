using Hspmgmt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;

namespace Hspmgmt.Models
{
    
        public class Patient
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public char Gender { get; set; }
            public long PhoneNo { get; set; }
            public string Address { get; set; }
            public int DocId { get; set; }
            public char IsDeleted { get; set; }
        }

    }

