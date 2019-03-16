using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ixf_V2.Models.PaperModel
{

    public class Paper
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        public Paper() { }

        public Paper(string name, string location)
        {
            Name = name;
            Location = location;
        }

        public Paper(string name, string location, string date, string time)
        {
            Name = name;
            Location = location;
            Date = date;
            Time = time;
        }
    }
}