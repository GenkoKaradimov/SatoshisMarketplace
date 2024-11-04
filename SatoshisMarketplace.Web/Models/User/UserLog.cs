namespace SatoshisMarketplace.Web.Models.User
{
    public class UserLog
    {
        public DateTime Timestamp { get; set; }

        public string IP { get; set; }

        public string LogType { get; set; }
    }
}
