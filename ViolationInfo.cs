namespace TimeTableProblem
{
    public class ViolationInfo
    {
        internal int Violation;
        internal int tuunenn;
        internal int renzoku;
        internal int hiruyasumi;
        internal int order;
        internal int compilsory;
        internal int six;
        internal int channel;
        internal int grade;
        internal int days;


        public void set(ViolationInfo s)
        {
                    Violation=s.Violation;
        tuunenn=s.tuunenn;
        renzoku=s.renzoku;
        hiruyasumi=s.hiruyasumi;
        order=s.order;
        compilsory=s.compilsory;
        six=s.six;
        channel=s.channel;
        grade=s.grade;
        days=s.days;
    }
        public void reset()
        {
            Violation = 0;
            tuunenn = 0;
            renzoku = 0;
            hiruyasumi = 0;
            order = 0;
            compilsory = 0;
            six = 0;
            channel = 0;
            grade = 0;
            days = 0;
        }
        public void Count()
        {
            //Violation += 10 * tuunenn;
            Violation += 2 * hiruyasumi;
            Violation += 2 * compilsory;
            Violation += 10 * grade;
            Violation += six;
            Violation += 3 * channel;
            //Violation += 10 * tuunenn;
        }

        #region MyRegion
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

        public int Tuunenn
        {
            get
            {
                return tuunenn;
            }

            set
            {
                tuunenn = value;
            }
        }

        public int Renzoku
        {
            get
            {
                return renzoku;
            }

            set
            {
                renzoku = value;
            }
        }

        public int Hiruyasumi
        {
            get
            {
                return hiruyasumi;
            }

            set
            {
                hiruyasumi = value;
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

        public int Six
        {
            get
            {
                return six;
            }

            set
            {
                six = value;
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