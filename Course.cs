namespace TimeTableProblem
{
    public class DeterminedCourse : Course
    {
        // public string name{get;set;}
        // public string group{get;set;}
        // public string code{get;set;}
        // public int grade{get;set;}
        // public int length{get;set;}
        // public int compilsory{get;set;}
        // public int senkou{get;set;}
        // public int continue_gakki{get;set;}
        // public int lecture{get;set;}
        // public int experiment{get;set;}
        // public int computer_room{get;set;}
        // public string teacher_code{get;set;}
        public int gakki { get; set; }
        public int day { get; set; }
        public int koma { get; set; }
    }
    public class Course
    {
        public string name { get; set; }
        public string group { get; set; }
        public string code { get; set; }
        public int grade { get; set; }
        public int length { get; set; }
        public int compilsory { get; set; }
        public int senkou { get; set; }
        public int continue_gakki { get; set; }
        public int lecture { get; set; }
        public int experiment { get; set; }
        public int computer_room { get; set; }
        public string teacher_code { get; set; }
        internal int index;
        internal int m_PlaceID = -1;
        internal int nextID = -1;
        internal int PlaceID
        {
            get
            {
                if (m_PlaceID == -1)
                {
                    m_PlaceID = toPlaceID();
                }
                return m_PlaceID;
            }
        }
        public int toPlaceID()
        {

            if (grade < 3)
            {
                return grade - 1;
            }
            else
            {
                return grade + senkou - 2;
            }
        }
        public bool IsSameCourse(Course setting)
        {
            return setting.name == this.name && setting.grade == this.grade && setting.senkou == this.senkou;
        }

        public void From(Course c)
        {
            this.name = c.name;
            this.group = c.group;
            this.code = c.code;
            this.grade = c.grade;
            this.length = c.length;
            this.compilsory = c.compilsory;
            this.senkou = c.senkou;
            this.continue_gakki = c.continue_gakki;
            this.lecture = c.lecture;
            this.experiment = c.experiment;
            this.computer_room = c.computer_room;
            this.teacher_code = c.teacher_code;

        }
    }
}