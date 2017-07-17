using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace TimeTableProblem
{
    public static class ReadData
    {
        public static List<DeterminedCourse>  readCourse(string filename  = "kotei.txt")
        {
            string str = System.IO.File.ReadAllText(filename, Encoding.UTF8);
            var pocos = ServiceStack.Text.CsvSerializer.DeserializeFromString<List<DeterminedCourse>>(str) ;
            return pocos;
        }
        public static List<Course>  readSubject(string filename = "data10.txt")
        {
            string str = System.IO.File.ReadAllText(filename, Encoding.UTF8);
            var pocos = ServiceStack.Text.CsvSerializer.DeserializeFromString<List<Course>>(str) ;
            return pocos;
        }

    }
}
