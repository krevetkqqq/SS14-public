namespace Content.Shared.IdentityManagement.Components;

public interface IIdentityInterface
{
    public string GetIdentityName(EntityUid target, EntityUid? examiner);
}
