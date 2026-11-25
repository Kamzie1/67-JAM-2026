using UnityEngine;
using UnityEngine.Serialization;

namespace YANG
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip music;

       private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlaySound(music, transform, 0.1f, true);
        }
        public void PlaySound(AudioClip clip, Transform spawnTransform, float volume, bool loop = false)
        {
           if (clip == null) return;

            AudioSource audio = Instantiate(audioSource, spawnTransform.position, Quaternion.identity, transform);
            audio.clip = clip;
            audio.volume = volume;
            audio.loop = loop;
            audio.Play();

            if (!loop)
            {
                Destroy(audio.gameObject, clip.length);
            }
        }
    }
}