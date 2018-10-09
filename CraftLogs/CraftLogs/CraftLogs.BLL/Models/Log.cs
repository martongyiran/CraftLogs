using CraftLogs.BLL.Enums;
using System;

namespace CraftLogs.BLL.Models
{
    public class Log
    {
        public Log(LogTypeEnum logType, string text)
        {
            Date = DateTime.Now;
            LogType = logType;
            Text = text;
        }

        public DateTime Date { get; set; }
        public string Text { get; set; }
        public LogTypeEnum LogType { get; set; }
    }
}
