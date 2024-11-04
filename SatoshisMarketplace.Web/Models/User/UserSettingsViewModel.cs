namespace SatoshisMarketplace.Web.Models.User
{
    public class UserSettingsViewModel
    {
        public string Username { get; set; }

        public List<Models.User.UserLog> Logs { get; set; }
    }
}
