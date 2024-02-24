using UnityEngine;

namespace IntoTheHorde
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item Data/Gun Data/Full-Auto Gun Data")]
    public class FullAutoGunItem : BaseGunItem
    {
        public override void Init() => base.Init();

        public override void OnEquip() {}

        public override void OnUnequip() {}

        public override void OnUseBegin() => base.TryShoot();

        public override void OnUseHeld() => base.TryShoot();

        public override void OnUseReleased() {}
    }
}
