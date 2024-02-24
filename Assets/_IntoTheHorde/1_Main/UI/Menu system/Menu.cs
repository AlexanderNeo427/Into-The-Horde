using UnityEngine;

namespace IntoTheHorde
{
	[DisallowMultipleComponent]
	public class Menu : MonoBehaviour
	{
		[SerializeField] string _menuName;

		public string MenuName => _menuName;
	}
}

