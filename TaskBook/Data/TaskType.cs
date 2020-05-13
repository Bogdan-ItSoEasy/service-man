using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TaskBook.Data
{
    [DataContract]
    public class CommonTask : Task
    {
        
    }
    [DataContract]
    public class BirthTask : Task
    {

        public BirthTask()
        {
            ImportantId = 4;
            RepeaterId = 5;
        }
        

        public override string TaskInfo
        {
            get
            {
                return string.Format("{0} {1} {2}\nИсполняется {3} {4}{5}", Surname, Name, Farthername, TaskDate.Year - RemindDate.Year, GetYearName(TaskDate.Year - RemindDate.Year), 
                Comment == default? Comment:"\n"+Comment);
            }
            set
            {
                
            }
        }

        public override string ToString()
        {
            return "1";
        }

        public override void UpdateTime()
        {           
            base.UpdateTime();
  
            OnPropertyChanged("TaskInfo");
        }

        private string GetYearName(int year)
        {
            
            List<int> twoType = new List<int>()
            {
                2,3,4
            };   

            if (year % 10 == 1)
            {
                return "год";
            }
            else if (twoType.Contains(year%10))
            {
                return "года";
            }
            else
            {
                return "лет";
            }
        }

        [DataMember]
        public DateTime BirthDay
        {
            get {return _birthDay; }
            set
            {
                _birthDay = value;
                OnPropertyChanged("TaskInfo");
            }
        }
        DateTime _birthDay;
        [DataMember]
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("TaskInfo");
            }
        }
        string _surname;
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("TaskInfo");
            }
        }
        string _name;
        [DataMember]
        public string Farthername
        {
            get { return _fartherName; }
            set
            {
                _fartherName = value;
                OnPropertyChanged("TaskInfo");
            }
        }
        string _fartherName;

        [DataMember]
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged("TaskInfo");
            }
        }
        string _comment;

    }

    public class SeparatorTask : Task
    {

    };
}