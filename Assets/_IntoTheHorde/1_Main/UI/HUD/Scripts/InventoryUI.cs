using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] Inventory _inventory;
        [Space( 1 )]
        [SerializeField] Image _primarySlot;
        [SerializeField] Image _secondarySlot;
        [SerializeField] Image _throwableSlot;
        [SerializeField] Image _medkitSlot;
        [SerializeField] Image _consumableSlot;
        [Space( 1 )]
        [SerializeField] Sprite _primaryDefaultSprite;
        [SerializeField] Sprite _secondaryDefaultSprite;
        [SerializeField] Sprite _throwableDefaultSprite;
        [SerializeField] Sprite _medkitDefaultSprite;
        [SerializeField] Sprite _consumableDefaultSprite;
        [Header("Customisations")]
        [SerializeField] Color _selectedItemColor   = Color.green;
        [SerializeField] Color _unselectedItemColor = Color.gray;
        [SerializeField] Color _missingItemColor    = new Color( 190f, 190f, 190f, 80f );

        Dictionary<Inventory.SLOT_TYPE, Image> m_slotTypeImageMap;

        PrimarySlotUI   m_primarySlotUI;
        SecondarySlotUI m_secondarySlotUI;
        int             m_numSlotTypes;

        void Awake()
        {
            m_numSlotTypes = Enum.GetNames(typeof( Inventory.SLOT_TYPE )).Length;

            m_slotTypeImageMap = new Dictionary<Inventory.SLOT_TYPE, Image>()
            {
                { Inventory.SLOT_TYPE.PRIMARY,    _primarySlot    },
                { Inventory.SLOT_TYPE.SECONDARY,  _secondarySlot  },
                { Inventory.SLOT_TYPE.THROWABLE,  _throwableSlot  },
                { Inventory.SLOT_TYPE.MEDKIT,     _medkitSlot     },
                { Inventory.SLOT_TYPE.CONSUMABLE, _consumableSlot },
            };

            _primarySlot.sprite    = _primaryDefaultSprite;
            _secondarySlot.sprite  = _secondaryDefaultSprite;
            _throwableSlot.sprite  = _throwableDefaultSprite;
            _medkitSlot.sprite     = _medkitDefaultSprite;
            _consumableSlot.sprite = _consumableDefaultSprite;

            _primarySlot.color    = _missingItemColor;
            _secondarySlot.color  = _missingItemColor;
            _throwableSlot.color  = _missingItemColor;
            _medkitSlot.color     = _missingItemColor;
            _consumableSlot.color = _missingItemColor;

            m_primarySlotUI = _primarySlot.GetComponent<PrimarySlotUI>();
            if (m_primarySlotUI == null)
                Debug.LogError("InventoryUI.cs : unable to find PrimarySlotUI component");

            m_secondarySlotUI = _secondarySlot.GetComponent<SecondarySlotUI>();
            if (m_secondarySlotUI == null)
                Debug.LogError("InventoryUI.cs : unable to find SecondarySlotUI component");
        }

        void Start()
        {
            UpdatePrimarySlot();
            UpdateSecondarySlot();
            UpdateThrowableSlot();
            UpdateMedkitSlot();
            UpdateConsumableSlot();

            ItemSlot currItemSlot = _inventory.CurrentItemSlot();

            if (currItemSlot.GetItem() != null)
                m_slotTypeImageMap[_inventory.CurrentSlotType].color = _selectedItemColor;
        }

        void OnEnable() => EventManager.AddListener( GameEvent.OnInventoryChanged, UpdateInventoryUI );

        void OnDisable() => EventManager.RemoveListener( GameEvent.OnInventoryChanged, UpdateInventoryUI );

        void UpdateInventoryUI(EventArgs eventArgs)
        {
            UpdatePrimarySlot();
            UpdateSecondarySlot();
            UpdateThrowableSlot();
            UpdateMedkitSlot();
            UpdateConsumableSlot();

            ItemSlot currItemSlot = _inventory.CurrentItemSlot();

            if (currItemSlot.GetItem() != null)
                m_slotTypeImageMap[_inventory.CurrentSlotType].color = _selectedItemColor;
        }

        void UpdatePrimarySlot()
        {
            Item primaryItem = _inventory.PrimarySlot.GetItem();
            if (primaryItem != null)
            {
                _primarySlot.sprite = primaryItem.ItemSprite;
                _primarySlot.color = _unselectedItemColor;

                BaseGunItem gunItem = primaryItem as BaseGunItem;
                if (gunItem != null)
                {
                    int bulletsLeftInMag = gunItem.GetNumBullets();
                    int reserveAmmo = gunItem.GetMagazineSize() * gunItem.GetNumMags();
                    m_primarySlotUI.SetInfo( bulletsLeftInMag, reserveAmmo );
                }
            }
            else
            {
                _primarySlot.sprite = _primaryDefaultSprite;
                _primarySlot.color = _missingItemColor;
                m_primarySlotUI.TurnOffText();
            }
        }

        void UpdateSecondarySlot()
        {
            Item secondaryItem = _inventory.SecondarySlot.GetItem();
            if (secondaryItem != null)
            {
                _secondarySlot.sprite = secondaryItem.ItemSprite;
                _secondarySlot.color  = _unselectedItemColor;

                BaseGunItem gunItem = secondaryItem as BaseGunItem;
                if (gunItem != null)
                {
                    m_secondarySlotUI.SetNumBullets(gunItem.GetNumBullets());
                }
            }
            else
            {
                _secondarySlot.sprite = _secondaryDefaultSprite;
                _secondarySlot.color  = _missingItemColor;
                m_secondarySlotUI.TurnOffText();
            }
        }

        void UpdateThrowableSlot()
        {
            Item throwableItem = _inventory.ThrowableSlot.GetItem();
            if (throwableItem != null)
            {
                _throwableSlot.sprite = throwableItem.ItemSprite;
                _throwableSlot.color  = _unselectedItemColor;
            }
            else
            {
                _throwableSlot.sprite = _throwableDefaultSprite;
                _throwableSlot.color  = _missingItemColor;
            }
        }

        void UpdateMedkitSlot()
        {
            Item medkitItem = _inventory.MedkitSlot.GetItem();
            if (medkitItem != null)
            {
                _medkitSlot.sprite = medkitItem.ItemSprite;
                _medkitSlot.color  = _unselectedItemColor;
            }
            else
            {
                _medkitSlot.sprite = _medkitDefaultSprite;
                _medkitSlot.color  = _missingItemColor;
            }
        }

        void UpdateConsumableSlot()
        {
            Item consumableItem = _inventory.ConsumableSlot.GetItem();
            if (consumableItem != null)
            {
                _consumableSlot.sprite = consumableItem.ItemSprite;
                _consumableSlot.color  = _unselectedItemColor;
            }
            else
            {
                _consumableSlot.sprite = _consumableDefaultSprite;
                _consumableSlot.color  = _missingItemColor;
            }
        }
    }
}
