using System;
using UnityEngine;

namespace IntoTheHorde
{
    [DisallowMultipleComponent]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] Sound[] _soundList;

        void Start()
        {
            foreach (Sound sound in _soundList)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                sound.Initialize();
            }
        }

        public void Play(string audioName)
        {
            Sound s = Array.Find(_soundList, sound => sound.Name == audioName);
            if (s.Source == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.Source.Play();
        }
    }
}
