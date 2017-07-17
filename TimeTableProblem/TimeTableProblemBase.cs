
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TimeTableProblem
{
    public class TimeTableProblemBase
    {
        protected CourseCollection infos;
        protected Random g_r;
        public const int nSize=90;
        public long evaltimes;
        public int minConf;
        protected int subnum;
        protected int decnum;
        public TimeTableProblemBase()
        {
            infos = new CourseCollection(ReadData.readSubject(),ReadData.readCourse());
            g_r = CourseInfoCollection.g_r;
            subnum = infos.Subjects.Count;
            decnum = infos.DeterminedCourses.Count;
        }

        public virtual void DoJob()
        {
        }

        public virtual void init()
        {

        }
    }
}