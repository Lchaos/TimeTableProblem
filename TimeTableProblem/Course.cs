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
        // public int profession{get;set;}
        // public int continue_semester{get;set;}
        // public int lecture{get;set;}
        // public int experiment{get;set;}
        // public int computer_room{get;set;}
        // public string teacher_code{get;set;}
        public int semester { get; set; }
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
        public int profession { get; set; }
        public int continue_semester { get; set; }
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
                return grade + profession - 2;
            }
        }
        public bool IsSameCourse(Course setting)
        {
            return setting.name == this.name && setting.grade == this.grade && setting.profession == this.profession;
        }

        public void From(Course c)
        {
            this.name = c.name;
            this.group = c.group;
            this.code = c.code;
            this.grade = c.grade;
            this.length = c.length;
            this.compilsory = c.compilsory;
            this.profession = c.profession;
            this.continue_semester = c.continue_semester;
            this.lecture = c.lecture;
            this.experiment = c.experiment;
            this.computer_room = c.computer_room;
            this.teacher_code = c.teacher_code;

        }
    }
}