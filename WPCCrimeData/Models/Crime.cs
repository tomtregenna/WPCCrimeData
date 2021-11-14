namespace WPCCrimeData.Models
{
    public class Crime
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Month { get; set; }
        public string Context { get; set; }
        public string Location_Type { get; set; }
        public string Location_Subtype { get; set; }
        //public string Outcome_Status { get; set; }

        public CrimeCategory CrimeCategory { get; set; }
        public Location Location { get; set; }
    }
}
