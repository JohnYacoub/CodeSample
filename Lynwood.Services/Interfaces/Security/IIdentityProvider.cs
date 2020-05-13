namespace Lynwood.Services
{
    public interface IIdentityProvider<T>
    {
        T GetCurrentUserId();
    }
}