﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LibraryModel.Models;

namespace M_Sinca_Teodora_Ioana_Lab2.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public DateTime? BirthDate { get; set; }
        public ICollection<Order>? Orders { get; set; }

        [ForeignKey("CityID")]
        public int? CityID { get; set; }

       public City? City { get; set; }
    }
}
