namespace IntoTheHorde
{
	public static class Constants
	{
		public static class Zombie
		{
			public static class Regular
			{
				public static class Locomotion
                {
					public const int Idle	= 0;
					public const int Walk   = 1;
					public const int Run	= 2;
					public const int Sprint = 3;
				}

				public static class AnimParam
				{
					public const string Speed    = "f_Speed";
					public const string Death_1  = "t_Death_1";
					public const string Death_2  = "t_Death_2";
					public const string Attack1	 = "b_isAttacking";
				}

                public static class AnimLayer
                {
					public const int BaseLayer		= 0;
					public const int UpperBodyLayer = 1;
				}
			}

			public static class Boomer 
			{
				public static class AnimParam {}
				public static class AnimLayer {}
			}

			public static class Spitter 
			{
				public static class AnimParam {}
				public static class AnimLayer {}
			}

			public static class Witch 
			{
				public static class AnimParam {}
				public static class AnimLayer {}
			}

			public static class Tank 
			{
				public static class AnimParam {}
				public static class AnimLayer {}
			}
		}

		public static class Survivor
		{
			public static class AnimParam {}
			public static class AnimLayer {}
		}

		public static class Input
		{
			public const string Horizontal  = "Horizontal";
			public const string Vertical    = "Vertical";
			public const string Sprint	    = "Sprint";
			public const string Jump	    = "Jump";
										    
			public const string MouseX	    = "Mouse X";
			public const string MouseY	    = "Mouse Y";
										    
			public const string Interact    = "Interact";
			public const string FlashLight  = "Flashlight";
			public const string Fire1	    = "Fire1";
			public const string Fire2	    = "Fire2";
			public const string Reload	    = "Reload";
										    
			public const string ItemSlot1   = "ItemSlot1";
			public const string ItemSlot2   = "ItemSlot2";
			public const string ItemSlot3   = "ItemSlot3";
			public const string ItemSlot4   = "ItemSlot4";
			public const string ItemSlot5   = "ItemSlot5";

			public const string MouseScroll = "Mouse ScrollWheel";
		}
	}
}