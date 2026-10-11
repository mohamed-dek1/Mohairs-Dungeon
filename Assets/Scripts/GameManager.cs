using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool god_mode = false;
    
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] float musicVolume = 0.25f;
    [SerializeField] float SFXVolume = 0.1f;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip itemPickup;
    public AudioClip playerHurt;
    public AudioClip playerDeath;
    public AudioClip enemyDeath;
    public AudioClip bombExplosion;
    public AudioClip doorOpen;
    public AudioClip secretFound;


    void Start()
    {
        Screen.SetResolution(1024, 960, false);

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        setMusicVolume(musicVolume);
        setSFXVolume(SFXVolume);
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    public void setMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void setSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
