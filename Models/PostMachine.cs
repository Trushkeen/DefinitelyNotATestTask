using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DefinetelyNotATestTask.Models
{
    public class PostMachine
    {
        //in test exercise document said that this field should be string, but referenced field to this in Order model is int,
        //so I changed type
        public int Id { get; set; }
        public string Address { get; set; }
        public bool IsWorking { get; set; }
    }
}
