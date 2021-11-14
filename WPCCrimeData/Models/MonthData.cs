using System;

namespace WPCCrimeData.Models
{
    public class MonthData
    {
        public string Text;
        public DateTime Date;

        public MonthData(string text, DateTime date)
        {
            Text = text;
            Date = date;
        }
    }
}
