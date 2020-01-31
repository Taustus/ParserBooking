using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserBooking
{
    public class Room
    {
        public List<Int64> Price { get; set; }
        public string Roomtype { get; set; }
        public List<int> Capacity { get; set; }
        public List<string> Refund { get; set; }
        public List<string> Meal { get; set; }
    }
}
