using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Collider ))]
    public class PickupInteraction : Interactable
    {
        [SerializeField] Item     _item;
        [SerializeField] Material _glowMaterial;

        Material m_defaultMat;
        Renderer m_renderer;

        public Item GetItem() => _item;

        void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
            // TODO : Glow on mouse hover
            /*if (_glowMaterial == null)
                Debug.LogError("ChangeMaterialOnHover.cs : Missing glowMaterial reference on " + gameObject.name);*/

            // m_renderer = GetComponent<Renderer>();
            // m_defaultMat = m_renderer.material;
        }

        // void OnEnable() => m_renderer.material = m_defaultMat;

        // TODO : Glow on mouse hover
        /*void OnMouseOver() => m_renderer.material = _glowMaterial;

        void OnMouseExit() => m_renderer.material = m_defaultMat;*/

        public override void OnInteract(InteractionSystem interactionSystem)
        {
            Inventory inventory = interactionSystem.GetInventory();
            Item newItemInstance = Instantiate( _item );
            inventory.AddItem( newItemInstance );

            Destroy( this.gameObject );
        }
    }
}
