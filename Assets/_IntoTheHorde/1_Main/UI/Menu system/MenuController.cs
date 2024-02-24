using UnityEngine;

/*
 *  Contains references to all Menus that
 *  you want to have access to
 */
namespace IntoTheHorde
{
	[DisallowMultipleComponent]
	public class MenuController : MonoBehaviour
	{
		[SerializeField] Menu[] _menuList;
		[SerializeField] Menu   _startMenu;

		[SerializeField] bool _openStartMenuOnPlay = true;

		void Start()
		{
			EventManager.RaiseEvent(GameEvent.OnEnterMainMenu, new EnterMainMenuEventArgs());

			if (_openStartMenuOnPlay)
				OpenMenu( _startMenu.MenuName );

			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible	 = true;
		}

		public void OpenMenu( string menuName ) 
		{
			foreach (Menu menu in _menuList)
			{
				bool active = menu.MenuName == menuName;
				menu.gameObject.SetActive( active );
			}
		}

		public void OpenMenu( Menu menu ) => OpenMenu( menu.MenuName );
	}
}

