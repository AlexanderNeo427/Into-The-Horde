using UnityEngine;

/*
 *  Base class for interactable items to derive from
 */
namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof( Collider ))]
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void OnInteract(InteractionSystem interactionSystem);
    }
}
