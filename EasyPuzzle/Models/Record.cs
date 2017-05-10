using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPuzzle.Models
{
    public class Record
    {
        //unique identity
        public long id { get; }
        //player's name 
        private string _name;
        //player's finish time
        private long _finishTime;
        //the date of record
        private DateTime _date;

        //constructor
        public Record(long id, string name, long time, DateTime date)
        {
            this.id = id;
            this.name = name;
            this.finishTime = time;
            this.date = date;
        }

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        public long finishTime
        {
            get { return _finishTime; }
            set { _finishTime = value; }
        }
        public DateTime date
        {
            get { return _date; }
            set { _date = value; }
        }
    }
}
