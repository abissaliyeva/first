using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Mixer and snapshots")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerSnapshot normalSnapshot;
    [SerializeField] private AudioMixerSnapshot pausedSnapshot;

    [Header("AudioSources (children of this root object)")]
    [SerializeField] private AudioSource musicA;
    [SerializeField] private AudioSource musicB;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    [Header("Gameplay sound effects")]
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip land;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip heal;
    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip enemyWindup;
    [SerializeField] private AudioClip enemyFire;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip win;

    private AudioSource activeMusic;
    private Coroutine crossfadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 

        activeMusic = musicA;

        if (mixer != null)
            mixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
    }

    private void Start()
    {
        ApplySavedVolumes();
        SetPaused(false, 0f);
    }

    public void ApplySavedVolumes()
    {
        float music = 1f;
        float sfx = 1f;

        if (SaveManager.Instance != null &&
            SaveManager.Instance.Data != null)
        {
            music = SaveManager.Instance.Data.musicVolume;
            sfx = SaveManager.Instance.Data.sfxVolume;
        }

        ApplyMixerVolume("MusicVol", music);
        ApplyMixerVolume("SFXVol", sfx);
    }

    private void ApplyMixerVolume(string parameter, float linearVolume)
    {
        if (mixer == null)
        {
            Debug.LogError("GameAudioManager: GameMix is not assigned.");
            return;
        }

        float decibels =
            20f * Mathf.Log10(Mathf.Max(0.0001f, Mathf.Clamp01(linearVolume)));

        if (!mixer.SetFloat(parameter, decibels))
        {
            Debug.LogError(
                "GameAudioManager: Missing exposed mixer parameter: " + parameter
            );
        }
    }

    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp01(value);
        ApplyMixerVolume("MusicVol", value);

        if (SaveManager.Instance != null)
            SaveManager.Instance.SetMusicVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        value = Mathf.Clamp01(value);
        ApplyMixerVolume("SFXVol", value);

        if (SaveManager.Instance != null)
            SaveManager.Instance.SetSfxVolume(value);
    }

    public void SetPaused(bool paused, float transitionSeconds = 0.3f)
    {
        AudioMixerSnapshot snapshot =
            paused ? pausedSnapshot : normalSnapshot;

        if (snapshot == null)
        {
            Debug.LogWarning("GameAudioManager: AudioMixer snapshot missing.");
            return;
        }

        snapshot.TransitionTo(transitionSeconds);
    }

    public void PlayMenuMusic() => PlayMusic(menuMusic);
    public void PlayGameMusic() => PlayMusic(gameMusic);

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicA == null || musicB == null)
        {
            Debug.LogError("GameAudioManager: Music clip/sources not assigned.");
            return;
        }

        SetPaused(false, 0.3f);

        if (activeMusic != null &&
            activeMusic.isPlaying &&
            activeMusic.clip == clip)
        {
            return;
        }

        if (crossfadeRoutine != null)
        {
            StopCoroutine(crossfadeRoutine);
            crossfadeRoutine = null;
        }

        AudioSource previous = activeMusic;
        AudioSource next = previous == musicA ? musicB : musicA;

        next.Stop();
        next.clip = clip;
        next.volume = 0f;
        next.loop = true;
        next.Play();

        crossfadeRoutine = StartCoroutine(
            Crossfade(previous, next, 1f)
        );

        activeMusic = next;
    }

    private IEnumerator Crossfade(AudioSource previous, AudioSource next, float seconds)
    {
        float startVolume = previous != null && previous.isPlaying
            ? previous.volume : 0f;

        float elapsed = 0f;

        while (elapsed < seconds)
        {
            elapsed += Time.unscaledDeltaTime; 
            float t = Mathf.Clamp01(elapsed / seconds);

            if (previous != null && previous != next)
                previous.volume = Mathf.Lerp(startVolume, 0f, t);

            next.volume = t;
            yield return null;
        }

        if (previous != null && previous != next)
        {
            previous.Stop();
            previous.volume = 0f;
        }

        next.volume = 1f;
        crossfadeRoutine = null;
    }

    private void PlayEffect(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("GameAudioManager: Audio clip not assigned.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogError("GameAudioManager: SFX AudioSource not assigned.");
            return;
        }

        sfxSource.pitch = Random.Range(0.92f, 1.08f);
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayJump() => PlayEffect(jump);
    public void PlayLand() => PlayEffect(land, 0.6f);
    public void PlayHit() => PlayEffect(hit);
    public void PlayHeal() => PlayEffect(heal);
    public void PlayPickup() => PlayEffect(coin);
    public void PlayEnemyWindup() => PlayEffect(enemyWindup);
    public void PlayEnemyFire() => PlayEffect(enemyFire);
    public void PlayDeath() => PlayEffect(death);
    public void PlayClick() => PlayEffect(click);
    public void PlayLose() => PlayEffect(death);
    public void PlayWin() => PlayEffect(win);
}
