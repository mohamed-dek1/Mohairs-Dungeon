using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public static bool god_mode = false;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip itemPickup;
    public AudioClip playerHurt;
    public AudioClip playerDeath;
    public AudioClip enemyDeath;
    public AudioClip bombExplosion;
    public AudioClip doorOpen;


    void Start()
    {
        Screen.SetResolution(1024, 960, false);

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
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
