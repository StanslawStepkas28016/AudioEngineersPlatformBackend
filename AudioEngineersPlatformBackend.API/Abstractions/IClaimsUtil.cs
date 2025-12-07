namespace API.Abstractions;

public interface IClaimsUtil
{
    public Task<Guid> ExtractIdUserFromClaims();
    public Task<string> ExtractRoleNameFromClaims();
}