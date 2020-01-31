using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserBooking
{
    public class Property
    {
        public string SPA { get; set; }
        public string ID { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Stars { get; set; }
        public int Reviews { get; set; }
        public double Rating { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
