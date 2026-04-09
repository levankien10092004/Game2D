using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Audio Clip")]
    [SerializeField] public AudioClip BackGround;
    [SerializeField] public AudioClip Coin;
    [SerializeField] public AudioClip Attack;
    [SerializeField] public AudioClip Jump;
    [SerializeField] public AudioClip Chose;
    [SerializeField] public AudioClip Ruong;
    [SerializeField] public AudioClip VicTory;
    [SerializeField] public AudioClip Lost;
    [SerializeField] public AudioClip Hurt;

    [Header("Music")]
    [SerializeField] public AudioClip MenuMusic;
    [SerializeField] public AudioClip GameMusic;




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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      if(scene.name == "MainMenu")
        {
            PlayMusic(MenuMusic);
        }
        else
        {
            PlayMusic(GameMusic); // hoặc StopMusic() nếu không muốn nhạc
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.Stop();
        musicSource.clip = clip;
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
    public void StopMusic()
    {
        musicSource.Stop();
    }

}
