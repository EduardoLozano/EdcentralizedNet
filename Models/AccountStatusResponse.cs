namespace EdcentralizedNet.Models
{
    public class AccountStatusResponse
    {
        public string WalletAddress { get; set; }
        public bool IsLoaded { get; set; }
        public string Message { get; set; }

        public AccountStatusResponse() 
        {
            Message = string.Empty;
        }

        public AccountStatusResponse(string walletAddress)
        {
            WalletAddress = walletAddress;
            Message = string.Empty;
        }
    }
}
