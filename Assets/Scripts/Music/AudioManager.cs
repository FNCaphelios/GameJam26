using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private Coroutine activeMusicRoutine;
    private AudioSource musicSource;

    [Header("Music Track")]
    public AudioClip musicClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        musicSource = GetComponent<AudioSource>();
        if(musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    public void PlayMusic(AudioClip clip, bool loop = false ,float volume = 1f)
    {
        if (clip == null || musicSource == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicClip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void FadeOutMusic(float duration = 1.5f)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    public void PlayMusicOnceWithFade(AudioClip clip, float fadeDuration = 1.5f, float volume = 1f)
    {
        if(clip == null || musicSource == null) return;

        if(activeMusicRoutine != null)
        {
            StopCoroutine(activeMusicRoutine);
        }
        activeMusicRoutine = StartCoroutine(PlayMusicOnceWithFadeRoutine(clip, 2f));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        if(musicSource == null) yield break;

        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    private IEnumerator PlayMusicOnceWithFadeRoutine(AudioClip clip, float duration, float maxVolume = 1f)
    {
        musicSource.clip = clip;
        musicSource.loop = false;
        musicSource.volume = 0f;
        musicSource.Play();

        // 1. FADE IN (Lautstärke von 0 auf maxVolume hochfahren)
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, maxVolume, timer / duration);
            yield return null;
        }
        musicSource.volume = maxVolume;
        // 2. WARTEN (Bis kurz vor Ende des Songs)
        float playDuration = clip.length - duration * 2;
        if (playDuration > 0)
        {
            yield return new WaitForSeconds(playDuration);
        }
        // 3. FADE OUT (Lautstärke von maxVolume auf 0 herunterfahren)
        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(maxVolume, 0f, timer / duration);
            yield return null;
        }
        musicSource.volume = 0f;
        musicSource.Stop();
        activeMusicRoutine = null;
    }
}