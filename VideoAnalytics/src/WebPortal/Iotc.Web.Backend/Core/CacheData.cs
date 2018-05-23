using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Iotc.Web.Backend.Models
{
    public class CacheData
    {
        private List<HistoryData> _list = null;
        private int COUNT = 10;

        public CacheData()
        {
            _list = new List<HistoryData>();
        }


        public void ClearData()
        {
            _list.Clear();
        }

        public void AddData(string deviceId, string time, string img, string text, string linktext)
        {
            HistoryData d = new HistoryData(time, deviceId, text,img,linktext);
            _list.Insert(0, d);
            if (_list.Count >= COUNT)
            {
                _list.RemoveAt(COUNT - 1);
            }
        }


        public List<HistoryData> GetData()
        {
            return _list;
        }


        public HistoryData GetDataByTime(string time)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (String.Equals(_list[i].AlertTime,time))
                    return _list[i];
            }
            return null;
        }
    }
}