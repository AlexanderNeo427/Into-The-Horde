using UnityEngine;

/*
 *  Base class for all item scriptable Objects to derive from
 */
namespace IntoTheHorde
{
    public abstract class Item : ScriptableObject
    {
        [Header("Item Data")]
        [SerializeField] protected string              _itemName;
        [SerializeField] protected Inventory.SLOT_TYPE _slotType;
        [SerializeField] protected GameObject          _itemModelPrefab;
        [SerializeField] protected GameObject          _itemDropPrefab;
        [SerializeField] protected Sprite              _itemSprite;
        [SerializeField] protected TransformInfo       _itemLocalTransform;

        protected GameActor m_owner;

        public void SetOwner(GameActor owner) => m_owner = owner;

        public abstract void Init();    // Pseudo constructor
        public abstract void OnEquip();
        public abstract void OnUseBegin();
        public abstract void OnUseHeld();
        public abstract void OnUseReleased();
        public abstract void OnUnequip();

        public string              ItemName           => _itemName;
        public Inventory.SLOT_TYPE SlotType           => _slotType;
        public GameObject          ItemModelPrefab    => _itemModelPrefab;
        public GameObject          ItemDropPrefab     => _itemDropPrefab;
        public Sprite              ItemSprite         => _itemSprite;
        public TransformInfo       ItemLocalTransform => _itemLocalTransform;
    }
}
