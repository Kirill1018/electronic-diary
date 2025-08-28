using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary
{
    internal class Subject
    {
        public string? Name { get; set; }
        public string? Date { get; set; }
        public string? Theme { get; set; }
        public string? Teacher { get; set; }
        public int Mark { get; set; }
        public Subject(string? name, string? date,
            string? theme, string? teacher,
            int mark)
        {
            Name = name;
            Date = date;
            Theme = theme;
            Teacher = teacher;
            Mark = mark;
        }
    }
}