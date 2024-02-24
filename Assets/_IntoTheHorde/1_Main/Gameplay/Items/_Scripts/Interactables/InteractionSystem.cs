using UnityEngine;

/*
 *  Attach this script to the player
 */
namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( InputReader ))] 
    public class InteractionSystem : MonoBehaviour
    {
        [Header ("References")]
        [SerializeField] Camera    _mainCam;
        [SerializeField] Inventory _inventory;
        [SerializeField] LayerMask _interactableLayer;

        [Header("Customisations")]
        [SerializeField] float _interactionRange = 2.5f;

        InputReader m_inputReader;

        void Awake() => m_inputReader = GetComponent<InputReader>();

        public Inventory GetInventory() => _inventory;

        void Update()
        {
            if (m_inputReader.InteractPressed)
            {
                Ray        ray     = new Ray( _mainCam.transform.position, _mainCam.transform.forward );
                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast( ray, out hitInfo, _interactionRange, _interactableLayer ))
                {
                    GameObject other = hitInfo.collider.gameObject;

                    Interactable interactable = other.GetComponent<Interactable>();
                    interactable?.OnInteract( this );
                }
            }
        }
    }
}
