using UnityEngine;

namespace IntoTheHorde
{
    public class ItemSlot 
    {
        GameObject m_itemModel;
        Item       m_item;

        public GameObject GetItemModel() => m_itemModel;
        public Item       GetItem()      => m_item;

        public ItemSlot()
        {
            m_itemModel = null;
            m_item      = null;
        }

        public void SetItemData(GameObject itemModel, Item item)
        {
            m_itemModel = itemModel;
            m_item      = item;
        }
    }
}
