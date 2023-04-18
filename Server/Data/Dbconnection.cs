namespace Server.Data
{
    public class Dbconnection
    {
        public string ConnectionString { get; set; }

        public Dbconnection(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
