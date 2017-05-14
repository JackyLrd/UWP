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
                    _recordList.Add(new Models.Record((string)statement[0], (long)statement[1]));
                }
            }
        }

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