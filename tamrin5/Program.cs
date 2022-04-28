using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace tamrin5
{
    class Time
    {
        int _hour;
        int _minuet;
        protected int hour
        {
            get { return _hour; }
            set { if (0 <= value && value < 24) _hour = value; else throw new Exception("hour should be between 0 and 24"); }
        }
        protected int minuet
        {
            get { return _minuet; }
            set { if (0 <= value && value < 24) _minuet = value; else throw new Exception("minuet should be between 0 and 60"); }
        }
        public virtual void change(int hour, int minuet)
        {
            this.hour = hour;
            this.minuet = minuet;
        }
    }
    class Day : Time
    {
        int _lunarDay;
        int _sonarDay;
        protected int lunarDay
        {
            get { return _lunarDay; }
            set { if (1 <= value && value < 31) _lunarDay = value; else throw new Exception("day should be between 1 and 31"); }
        }
        protected int sonarDay
        {
            get { return _sonarDay; }
            set { if (1 <= value && value < 31) _sonarDay = value; else throw new Exception("day should be between 1 and 31"); }
        }
        public override void change(int lunarDay, int sonarDay)
        {
            this.lunarDay = lunarDay;
            this.sonarDay = sonarDay;
        }
    }
    class Month : Day
    {
        protected string lunarName;
        protected string sonarName;
        int _lunarMonth;
        int _sonarMonth;
        protected int lunarMonth
        {
            get { return _lunarMonth; }
            set { if (0 <= value && value < 12) _lunarMonth = value; else throw new Exception("month should be between 1 and 12"); }
        }
        protected int sonarMonth
        {
            get { return _sonarMonth; }
            set { if (0 <= value && value < 12) _sonarMonth = value; else throw new Exception("month should be between 1 and 12"); }
        }
        public override void change(int lunarMonth, int sonarMonth)
        {
            this.lunarMonth = lunarMonth;
            this.sonarMonth = sonarMonth;
        }
    }
    class Date : Month
    {
        Regex sonarDatePattern = new Regex(@"^\d{4}_\d{2}_\d{2}$");
        Regex lunarDatePattern = new Regex(@"^\d{2}_\d{2}_\d{4}$");
        int _lunarYear;
        int _sonarYear;
        protected int lunarYear
        {
            get { return _lunarYear; }
            set { if (1 <= value) _lunarYear = value; else throw new Exception("year should be positive"); }
        }
        protected int sonarYear
        {
            get { return _sonarYear; }
            set { if (1 <= value) _sonarYear = value; else throw new Exception("year should be positive"); }
        }
        public override void change(int lunarYear, int sonarYear)
        {
            this.lunarYear = lunarYear;
            this.sonarYear = sonarYear;
        }
        public Date(string lunarDate, string sonarDate)
        {
            extractLunarDate(lunarDate);
            extractSonarDate(sonarDate);
        }
        public Date(string date)
        {
            if (verifyLunarDate(date))
            {
                extractLunarDate(date);
            }
            if (verifySonarDate(date))
            {
                extractSonarDate(date);
            }
        }
        //utility
        void extractSonarDate(string sonarDate)
        {
            if (!verifySonarDate(sonarDate)) throw new Exception("sonar date is not in the correct format");
            sonarYear = int.Parse(sonarDate.Substring(0, 4));
            sonarMonth = int.Parse(sonarDate.Substring(5, 2));
            sonarDay = int.Parse(sonarDate.Substring(8, 2));
        }
        void extractLunarDate(string lunarDate)
        {
            if (!verifyLunarDate(lunarDate)) throw new Exception("lunar date is not in the correct format");
            lunarYear = int.Parse(lunarDate.Substring(5, 4));
            lunarMonth = int.Parse(lunarDate.Substring(3, 2));
            lunarDay = int.Parse(lunarDate.Substring(0, 2));
        }
        bool verifySonarDate(string sonarDate)
        {
            return sonarDatePattern.IsMatch(sonarDate);
        }
        bool verifyLunarDate(string lunarDate)
        {
            return lunarDatePattern.IsMatch(lunarDate);
        }
        public override bool Equals(object obj)
        {
            Date b = obj as Date;
            return (hour == b.hour && minuet == b.minuet) && (lunarMonth == b.lunarMonth && lunarDay == b.lunarDay && lunarYear == b.lunarYear) || (sonarMonth == b.sonarMonth && sonarDay == b.sonarDay && sonarYear == b.sonarYear);
        }
        public static bool operator== (Date a, Date b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Date a, Date b)
        {
            return !(a == b);
        }
    }
    class Calender
    {
        static int counter = 1;
        int ID;
        public string name { get; }
        bool state = false;
        List<Event> events = new List<Event>();
        public Calender(string title)
        {
            name = title;
            ID = counter;
            counter++;
        }
        public bool matchID(int id)
        {
            return id == ID;
        }
        public void activate()
        {
            state = true;
        }
        public void deActivate()
        {
            state = false;
        }
        public bool isActive()
        {
            return state;
        }
        public void addEvent(string title, Meeting type, string sonarDate, string lunarDate)
        {
            Date date = new Date(lunarDate, sonarDate);
            Event newEvent = new Event(date, title, type);
            events.Add(newEvent);
        }
        public void deleteEvent(string title)
        {
            foreach(var Event in events)
            {
                if(Event.title == title)
                {
                    events.Remove(Event);
                    return;
                }
            }
            throw new Exception("no event with this title");
        }
        public Event findEventByTitle(string name)
        {
            foreach(var Event in events)
            {
                if (Event.title == name)
                    return Event;
            }
            throw new Exception("no event with this name exist");
        }
        public List<Event> getEventsOn(Date date)
        {
            List<Event> ans = new List<Event>();
            foreach(var Event in events)
            {
                if (Event.date == date)
                {
                    ans.Add(Event);
                }
            }
            return ans;
        }
    }
    enum Meeting { Conference, VIP, Ceremony, TradeShow };
    struct Event
    {
        public Date date { get; }
        public string title;
        static int counter = 1;
        int ID;
        Meeting type;
        public Event(Date date, string title, Meeting type)
        {
            this.date = date;
            this.title = title;
            this.type = type;
            ID = counter++;
        }
        public string getString()
        {
            return $"Event: {title} {type}";
        }
    }
    class Account
    {
        static List<Account> accounts = new List<Account>();
        static Regex passwordPattern = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[\w_]{5,}$");
        static Regex usernamePattern = new Regex(@"^[\w_]*$");
        string _username;
        string _password;
        List<Calender> calenders = new List<Calender>();
        string username
        {
            get { return _username; }
            set { if (isValidUsername(value)) _username = value; else throw new Exception("username is not valid"); }
        }
        string password
        {
            get { return _password; }
            set { if (isValidPassword(value)) _password = value; else throw new Exception("password is not valid"); }
        }
        public Account(string username, string password)
        {
            this.username = username;
            this.password = password;
            calenders.Add(new Calender(username));
        }
        //main functions
        public static void addAccount(string username, string password)
        {
            Account newAccount = new Account(username, password);
            accounts.Add(newAccount);
        }
        public static Account login(string username, string password)
        {
            foreach(var account in accounts)
            {
                if (account.username == username)
                    if (account.password == password)
                        return account;
                    else
                        throw new Exception("password didnt match");
            }
            throw new Exception("username not found");
        }
        public static void changePassword(string username, string oldPass, string newPass)
        {
            Account account = findAccountByUsername(username);
            if (account.password == oldPass)
                account.password = newPass;
            else
                throw new Exception("old password is incorrect");
        }
        public static void Remove(string username, string password)
        {
            Account account = findAccountByUsername(username);
            if (account.password == password)
                accounts.Remove(account);
            else
                throw new Exception("password is incorrect");
        }
        public static string getAllUsernames()
        {
            if (accounts.Count == 0) throw new Exception("nothing found");
            string result = "";
            sortByASCCI();
            foreach(var account in accounts)
            {
                result += $"{account.username}\n";
            }
            return result;
        }
        public void addCalender(string title)
        {
            Calender newCalender = new Calender(title);
            calenders.Add(newCalender);
        }
        public void activateDate(int id)
        {
            Calender calender = findCalenderByID(id);
            calender.activate();
        }
        public void deActivateDate(int id)
        {
            Calender calender = findCalenderByID(id);
            calender.deActivate();
        }
        public void deleteDate(int id)
        {
            Calender calender = findCalenderByID(id);
            calenders.Remove(calender);
        }
        public void changeDate(int id, string title, EditMode mode, int value1, int value2)
        {
            Calender calender = findCalenderByID(id);
            Date date = calender.findEventByTitle(title).date;
            switch (mode)
            {
                case EditMode.ChangeTime:
                    {
                        Time time = date;
                        time.change(value1, value2);
                        break;
                    }
                case EditMode.ChangeDay:
                    {
                        Day day = date;
                        day.change(value1, value2);
                        break;
                    }
                case EditMode.ChangeMonth:
                    {
                        Month month = date;
                        month.change(value1, value2);
                        break;
                    }
                case EditMode.ChangeYear:
                    {
                        date.change(value1, value2);
                        break;
                    }
            }
        }
        public List<Event> getActiveEvents(Date date)
        {
            List<Event> ans = new List<Event>();
            foreach(var calender in calenders)
            {
                if (calender.isActive())
                    ans = mergeEventList(ans ,calender.getEventsOn(date));
            }
            return ans;
        }
        public void showActiveCalenders()
        {
            bool flag = false;
            foreach (var calender in calenders)
            {
                if (calender.isActive())
                {
                    Console.WriteLine($"{calender.name}");
                    flag = true;
                }
            }
            if (!flag)
            {
                Console.WriteLine("nothing found");
            }
        }
        //utility functions
        List<Event> mergeEventList(List<Event> a, List<Event> b)
        {
            List<Event> ans = new List<Event>();
            foreach(var Event in b)
            {
                ans.Add(Event);
            }
            foreach (var Event in a)
            {
                ans.Add(Event);
            }
            return ans;
        }
        public Calender findCalenderByID(int id)
        {
            foreach(var calender in calenders)
            {
                if (calender.matchID(id))
                    return calender;
            }
            throw new Exception("no date with this ID");
        }
        static void sortByASCCI()
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                for (int j = 0; j < accounts.Count; j++)
                {
                    if (string.Compare(accounts[i].username, accounts[j].username) > 0)
                    {
                        string temp = accounts[i]._username;
                        accounts[i]._username = accounts[j]._username;
                        accounts[j]._username = temp;
                    }
                }
            }
        }
        static Account findAccountByUsername(string username)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].username == username)
                    return accounts[i];
            }
            throw new Exception("no accounts with this name found");
        }
        static bool isValidUsername(string username)
        {
            return usernamePattern.IsMatch(username) && isRegisteredUsername(username);
        }
        static bool isValidPassword(string password)
        {
            return passwordPattern.IsMatch(password);
        }
        static bool isRegisteredUsername(string username)
        {
            foreach (var account in accounts)
            {
                if (username == account.username)
                    return true;
            }
            return false;
        }
    }
    enum EditMode { ChangeTime, ChangeDay, ChangeMonth, ChangeYear}
    class Program
    {
        static string Prompt(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
        static void OpenDateMenu(Calender calender)
        {
            while (true)
            {
                string commnad = Prompt("enter your command");
                switch (commnad)
                {
                    case "Add Event":
                        {
                            calender.addEvent(Prompt("title:"), (Meeting)Enum.Parse(typeof(Meeting), Prompt("type:")), Prompt("sonar date:"), Prompt("lunar date:"));
                            break;
                        }
                    case "Delete Event":
                        {
                            calender.deleteEvent(Prompt("title"));
                            break;
                        }
                    case "Back":
                        {
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("command unknown");
                            break;
                        }
                }
            }
        }
        static void sortEventList(List<Event> events)
        {
            for(int i = 0; i < events.Count - 1; i++)
            {
                for(int j = i + 1; j < events.Count; j++)
                {
                    if(string.Compare(events[i].title, events[j].title) > 0)
                    {
                        Event temp = events[j];
                        events[j] = events[i];
                        events[i] = temp;
                    }
                }
            }
        }
        static void printEvents(List<Event> events)
        {
            foreach(var Event in events)
            {
                Console.WriteLine(Event.getString());
            }
        }
        static void Main(string[] args)
        {
            Account LogedInAccount = null;
            while (true)
            {
                try
                {
                    string command = Prompt("Please enter your command:");
                    if (LogedInAccount == null)
                    {
                        switch (command)
                        {
                            case "Register":
                                {
                                    Account.addAccount(Prompt("username:"), Prompt("password:"));
                                    break;
                                }
                            case "Login":
                                {
                                    LogedInAccount = Account.login(Prompt("username:"), Prompt("password:"));
                                    break;
                                }
                            case "Change Password":
                                {
                                    Account.changePassword(Prompt("username:"), Prompt("old password:"), Prompt("new password"));
                                    break;
                                }
                            case "Remove":
                                {
                                    Account.Remove(Prompt("username:"), Prompt("password:"));
                                    break;
                                }
                            case "Show All Usernames":
                                {
                                    string allUsernames = Account.getAllUsernames();
                                    Console.WriteLine($"******\n{allUsernames}\n******");
                                    break;
                                }
                            case "Exit":
                                {
                                    return;
                                }
                            default:
                                {
                                    Console.WriteLine("command unknown");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        switch (command)
                        {
                            case "Create New Calendar":
                                {
                                    LogedInAccount.addCalender(Prompt("title:"));
                                    break;
                                }
                            case "Enable Calendar":
                                {
                                    LogedInAccount.activateDate(int.Parse(Prompt("ID:")));
                                    break;
                                }
                            case "Disable Calendar":
                                {
                                    LogedInAccount.deActivateDate(int.Parse(Prompt("ID:")));
                                    break;
                                }
                            case "Delete Calendar":
                                {
                                    LogedInAccount.deActivateDate(int.Parse(Prompt("ID:")));
                                    break;
                                }
                            case "Edit Calendar":
                                {
                                    LogedInAccount.changeDate(int.Parse(Prompt("ID:")), Prompt("Event Title:"), (EditMode)Enum.Parse(typeof(EditMode), Prompt("Mode:")), int.Parse(Prompt("value1:")), int.Parse(Prompt("value2:")));
                                    break;
                                }
                            case "Open Calendar":
                                {
                                    Calender calender = LogedInAccount.findCalenderByID(int.Parse("ID:"));
                                    OpenDateMenu(calender);
                                    break;
                                }
                            case "Show":
                                {
                                    string temp = Prompt("Date:");
                                    Date wantedDate = new Date(temp);
                                    List<Event> events = LogedInAccount.getActiveEvents(wantedDate);
                                    sortEventList(events);
                                    Console.WriteLine($"events on {temp}");
                                    printEvents(events);
                                    break;
                                }
                            case "Show Enabled Calendars":
                                {
                                    LogedInAccount.showActiveCalenders();
                                    break;
                                }
                            case "Logout":
                                {
                                    LogedInAccount = null;
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("command unknown");
                                    break;
                                }
                        }  
                    }

                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
