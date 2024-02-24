using System.Collections.Generic;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class AISurvivorInventory : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform _itemHolder;

        Inventory.SLOT_TYPE m_currSlotType;

        Dictionary<Inventory.SLOT_TYPE, ItemSlot> m_itemSlotMap = new Dictionary<Inventory.SLOT_TYPE, ItemSlot>();

        public Item CurrentItem => m_itemSlotMap[m_currSlotType].GetItem();

        void Start()
        {
            m_currSlotType = Inventory.SLOT_TYPE.PRIMARY;

            // Init item slots
            int numSlots = System.Enum.GetNames(typeof( Inventory.SLOT_TYPE )).Length;
            for (int i = 0; i < numSlots; ++i)
            {
                Inventory.SLOT_TYPE slotType = (Inventory.SLOT_TYPE)i;
                m_itemSlotMap[slotType] = new ItemSlot();
            }
        }

        /*
         *  Adds an item into the inventory
         *  and sets it as the currently held item
         */
        public void AddItem(Item item)
        {
            Inventory.SLOT_TYPE slotType = item.SlotType;
            GameObject currItemModel = m_itemSlotMap[slotType].GetItemModel();
            if (currItemModel != null)
                Destroy( currItemModel );

            GameObject itemObj = Instantiate( item.ItemModelPrefab );
            itemObj.transform.SetParent( _itemHolder );
            itemObj.transform.localPosition = item.ItemLocalTransform.Position;
            itemObj.transform.localRotation = Quaternion.Euler( item.ItemLocalTransform.Rotation );
            itemObj.transform.localScale    = item.ItemLocalTransform.Scale;

            m_itemSlotMap[slotType].SetItemData( item.ItemModelPrefab, item );
            m_currSlotType = slotType;
        }

        public Item GetCurrentItem() => m_itemSlotMap[m_currSlotType].GetItem();

        public Item GetItem(Inventory.SLOT_TYPE slotType) => m_itemSlotMap[slotType].GetItem();
    }
}
