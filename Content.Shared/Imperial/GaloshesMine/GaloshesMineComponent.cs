namespace Content.Shared.Imperial.GaloshesMine.Components;

[RegisterComponent]
public sealed partial class GaloshesMineComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool V2 = false;
    public bool IsTimer = false;
}
