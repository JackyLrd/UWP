﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPuzzle.Models
{
    public class Record
    {
        //player's name 
        private string _name;
        //player's finish time
        private string _finishTime;

        //constructor
        public Record(string name, string time)
        {
            this.name = name;
            this.finishTime = time;
        }

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public string finishTime
        {
            get { return _finishTime; }
            set { _finishTime = value; }
        }
    }
}
