using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Audio Clip")]
    public AudioClip BackGround;
    public AudioClip Coin;
    public AudioClip Attack;
    public AudioClip Jump;
    public AudioClip Chose;
    public AudioClip Ruong;
    public AudioClip VicTory;
    public AudioClip Lost;
    public AudioClip Hurt;

    private void Awake()
    {
        // Singleton
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolume(); // 🔥 LOAD VOLUME NGAY
    }

    private void Start()
    {
        LoadVolume();
        musicSource.clip = BackGround;
        musicSource.loop = true;
        musicSource.Play();
    }

    void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat("musicvolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("sfxvolume", 1f);

        mixer.SetFloat("Music", Mathf.Log10(musicVol) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(sfxVol) * 20);

    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void StopSFX()
    {
        sfxSource.Stop();
    }

}
