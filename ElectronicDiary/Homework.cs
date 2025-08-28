using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary
{
    public class Homework
    {
        public string? LessDate { get; set; }
        public string? LessTheme { get; set; }
        public string? Teacher { get; set; }
        public string? Task { get; set; }
        public int? Mark { get; set; }
        public string? Comment { get; set; }
        public Homework(string? lessDate, string? lessTheme,
            string? teacher, string? task,
            int? mark, string? comment)
        {
            LessDate = lessDate;
            LessTheme = lessTheme;
            Teacher = teacher;
            Task = task;
            Mark = mark;
            Comment = comment;
        }
    }
}