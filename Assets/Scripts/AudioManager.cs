using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource; // nhạc nền
    [SerializeField] private AudioSource sfxSource; // sound effect

    [Header("BGM Clips")]
    [SerializeField] private AudioClip bgMusicNormal;
    [SerializeField] private AudioClip bgMusicBoss;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip playerAttack;
    [SerializeField] private AudioClip enemyShout;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip Fire;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Nhạc nền
    public void PlayNormalBGM()
    {
        bgmSource.clip = bgMusicNormal;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayBossBGM()
    {
        bgmSource.clip = bgMusicBoss;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // Sound effects
    public void PlayPlayerAttack() => sfxSource.PlayOneShot(playerAttack);
    public void PlayEnemyShout() => sfxSource.PlayOneShot(enemyShout);
    public void PlayExplosion() => sfxSource.PlayOneShot(explosion);
    public void PlayFire() => sfxSource.PlayOneShot(Fire);
}
