using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace tamrin5_2
{
    enum Gender { male, female, other}
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
            foreach(var person in people)
            {
                if(person.SSN == ssn)
                {
                    return true;
                }
            }
            return false;
        }
        public static Person findPerson(string ssn)
        {
            foreach(var person in people)
            {
                if(person.SSN == ssn)
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
        int minTRA
        {
            get { return _minTRA; }
            set
            {
                if (value < 0) throw new Exception("minimum TRA cant be negetive");
                _minTRA = value;
            }
        }
        public Professor(string name, string SSN, string field, Gender gender) : base(name, SSN, field, gender)
        {
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
        string field
        {
            get { return _field; }
            set
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
        protected string professorSSN
        {
            get { return _professorSSN; }
            set
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
            foreach(var unit in units)
            {
                if(ID == unit.unitID)
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
        //utility
        public static Unit findUnit(int id)
        {
            foreach(var unit in units)
            {
                if(id == unit.unitID)
                {
                    return unit;
                }
            }
            throw new Exception("no unit found");
        }
        public static void updateStudent(Student input)
        {
            foreach(var unit in units)
            {
                unit.update(input);
            }
        }
        void update(Student input)
        {
            for(int i = 0; i < students.Count; i++)
            {
                if(input.SSN == students[i].SSN)
                {
                    students[i] = input;
                }
            }
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
                                int unitID = int.Parse(Prompt("unit ID:"));
                                Unit unit = Unit.findUnit(unitID);
                                Student student = Person.findPerson(Prompt("SSN:")) as Student;
                                TeacherAssistant TA = new TeacherAssistant(student, unitID);
                                unit.addTA(TA);
                                Person.replace(student, TA);
                                Unit.updateStudent(TA);
                                break;
                            }
                        case "set_student_teaching_assistant":
                            {
                                int unitID = int.Parse(Prompt("unit ID:"));
                                Unit unit = Unit.findUnit(unitID);
                                Student student = Person.findPerson(Prompt("SSN:")) as Student;
                                TeacherAssistant TA = new TeacherAssistant(student, unitID);
                                unit.addTA(TA);
                                Person.replace(student, TA);
                                Unit.updateStudent(TA);
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
