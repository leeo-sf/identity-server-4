using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;

namespace AuthServer.configs;

internal class CustomProfileService : IProfileService
{
    private readonly TestUserStore _user;

    public CustomProfileService(TestUserStore user)
    {
        _user = user;
    }

    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = _user.FindBySubjectId(context.Subject.GetSubjectId());

        if (user is not null)
        {
            var claims = user.Claims.Where(
                claim => context.RequestedClaimTypes.Contains(claim.Type))
                .ToList();

            context.IssuedClaims = claims;
        }

        return Task.CompletedTask;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        var user = _user.FindBySubjectId(context.Subject.GetSubjectId());
        context.IsActive = user != null;
        return Task.CompletedTask;
    }
}
