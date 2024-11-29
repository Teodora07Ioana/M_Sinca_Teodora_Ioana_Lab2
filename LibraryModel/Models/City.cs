using M_Sinca_Teodora_Ioana_Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel.Models
{
    public class City
    {
        public int ID { get; set; } 
        public string CityName { get; set; } 

        public ICollection<Customer> Customer { get; set; }
    }
}
