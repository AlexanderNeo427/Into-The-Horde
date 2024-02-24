using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class MapLoader : MonoBehaviour
    {
        [SerializeField] Map[] _maps;

        public void LoadMap(string mapName)
        {
            foreach (Map map in _maps)
            {
                if (map.MapName == mapName)
                    SceneManager.LoadScene( map.FirstChapter.BuildIndex );
            }
        }
    }
}
