﻿using System;
using SQLitePCL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPuzzle.ViewModels
{
    class RecordViewModel
    {
        private ObservableCollection<Models.Record> _recordList = new ObservableCollection<Models.Record>();
        public ObservableCollection<Models.Record> RecordList { get { return this._recordList; } }

        public RecordViewModel()
        {
            var db = App.conn;
            string sql_load = @"SELECT * FROM Record";
            using (var statement = db.Prepare(sql_load))
            {
                while (statement.Step() != SQLiteResult.DONE)
                {
                    DateTime date = DateTime.Parse((string)statement[3]);
                    _recordList.Add(new Models.Record((long)statement[0],
                                                    (string)statement[1],
                                                    (long)statement[2],
                                                    date));
                }
            }
        }

        public void addRecord(string name, long time, DateTime date)
        {
            var db = App.conn;
            //insert into db
            string sql_insert = @"INSERT
                                    INTO Record(Name, FinishTime, Date)
                                    VALUES (?,?,?)";
            var id = (long)0;
            string sql_select = @"SELECT last_insert_rowid() FROM Record";
            try
            {
                using (var statement = db.Prepare(sql_insert))
                {
                    statement.Bind(1, name);
                    statement.Bind(2, time);
                    statement.Bind(3, date.ToString());
                    statement.Step();
                }
                using (var statement = db.Prepare(sql_select))
                {
                    while (statement.Step() != SQLiteResult.DONE)
                    {
                        id = (long)statement[0];
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
            var newRecord = new Models.Record(id, name, time, date);
            this._recordList.Add(newRecord);
        }

        public ObservableCollection<Models.Record> getTop5Player()
        {
            return new ObservableCollection<Models.Record>(from i in RecordList orderby i.finishTime select i);
        }
    }
}