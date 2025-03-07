public interface IUserContextService
{
    string? GetCurrentUserId();
    bool IsAdmin();
}