using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FASTFOOD.Models
{
   

    public class Service
    {
        public long Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }

}
