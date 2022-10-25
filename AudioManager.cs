using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {
    //public enum SoundType { MeleeHit, RangedHit, Music, Miss };

    //[SerializeField] AudioClip[] meleeHitSounds;
    //[SerializeField] AudioClip[] rangedHitSounds;
    //[SerializeField] AudioClip[] music;
    //[SerializeField] AudioClip miss;

    [SerializeField] AudioClip[] allSounds;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public static AudioManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    IEnumerator PlaySFX(AudioClip clip) {
        musicSource.volume = .7f;
        sfxSource.PlayOneShot(clip);
        yield return new WaitWhile(() => sfxSource.isPlaying);
        musicSource.volume = 1f;
    }

    public void PlaySound(String name) {
        AudioClip s = null;
        if (!string.IsNullOrEmpty(name)) {
            s = Array.Find(allSounds, s => s.name == name);
            if (s == null) {
                Debug.LogWarning($"Sound: {name} not found");
                return;
            }
        }
        StartCoroutine(PlaySFX(s));
    }

    //public void PlaySound(SoundType soundType) {
    //    switch (soundType) {
    //        case SoundType.MeleeHit:
    //            StartCoroutine(PlaySFX(meleeHitSounds[Random.Range(0, meleeHitSounds.Length)]));
    //            break;
    //        case SoundType.RangedHit:
    //            StartCoroutine(PlaySFX(rangedHitSounds[Random.Range(0, rangedHitSounds.Length)]));
    //            break;
    //        case SoundType.Music:
    //            musicSource.clip = music[Random.Range(0, music.Length)];
    //            musicSource.loop = true;
    //            musicSource.Play();
    //            break;
    //        case SoundType.Miss:
    //            StartCoroutine(PlaySFX(miss));
    //            break;
    //    }
    //}
}
