using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableProblem
{
    public class CourseSetting
    {
        public int semester;
        public int day;
        public int koma;
        public int index;

        public void Set(DeterminedCourse course)
        {
            semester = course.semester;
            day = course.day;
            koma = course.koma;
        }

        public int ToId()
        {
            return (semester - 1) * 30 + (day - 1) * 6 + koma - 1;
        }
        public void FromId(int index)
        {
            if (index<0)
            {
                return;
            }
            semester = index / 30 + 1;
            day = index % 30 / 6 + 1;
            koma = index % 6 + 1;
        }


        public static int getkoma(int index)
        {
            return index % 6 + 1;
        }
        public static int getday(int index)
        {
            return index % 30 / 6 + 1;
        }
        public static int getsemester(int index)
        {
            return index / 30 + 1;
        }


        public void Set(CourseSetting setting)
        {
            this.semester = setting.semester;
            this.day = setting.day;
            this.koma = setting.koma;
        }
        public void Exchange(CourseSetting setting)
        {
            int tmpsemester = setting.semester;
            int tmpday = setting.day;
            int tmpkoma = setting.koma;
            setting.semester = this.semester;
            setting.day = this.day;
            setting.koma = this.koma;
            this.semester = tmpsemester;
            this.day = tmpday;
            this.koma = tmpkoma;
        }

        public override bool Equals(object obj)
        {
            if (obj is CourseSetting)
            {
                return ((CourseSetting)obj).ToId() == this.ToId();
            }
            return base.Equals(obj);
        }
        //public int courseid;
    }
}
