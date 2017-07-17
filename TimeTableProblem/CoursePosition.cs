using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableProblem
{
    public class CourseSetting
    {
        public int gakki;
        public int day;
        public int koma;
        public int index;

        public void Set(DeterminedCourse course)
        {
            gakki = course.gakki;
            day = course.day;
            koma = course.koma;
        }

        public int ToId()
        {
            return (gakki - 1) * 30 + (day - 1) * 6 + koma - 1;
        }
        public void FromId(int index)
        {
            if (index<0)
            {
                return;
            }
            gakki = index / 30 + 1;
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
        public static int getgakki(int index)
        {
            return index / 30 + 1;
        }


        public void Set(CourseSetting setting)
        {
            this.gakki = setting.gakki;
            this.day = setting.day;
            this.koma = setting.koma;
        }
        public void Exchange(CourseSetting setting)
        {
            int tmpgakki = setting.gakki;
            int tmpday = setting.day;
            int tmpkoma = setting.koma;
            setting.gakki = this.gakki;
            setting.day = this.day;
            setting.koma = this.koma;
            this.gakki = tmpgakki;
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
