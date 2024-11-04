namespace SatoshisMarketplace.Web.Models.User
{
    public class UserLogs
    {
        public string Username { get; set; }

        public int AllLogsCount { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public List<UserLog> Logs { get; set; }
    }
}
