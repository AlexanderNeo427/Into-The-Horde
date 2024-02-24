using UnityEngine;

/*		
 *	Just attach this script to whatever gameObject needs to read input
 */
namespace IntoTheHorde
{
	[DisallowMultipleComponent]
    public class InputReader : MonoBehaviour
    {
		public Vector2 MoveInput
        {
			get
            {
				Vector2 movement = Vector2.zero;
				movement.x = Input.GetAxis( Constants.Input.Horizontal );
				movement.y = Input.GetAxis( Constants.Input.Vertical );
				return movement;
			}
        }

		public Vector2 LookInput
        {
			get
			{
				Vector2 lookDelta = Vector2.zero;
				lookDelta.x = Input.GetAxis( Constants.Input.MouseX );
				lookDelta.y = Input.GetAxis( Constants.Input.MouseY );
				return lookDelta;
			}
		}

        public bool JumpPressed  => Input.GetButtonDown( Constants.Input.Jump );
		public bool JumpHeld	 => Input.GetButton( Constants.Input.Jump );
		public bool JumpReleased => Input.GetButtonUp( Constants.Input.Jump );

		public bool SprintPressed  => Input.GetButtonDown( Constants.Input.Sprint );
		public bool SprintHeld	   => Input.GetButton( Constants.Input.Sprint );
		public bool SprintReleased => Input.GetButtonUp( Constants.Input.Sprint );

		public bool InteractPressed  => Input.GetButtonDown( Constants.Input.Interact );
		public bool InteractHeld	 => Input.GetButton( Constants.Input.Interact);
		public bool InteractReleased => Input.GetButtonUp( Constants.Input.Interact );

		public bool Fire1Pressed  => Input.GetButtonDown( Constants.Input.Fire1 );
		public bool Fire1Held	  => Input.GetButton( Constants.Input.Fire1 );
		public bool Fire1Released => Input.GetButtonUp( Constants.Input.Fire1 );

		public bool Fire2Pressed  => Input.GetButtonDown( Constants.Input.Fire2 );
		public bool Fire2Held	  => Input.GetButton( Constants.Input.Fire2 );
		public bool Fire2Released => Input.GetButtonUp( Constants.Input.Fire2 );

		public bool FlashlightPressed  => Input.GetButtonDown( Constants.Input.FlashLight );
		public bool FlashlightHeld	   => Input.GetButton( Constants.Input.FlashLight );
		public bool FlashlightReleased => Input.GetButtonUp( Constants.Input.FlashLight );

		public bool ReloadPressed  => Input.GetButtonDown( Constants.Input.Reload );
		public bool ReloadHeld	   => Input.GetButton( Constants.Input.Reload );
		public bool ReloadReleased => Input.GetButtonUp( Constants.Input.Reload );

		public bool ItemSlot1Pressed  => Input.GetButtonDown( Constants.Input.ItemSlot1 );
		public bool ItemSlot1Held	  => Input.GetButton( Constants.Input.ItemSlot1 );
		public bool ItemSlot1Released => Input.GetButtonUp( Constants.Input.ItemSlot1 );

		public bool ItemSlot2Pressed  => Input.GetButtonDown( Constants.Input.ItemSlot2 );
		public bool ItemSlot2Held	  => Input.GetButton( Constants.Input.ItemSlot2 );
		public bool ItemSlot2Released => Input.GetButtonUp( Constants.Input.ItemSlot2 );

		public bool ItemSlot3Pressed  => Input.GetButtonDown( Constants.Input.ItemSlot3 );
		public bool ItemSlot3Held	  => Input.GetButton( Constants.Input.ItemSlot3 );
		public bool ItemSlot3Released => Input.GetButtonUp( Constants.Input.ItemSlot3 );

		public bool ItemSlot4Pressed  => Input.GetButtonDown( Constants.Input.ItemSlot4 );
		public bool ItemSlot4Held	  => Input.GetButton( Constants.Input.ItemSlot4 );
		public bool ItemSlot4Released => Input.GetButtonUp( Constants.Input.ItemSlot4 );

		public bool ItemSlot5Pressed  => Input.GetButtonDown( Constants.Input.ItemSlot5 );
		public bool ItemSlot5Held	  => Input.GetButton( Constants.Input.ItemSlot5 );
		public bool ItemSlot5Released => Input.GetButtonUp( Constants.Input.ItemSlot5 );

		public bool SelectNextWeapon => Input.GetAxis( Constants.Input.MouseScroll ) > 0f;
		public bool SelectPrevWeapon => Input.GetAxis( Constants.Input.MouseScroll ) < 0f;
	}
}
