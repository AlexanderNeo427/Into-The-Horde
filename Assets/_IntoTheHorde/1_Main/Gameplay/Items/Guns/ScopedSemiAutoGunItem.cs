using UnityEngine;

namespace IntoTheHorde
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item Data/Gun Data/Scoped Semi-Auto Gun Data")]
    public class ScopedSemiAutoGunItem : SemiAutoGunItem, IScopeable
    {
        [Header("Scoped Semi-Auto Gun Data")]
        [SerializeField] float  _scopedFOV = 12f;
        [SerializeField] Sprite _scopeSprite;

        public override void OnUnequip() => EventManager.RaiseEvent(GameEvent.OnGunUnscope, new GunUnscopeEventArgs());

        public void OnScope() => EventManager.RaiseEvent(GameEvent.OnGunScope, new GunScopeEventArgs(_scopedFOV, _scopeSprite));

        public void OnUnscope() => EventManager.RaiseEvent(GameEvent.OnGunUnscope, new GunUnscopeEventArgs());
    }
}
