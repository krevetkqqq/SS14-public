using Content.Shared.Imperial.GaloshesMine.Components;
using Content.Shared.Inventory.Events;
using Content.Server.Explosion.EntitySystems;
using Content.Shared.Interaction.Components;
using Robust.Shared.Timing;
using Content.Server.Chat.Systems;

namespace Content.Server.Imperial.GaloshesMine.Systems;

public sealed partial class GaloshesMineSystems : EntitySystem
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    private int _elapsed = 0;
    private TimeSpan time;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GaloshesMineComponent, GotEquippedEvent>(OnEquip);
    }
    public override void Update(float frameTime)
    {
        if (!_gameTiming.IsFirstTimePredicted)
            return;
        var query = EntityQueryEnumerator<GaloshesMineComponent>();
        var curTime = _gameTiming.CurTime;

        while (query.MoveNext(out var uid, out var component))
        {
            if (!component.IsTimer)
                return;
            var i = 13;
            if (component.V2)
                i *= 2;
            if (_elapsed == 0)
                time = curTime;
            time += TimeSpan.FromSeconds(1);
            if (curTime <= time)
            {
                _chat.TrySendInGameICMessage(uid, $"Отсчет до смерти: {3 - _elapsed}", InGameICChatType.Speak, false);
                _elapsed++;
            }
            if (_elapsed == 2)
            {
                var sysMan = IoCManager.Resolve<IEntitySystemManager>();
                sysMan.GetEntitySystem<ExplosionSystem>().QueueExplosion(uid, "Default", i, i, i);
                _elapsed = 0;
            }
        }
    }

    private void OnEquip(EntityUid uid, GaloshesMineComponent comp, GotEquippedEvent ev)
    {
        EnsureComp<UnremoveableComponent>(uid);
        comp.IsTimer = true;
    }
}
