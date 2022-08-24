namespace EdcentralizedNet.ViewModels
{
    public class AccountStatusVM
    {
        public string WalletAddress { get; set; }
        public bool IsLoaded { get; set; }
        public string Message { get; set; }

        public AccountStatusVM()
        {
            Message = string.Empty;
        }

        public AccountStatusVM(string walletAddress)
        {
            WalletAddress = walletAddress;
            Message = string.Empty;
        }
    }
}
