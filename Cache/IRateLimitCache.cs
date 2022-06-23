namespace EdcentralizedNet.Cache
{
    public interface IRateLimitCache
    {
        bool CanRequestOpensea();
        bool CanRequestEtherscan();
    }
}
