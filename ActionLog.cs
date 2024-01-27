using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public class ActionLogEntry
    {
        private string _key;
        private int _count;

        public string Key => _key;
        public int Count => _count;


        public ActionLogEntry(string key, int count)
        {
            _key = key;
            _count = count;
        }

        public void IncrementCount()
        {
            _count++;
        }
    }


    public class ActionLog
    {
        private string _logString = string.Empty;
        private string _archive = string.Empty;

        public List<ActionLogEntry> Logs { get; private set; }

        public ActionLog()
        {
            Logs = new List<ActionLogEntry>();
        }


        public string LogString => _logString;

        public void AddLog(string log)
        {
            if(Logs.Count == 0)
            {
                Logs.Add(new ActionLogEntry(log, 1));
                _logString = log + "\n";
                return;
            }

            var last = Logs[^1];
            if (last.Key == log)
            {
                last.IncrementCount();
            }
            else
            {
                Logs.Add(new ActionLogEntry(log, 1));
                _logString += log + "\n";
            }

            if(Logs.Count > 5)
            {
                var first = Logs[0];
                var suffix = first.Count > 1 ? $" (x{first.Count})\n" : "\n";
                _archive += first.Key + suffix;
                Logs.RemoveAt(0);
            }
            _logString = "";
            for (int i = 0; i < Logs.Count; i++)
            {
                var suffix = Logs[i].Count > 1 ? $" (x{Logs[i].Count})\n" : "\n";
                _logString += Logs[i].Key + suffix;
            }
        }

        public void PrintAllLogs()
        {
            foreach(var log in Logs)
            {
                System.Console.WriteLine(log.Key);
            }
        }

        public void ClearLogs()
        {
            Logs.Clear();
        }
    }
}
