using System;
using UnityEngine;

/*
 *  Attach this script to the player
 */
namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( GameActor ), typeof( Inventory ))]
    public class GunSpreadManager : MonoBehaviour
    {
        [SerializeField] CrosshairSpread _crosshairSpread;

        Camera    m_mainCam;
        GameActor m_actor;
        Inventory m_inventory;

        // Spread in degrees
        public float Spread { get; private set; }

        bool m_gunIsScoped     = false;
        bool m_incrementSpread = false;

        void Awake()
        {
            m_mainCam   = Camera.main;
            m_actor     = GetComponent<GameActor>();
            m_inventory = GetComponent<Inventory>();
        }

        void OnEnable()
        {
            EventManager.AddListener(GameEvent.OnGunFired,   OnShootHandler);
            EventManager.AddListener(GameEvent.OnGunScope,   OnGunScope);
            EventManager.AddListener(GameEvent.OnGunUnscope, OnGunUnscope);
        }

        void OnDisable()
        {
            EventManager.RemoveListener(GameEvent.OnGunFired,   OnShootHandler);
            EventManager.RemoveListener(GameEvent.OnGunScope,   OnGunScope);
            EventManager.RemoveListener(GameEvent.OnGunUnscope, OnGunUnscope);
        }

        void Update()
        {
            if (m_inventory.CurrentSlotType == Inventory.SLOT_TYPE.PRIMARY ||
                m_inventory.CurrentSlotType == Inventory.SLOT_TYPE.SECONDARY)
            {
                Item item = m_inventory.CurrentItemSlot().GetItem();
                if (item == null) return;

                BaseGunItem gunItem = item as BaseGunItem;
                if (gunItem == null) return;

                Spread -= gunItem._spreadDecayRate * Time.deltaTime;
                Spread = Mathf.Clamp( Spread, gunItem._minGunSpread, gunItem._maxGunSpread );

                Vector3 pos = m_mainCam.transform.position;
                Vector3 dir = Vector3.RotateTowards(m_mainCam.transform.forward, 
                                                    m_mainCam.transform.right, 
                                                    Mathf.Deg2Rad * Spread, 0.0f);

                float crosshairSpread = _crosshairSpread.GetCrosshairSpread( pos, dir, gunItem._gunRange ) * 1.2f;
                _crosshairSpread.SetCrosshairSpread( crosshairSpread );

                if (m_incrementSpread)
                {
                    Spread = gunItem._maxGunSpread;
                    m_incrementSpread = false;
                }

                if (m_gunIsScoped)
                    Spread = 0f;
            }
            else
            {
                _crosshairSpread.SetToDefaultSpread();
            }
        }

        void OnShootHandler(EventArgs eventArgs)
        {
            GunFiredEventArgs args = eventArgs as GunFiredEventArgs;

            if (args == null)                 return;
            if (args.Shooter != this.m_actor) return;

            float randX = UnityEngine.Random.Range(-Spread, Spread);
            float randY = UnityEngine.Random.Range(-Spread, Spread);
            float randZ = UnityEngine.Random.Range(-Spread, Spread);

            Vector3 pos = m_mainCam.transform.position;
            Vector3 dir = Quaternion.Euler( randX, randY, randZ ) * m_mainCam.transform.forward;

            _crosshairSpread.SetCrosshairSpread( pos, dir, args.GunRange );
            m_incrementSpread = true;
        }

        void OnGunScope(EventArgs args) => m_gunIsScoped = true;

        void OnGunUnscope(EventArgs args) => m_gunIsScoped = false;
    }
}
