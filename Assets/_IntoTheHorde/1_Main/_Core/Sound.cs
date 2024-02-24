using UnityEngine;

namespace IntoTheHorde
{
    [System.Serializable]
    public class Sound 
    {
        [SerializeField]                  string      _name         = "Default_sound_name_01";
        [SerializeField]                  AudioClip   _audioClip    = null;
        [SerializeField] [Range(0f, 1f)]  float       _volume       = 1f;
        [SerializeField] [Range(-3f, 3f)] float       _pitch        = 1f;
        [SerializeField]                  bool        _loop         = false;
        [SerializeField] [Range(0f, 1f)]  float       _spatialBlend = 0f;

        [HideInInspector] 
        public AudioSource Source;

        public string    Name         => _name;
        public AudioClip Clip         => _audioClip;
        public float     Volume       => _volume;
        public float     Pitch        => _pitch;
        public bool      Loop         => _loop;
        public float     SpatialBlend => _spatialBlend;

        public void Initialize()
        {
            Source.clip         = _audioClip;
            Source.volume       = _volume;
            Source.pitch        = _pitch;
            Source.loop         = _loop;
            Source.spatialBlend = _spatialBlend;
        }
    }
}
