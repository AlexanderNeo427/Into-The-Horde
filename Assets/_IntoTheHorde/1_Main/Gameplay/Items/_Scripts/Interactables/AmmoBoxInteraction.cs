namespace IntoTheHorde
{
    public class AmmoBoxInteraction : Interactable
    {
        public override void OnInteract(InteractionSystem interactionSystem)
        {
            interactionSystem.GetInventory().OnPickupAmmo();
        }
    }
}
