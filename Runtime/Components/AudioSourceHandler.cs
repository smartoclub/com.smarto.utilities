namespace Smarto.Components
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Keeps a list of audio clips and generates the corresponding audio sources at runtime.
    /// Inspired by this Brackey video: https://youtu.be/6OT43pvUyfY
    /// </summary>
    public class AudioSourceHandler : MonoBehaviour
    {
        [SerializeField] private float masterVolume = 1.0f; // The default volume of every audio source.
        [SerializeField] private SoundEffect[] soundEffects = default;
        
        private void Awake()
        {
            foreach(SoundEffect sound in soundEffects)
            {
                sound.AudioSource = gameObject.AddComponent<AudioSource>();
                sound.AudioSource.clip = sound.AudioClip;
                sound.AudioSource.volume = masterVolume;
                sound.AudioSource.playOnAwake = false;
            }
        }

        public void PlayAudioByName(string name)
        {
            SoundEffect sound = Array.Find(soundEffects, targetSound => targetSound.Name == name);

            if (sound != null)
            {
                sound.AudioSource.Play();
            }
            else
            {
                Debug.LogWarning($"[AudioSourceHandler] Could not find audio clip with name '{name}'.");
            }
        }
    }

    [Serializable]
    public class SoundEffect
    {
        public string Name;
        public AudioClip AudioClip;
        public float Volume;
        [Tooltip("Useful for filtering through sound effects for volume control options.")]
        public string Tag;

        [HideInInspector]
        public AudioSource AudioSource;
    }
}