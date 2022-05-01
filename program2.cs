using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace tamrin5_2
{
    enum Gender { male, female, other }
    class Person
    {
        string _name;
        string _SSN;
        string _field;
        Gender gender;
        static List<Person> people = new List<Person>();
        public string name
        {
            get { return _name; }
            protected set
            {
                Regex pattern = new Regex(@".{3,20}");
                if (!pattern.IsMatch(value)) throw new Exception("name is not in correct format");
                _name = value;
            }
        }
        public string SSN
        {
            get { return _SSN; }
            protected set
            {
                Regex pattern = new Regex(@"\d{10}");
                if (!pattern.IsMatch(value) && SSNIsRegistered(SSN)) throw new Exception("SSN is not in correct format");
                _SSN = value;
            }
        }
        public string field
        {
            get { return _field; }
            protected set
            {
                Regex pattern = new Regex(@".{3,20}");
                if (!pattern.IsMatch(value)) throw new Exception("field is not in correct format");
                _field = value;
            }
        }
        public Person(string name, string SSN, string field, Gender gender)
        {
            this.name = name;
            this.SSN = SSN;
            this.field = field;
            this.gender = gender;
            people.Add(this);
        }
        public Person(Person input)
        {
            name = input.name;
            SSN = input.SSN;
            field = input.field;
            gender = input.gender;
        }
        //utility
        bool SSNIsRegistered(string ssn)
        {
            foreach (var person in people)
            {
                if (person.SSN == ssn)
                {
                    return true;
                }
            }
            return false;
        }
        public static Person findPerson(string ssn)
        {
            foreach (var person in people)
            {
                if (person.SSN == ssn)
                {
                    return person;
                }
            }
            throw new Exception("this person is not registred in system");
        }
        public static void replace(Person a, Person b)
        {
            people.Remove(a);
            people.Add(b);
        }
    }
    class Professor : Person
    {
        int _roomNumber;
        int _minTRA;
        List<ResearchAssistant> researchAssistants = new List<ResearchAssistant>();
        int roomNumber
        {
            get { return _roomNumber; }
            set
            {
                if (!(1 <= value && value <= 1000)) throw new Exception("room number should be in range 1 to 1000");
                _roomNumber = value;
            }
        }
        public int minTRA
        {
            get { return _minTRA; }
            private set
            {
                if (value < 0) throw new Exception("minimum TRA cant be negetive");
                _minTRA = value;
            }
        }
        public Professor(string name, string SSN, string field, Gender gender, int roomNumber, int minTRA) : base(name, SSN, field, gender)
        {
            this.roomNumber = roomNumber;
            this.minTRA = minTRA;
        }
        public Professor(string name, string SSN, string field, Gender gender) : base(name, SSN, field, gender)
        {
        }
        public void addResearchAssisstant(ResearchAssistant RA)
        {
            researchAssistants.Add(RA);
        }
        public void printStatus()
        {
            Console.WriteLine($"{name} {field} {roomNumber} {minTRA}");
            List<string> units = Unit.getProfessorUnits(this);
            if(units.Count != 0)
            {
                Console.WriteLine("units");
                foreach(var unit in units)
                {
                    Console.Write($"{unit} ");
                }
                Console.Write("\n");
            }
            if(researchAssistants.Count != 0)
            {
                Console.WriteLine("Research Assisstants:");
                foreach(var RA in researchAssistants)
                {
                    Console.Write($"{RA.name} ");
                }
                Console.Write("\n");
            }
        }
    }
    class Student : Person
    {
        int _enteringYear;
        int enteringYear
        {
            get { return _enteringYear; }
            set
            {
                if (!(1350 <= value && value <= DateTime.Now.Year)) throw new Exception($"year should be between 1350 and {DateTime.Now.Year}");
                _enteringYear = value;
            }
        }
        public Student(string name, string SSN, string field, Gender gender, int enteringYear) : base(name, SSN, field, gender)
        {
            this.enteringYear = enteringYear;
        }
        public Student(Student input) : base(input)
        {
            enteringYear = input.enteringYear;
        }
        public void convertToTA(int unitID)
        {
            Unit unit = Unit.findUnit(unitID);
            if (unit.field != field) throw new Exception("field dont match");
            if (unit.professorSSN == null) throw new Exception("this unit already has a professor");
            if (this is TeacherAssistant || this is ResearchAssistant) throw new Exception("this student already has a role");
            TeacherAssistant TA = new TeacherAssistant(this, unitID);
            unit.addTA(TA);
            replace(this, TA);
            Unit.updateStudent(TA);
        }
        public void convertToRA(string professorSSN, string projectName, int freeTime)
        {
            ResearchAssistant RA = new ResearchAssistant(this, projectName, professorSSN, freeTime);
            Professor professor = findPerson(professorSSN) as Professor;
            if (professor.field != field) throw new Exception("field dont match");
            if (professor == null) throw new Exception("professor not found");
            if (professor.minTRA <= freeTime) throw new Exception("free time isnt enough for this professor");
            if (this is TeacherAssistant || this is ResearchAssistant) throw new Exception("this student already has a role");
            professor.addResearchAssisstant(RA);
            replace(this, RA);
            Unit.updateStudent(RA);
        }
        public void printStatus()
        {

            Console.WriteLine($"{name} {enteringYear} {field}");
            List<string> units = Unit.getStudentUnits(this);
            if (units.Count != 0)
            {
                Console.WriteLine("Units:");
                foreach (var unit in units)
                {
                    Console.Write($"{unit} ");
                }
                Console.WriteLine($"\n");
            }
            if (this is TeacherAssistant) Console.Write("this student is TA\n");
            if (this is ResearchAssistant) Console.Write("this student is RA\n");
        }
    }
    class TeacherAssistant : Student
    {
        int _unitID;
        int unitID
        {
            get { return _unitID; }
            set
            {
                if (value < 0) throw new Exception("Unit ID cant be negetive");
                _unitID = value;
            }
        }
        public TeacherAssistant(Student father, int unitID) : base(father)
        {
            this.unitID = unitID;
        }
    }
    class ResearchAssistant : Student
    {
        string _projectName;
        int _freeTime;
        string professorSSN;
        int freeTime
        {
            get { return _freeTime; }
            set
            {
                if (value < 0) throw new Exception("free Time value cant be negetive");
                _freeTime = value;
            }
        }
        string projectName
        {
            get { return _projectName; }
            set
            {
                Regex pattern = new Regex(@".{1,30}");
                if (!pattern.IsMatch(value)) throw new Exception("project name lenght should be between 1 and 30");
                _projectName = value;
            }
        }
        public ResearchAssistant(string name, string SSN, string field, Gender gender, int enteringYear, string projectName, int freeTime, string professorSSN) : base(name, SSN, field, gender, enteringYear)
        {
            this.professorSSN = professorSSN;
            this.projectName = projectName;
            this.freeTime = freeTime;
        }
        public ResearchAssistant(Student father, string projectName, string professorSSN, int freeTime) : base(father)
        {
            this.projectName = projectName;
            this.freeTime = freeTime;
            this.professorSSN = professorSSN;
        }
    }
    class Unit
    {
        int _unitID;
        string _name;
        string _field;
        int _maxSize;
        string _professorSSN = null;
        List<TeacherAssistant> teachingAssistants = new List<TeacherAssistant>();
        List<Student> students = new List<Student>();
        static List<Unit> units = new List<Unit>();
        int unitID
        {
            get { return _unitID; }
            set
            {
                if (!(1 <= value && value <= 100000) || IDIsRegistered(value)) throw new Exception("ID should be between 1 to 1000");
                _unitID = value;
            }
        }
        string name
        {
            get { return _name; }
            set
            {
                Regex pattern = new Regex(@".{3,20}");
                if (pattern.IsMatch(value)) throw new Exception("name is not in the correct format");
                _name = value;
            }
        }
        public string field
        {
            get { return _field; }
            private set
            {
                Regex pattern = new Regex(@".{3,20}");
                if (pattern.IsMatch(value)) throw new Exception("field is not in the correct format");
                _field = value;
            }
        }
        int maxSize
        {
            get { return _maxSize; }
            set
            {
                if (!(10 <= value && value <= 180)) throw new Exception("max student size should be between 1 to 180");
                _maxSize = value;
            }
        }
        public string professorSSN
        {
            get { return _professorSSN; }
            private set
            {
                Regex pattern = new Regex(@"\d{10}");
                if (!pattern.IsMatch(value)) throw new Exception("SSN is not in correct format");
                _professorSSN = value;
            }
        }
        public Unit(int unitID, string name, string field, int maxSize)
        {
            this.unitID = unitID;
            this.name = name;
            this.maxSize = maxSize;
            this.field = field;
            units.Add(this);
        }
        static bool IDIsRegistered(int ID)
        {
            foreach (var unit in units)
            {
                if (ID == unit.unitID)
                {
                    return true;
                }
            }
            return false;
        }
        public void addStudent(Student student)
        {
            if (student == null) throw new Exception("student does not exist");
            if (students.Contains(student)) throw new Exception("already a part of Unit");
            if (students.Count == maxSize) throw new Exception("this unit is full");
            if (student.field != field) throw new Exception("field dont match");
            students.Add(student);
        }
        public void addProfessor(Professor professor)
        {
            if (professor == null) throw new Exception("professor does not exist");
            if (professorSSN != null) throw new Exception("already has a professor");
            if (professor.field != field) throw new Exception("field dont match");
            professorSSN = professor.SSN;
        }
        public void addTA(TeacherAssistant input)
        {
            teachingAssistants.Add(input);
        }
        public void printStatus()
        {
            if (professorSSN == null) Console.Write("none ");
            else
            {
                Professor professor = Person.findPerson(professorSSN) as Professor;
                Console.Write($"{professor.name} ");
            }
            Console.WriteLine($"{maxSize} {field}");
            if(teachingAssistants.Count != 0)
            {
                Console.WriteLine("Teacher Assisstants:");
                foreach (var TA in teachingAssistants)
                {
                    Console.WriteLine($"{TA.name} ");
                }
                Console.Write("\n");
            }
            if(students.Count != 0)
            {
                Console.WriteLine("participants:");
                foreach (var student in students)
                {
                    Console.Write($"{student.name} ");
                }
                Console.Write("\n");
            }
        }
        //utility
        public static Unit findUnit(int id)
        {
            foreach (var unit in units)
            {
                if (id == unit.unitID)
                {
                    return unit;
                }
            }
            throw new Exception("no unit found");
        }
        public static void updateStudent(Student input)
        {
            foreach (var unit in units)
            {
                unit.update(input);
            }
        }
        void update(Student input)
        {
            for (int i = 0; i < students.Count; i++)
            {
                if (input.SSN == students[i].SSN)
                {
                    students[i] = input;
                }
            }
        }
        public static List<string> getStudentUnits(Student student)
        {
            List<string> ans = new List<string>();
            foreach(var unit in units)
            {
                if (unit.students.Contains(student))
                {
                    ans.Add(unit.name);
                }
            }
            return ans;
        }
        public static List<string> getProfessorUnits(Professor professor)
        {
            List<string> ans = new List<string>();
            foreach (var unit in units)
            {
                if (unit.professorSSN == professor.SSN)
                {
                    ans.Add(unit.name);
                }
            }
            return ans;
        }
        public bool studentIsInUnit(Student input)
        {
            foreach(var student in students)
            {
                if(student == input)
                {
                    return true;
                }
            }
            return false;
        }
    }
    class Mark
    {
        static List<Mark> marks = new List<Mark>();
        int _unitID;
        string _professorSSN;
        string _studentSSN;
        double _value;
        int unitID
        {
            get { return _unitID; }
            set
            {
                if (!(1 <= value && value <= 100000)) throw new Exception("ID should be between 1 to 1000");
                _unitID = value;
            }
        }
        public string professorSSN
        {
            get { return _professorSSN; }
            private set
            {
                Regex pattern = new Regex(@"\d{10}");
                if (!pattern.IsMatch(value)) throw new Exception("SSN is not in correct format");
                _professorSSN = value;
            }
        }
        public string studentSSN
        {
            get { return _studentSSN; }
            private set
            {
                Regex pattern = new Regex(@"\d{10}");
                if (!pattern.IsMatch(value)) throw new Exception("SSN is not in correct format");
                _studentSSN = value;
            }
        }
        public double value
        {
            get { return _value; }
            set
            {
                if (!(0 <= value && value <= 20)) throw new Exception("mark should be between 0 and 20");
                _value = value;
            }
        }
        public Mark(int unitID, string professorSSN, string studentSSN, double value)
        {
            if (!(Person.findPerson(professorSSN) is Professor)) throw new Exception("no professor found");
            if (!(Person.findPerson(studentSSN) is Student)) throw new Exception("no student found");
            Unit unit = Unit.findUnit(unitID);
            if (unit.professorSSN != professorSSN) throw new Exception("this is not the professor of this unit");
            if (!unit.studentIsInUnit(Person.findPerson(studentSSN) as Student)) throw new Exception("this student is not in the unit");
            this.unitID = unitID;
            this.professorSSN = professorSSN;
            this.studentSSN = studentSSN;
            this.value = value;
            marks.Add(this);
        }
        public static void setMark(int unitID, string professorSSN, string studentSSN, int value)
        {
            Mark mark = findMark(unitID, professorSSN, studentSSN);
            if(mark != null)
                mark.value = value;
            else
            {
                new Mark(unitID, professorSSN, studentSSN, value);
            }
        }
       public static Mark findMark(int unitID, string professorSSN, string studentSSN)
        {
            foreach(var mark in marks)
            {
                if(mark.unitID == unitID && mark.professorSSN == professorSSN && mark.studentSSN == studentSSN)
                {
                    return mark;
                }
            }
            return null;
        }
    }
    class Program
    {
        static string Prompt(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    string command = Prompt("enter your command:");
                    switch (command)
                    {
                        case "register_student":
                            {
                                new Student(Prompt("name:"), Prompt("SSN:"), Prompt("field:"), (Gender)Enum.Parse(typeof(Gender), Prompt("gender:")), int.Parse(Prompt("entering year:")));
                                break;
                            }
                        case "register_professor":
                            {
                                new Professor(Prompt("name:"), Prompt("SSN:"), Prompt("field:"), (Gender)Enum.Parse(typeof(Gender), Prompt("gender:")));
                                break;
                            }
                        case "make_unit":
                            {
                                new Unit(int.Parse(Prompt("unit ID:")), Prompt("name:"), Prompt("field:"), int.Parse(Prompt("max size:")));
                                break;
                            }
                        case "add_student":
                            {
                                Unit unit = Unit.findUnit(int.Parse(Prompt("unit id:")));
                                Student student = Person.findPerson(Prompt("SSN:")) as Student;
                                unit.addStudent(student);
                                break;
                            }
                        case "add_professor":
                            {
                                Unit unit = Unit.findUnit(int.Parse(Prompt("unit id:")));
                                Professor professor = Person.findPerson(Prompt("SSN:")) as Professor;
                                unit.addProfessor(professor);
                                break;
                            }
                        case "set_student_teaching_assistant":
                            {
                                Student student = Person.findPerson(Prompt("SSN:")) as Student;
                                if (student == null) throw new Exception("no student found");
                                student.convertToTA(int.Parse(Prompt("unit ID:")));
                                break;
                            }
                        case "set_student_research_assistant":
                            {
                                Student student = Person.findPerson(Prompt("student SSN:")) as Student;
                                if (student == null) throw new Exception("no student found");
                                student.convertToRA(Prompt("Professor SSN:"), Prompt("Project Name:"), int.Parse(Prompt("free time:")));
                                break;
                            }
                        case "student_status":
                            {
                                Student student = Person.findPerson(Prompt("student SSN:")) as Student;
                                if (student == null) throw new Exception("no student found");
                                student.printStatus();
                                break;
                            }
                        case "professor_status":
                            {
                                Professor professor = Person.findPerson(Prompt("professor SSN:")) as Professor;
                                if (professor == null) throw new Exception("no professor found");
                                professor.printStatus();
                                break;
                            }
                        case "unit_status":
                            {
                                Unit unit = Unit.findUnit(int.Parse(Prompt("unit ID:")));
                                unit.printStatus();
                                break;
                            }
                        case "set_final_mark":
                            {
                                Mark.setMark(int.Parse(Prompt("unit ID:")), Prompt("Professor SSN:"), Prompt("student SSN:"), int.Parse(Prompt("value")));
                                break;
                            }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
