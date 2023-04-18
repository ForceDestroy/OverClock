namespace Server.BOModels
{
    public class EncryptedData
    {
        public string Data { get; set; }

        public EncryptedData(string data)
        {
            this.Data = data;
        }
    }
}
