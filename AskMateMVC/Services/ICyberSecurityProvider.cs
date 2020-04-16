namespace AskMateMVC.Services
{
    public interface ICyberSecurityProvider
    {
        public string EncryptPassword(string password);
        public bool IsValidUser(string username, string password);
    }
}