using System;
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
                    _recordList.Add(new Models.Record((string)statement[0], (string)statement[1]));
                }
            }
        }

        /*public void addRecord(string name, string time)
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
                //todo:handling exception
            }
            var newRecord = new Models.Record(name, time);
            this._recordList.Add(newRecord);
        }*/

       // public void removeRecord(long id)
       // {
            //var toDelete = RecordList.Single<Models.Record>(i => i.id == id);
            //RecordList.Remove(toDelete);
            //var db = App.conn;
            //string sql_delete = @"DELETE
            //                      FROM Record
            //                      WHERE Id = ?";
            //using (var statement = db.Prepare(sql_delete))
            //{
            //    statement.Bind(1, id);
            //    statement.Step();
            //}
        //}

        public List<Models.Record> getTop5Players()
        {
            //???how to get slice of 1-5
            List<Models.Record> allRecord = (new ObservableCollection<Models.Record>(from i in RecordList orderby i.finishTime select i)).ToList<Models.Record>();
            List<Models.Record> top5Players = new List<Models.Record>();
            for (int i = 0; i < 5; i++)
            {
                if (i >= allRecord.Count)
                    break;
                top5Players.Add(allRecord[i]);
            }          

            return top5Players;
        }
    }
}