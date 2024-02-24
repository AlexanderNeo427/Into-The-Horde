using UnityEngine;
using Trisibo;

namespace IntoTheHorde
{
    [System.Serializable]
    public class Map 
    {
        [SerializeField] string     _mapName;
        [SerializeField] SceneField _firstChapter;

        public string     MapName      => _mapName;
        public SceneField FirstChapter => _firstChapter;
    }
}
