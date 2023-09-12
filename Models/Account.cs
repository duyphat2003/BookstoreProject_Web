namespace BookstoreProject.Models
{
    public class Account
    {
        private string account;
        private string password;
        private string role;


        public Account()
        {

        }

        public Account(string account, string password, string role)
        {
            this.account = account;
            this.password = password;
            this.role = role;
        }

        public string getAccount()
        {
            return account;
        }

        public void setAccount(string account)
        {
            this.account = account;
        }

        public string getPassword()
        {
            return password;
        }

        public void setPassword(string password)
        {
            this.password = password;
        }

        public string getRole()
        {
            return role;
        }

        public void setRole(string role)
        {
            this.role = role;
        }
    }
}
