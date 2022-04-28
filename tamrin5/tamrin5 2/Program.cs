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
        List<Person> people = new List<Person>();
        protected string name
        {
            get { return _name; }
            set
            {
                Regex pattern = new Regex(@".{3,20}");
                if (!pattern.IsMatch(value)) throw new Exception("name is not in correct format");
                _name = value;
            }
        }
        protected string SSN
        {
            get { return _SSN; }
            set
            {
                Regex pattern = new Regex(@"\d{10}");
                if (!pattern.IsMatch(value) && SSNIsRegistered(SSN)) throw new Exception("SSN is not in correct format");
                _SSN = value;
            }
        }
        protected string field
        {
            get { return _field; }
            set
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
        public TeacherAssistant(string name, string SSN, string field, Gender gender, int enteringYear, int unitID) : base(name, SSN, field, gender, enteringYear)
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
        string _professorSSN;
        List<ResearchAssistant> teachingAssistants = new List<ResearchAssistant>();
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
