using UnityEngine;

namespace IntoTheHorde
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item Data/Throwable Data")]
    public class ThrowableItem : Item
    {
        [Header("Throwable Item Data")]
        [SerializeField] BaseThrowableBehavior _throwablePrefab;

        public override void Init() {}

        public override void OnEquip() {}

        public override void OnUseBegin()
        {
            // TODO : Animation of the grenade of whatever moving backwards
            //        like it's about to be thrown
        }

        public override void OnUseHeld() {}

        public override void OnUseReleased()
        {
            ThrowableUsedEventArgs args = new ThrowableUsedEventArgs( m_owner, _throwablePrefab );
            EventManager.RaiseEvent( GameEvent.OnUseThrowable, args );
        }

        public override void OnUnequip() {}
    }
}
