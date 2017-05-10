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

        public string name { get; set; }
        public long finishTime { get; set; }
        public DateTime date { get; set; }
    }
}
