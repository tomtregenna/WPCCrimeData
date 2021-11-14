using System;
using System.Collections.Generic;
using WPCCrimeData.Models;

namespace WPCCrimeData.ViewModels
{
    public class HomeViewModel
    {
        public bool ShowSplash = true;
        public List<string> Errors;

        public List<Crime> Crimes { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<MonthData> DateList;

        public string LastUpdatedStr => LastUpdated.ToLongDateString();
    }
}
