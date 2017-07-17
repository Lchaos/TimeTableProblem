namespace TimeTableProblem
{
    public class ViolationInfo
    {
        internal int Violation;
        internal int continueYear;
        internal int continued;
        internal int crossLanchTime;
        internal int order;
        internal int compilsory;
        internal int sixthCourse;
        internal int channel;
        internal int grade;
        internal int days;


        public void set(ViolationInfo s)
        {
                    Violation=s.Violation;
        continueYear=s.continueYear;
        continued=s.continued;
        crossLanchTime=s.crossLanchTime;
        order=s.order;
        compilsory=s.compilsory;
        sixthCourse=s.sixthCourse;
        channel=s.channel;
        grade=s.grade;
        days=s.days;
    }
        public void reset()
        {
            Violation = 0;
            continueYear = 0;
            continued = 0;
            crossLanchTime = 0;
            order = 0;
            compilsory = 0;
            sixthCourse = 0;
            channel = 0;
            grade = 0;
            days = 0;
        }
        public void Count()
        {
            //Violation += 10 * continueYear;
            Violation += 2 * crossLanchTime;
            Violation += 2 * compilsory;
            Violation += 10 * grade;
            Violation += sixthCourse;
            Violation += 3 * channel;
            //Violation += 10 * continueYear;
        }

        #region for serilized
        public int violation
        {
            get
            {
                return Violation;
            }

            set
            {
                Violation = value;
            }
        }

        public int ContinueYear
        {
            get
            {
                return continueYear;
            }

            set
            {
                continueYear = value;
            }
        }

        public int Continued
        {
            get
            {
                return continued;
            }

            set
            {
                continued = value;
            }
        }

        public int CrossLanchTime
        {
            get
            {
                return crossLanchTime;
            }

            set
            {
                crossLanchTime = value;
            }
        }

        public int Order
        {
            get
            {
                return order;
            }

            set
            {
                order = value;
            }
        }

        public int Compilsory
        {
            get
            {
                return compilsory;
            }

            set
            {
                compilsory = value;
            }
        }

        public int SixthCourse
        {
            get
            {
                return sixthCourse;
            }

            set
            {
                sixthCourse = value;
            }
        }

        public int Channel
        {
            get
            {
                return channel;
            }

            set
            {
                channel = value;
            }
        }

        public int Grade
        {
            get
            {
                return grade;
            }

            set
            {
                grade = value;
            }
        }

        public int Days
        {
            get
            {
                return days;
            }

            set
            {
                days = value;
            }
        }
        #endregion

    }
}