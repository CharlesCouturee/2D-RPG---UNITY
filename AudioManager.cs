using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] float sfxMinimumDistance;
    [SerializeField] AudioSource[] sfx;
    [SerializeField] AudioSource[] bgm;

    public bool playBGM;

    [SerializeField] int bgmIndex;
    bool canPlaySFX;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        Invoke("AllowSFX", 1f);
    }

    private void Update()
    {
        if (!playBGM)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //return;

        if (canPlaySFX == false)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = UnityEngine.Random.Range(0.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));
    IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;
        while (_audio.volume > 0.1f)
        {
            _audio.volume -= _audio.volume * 0.2f;
            yield return new WaitForSeconds(0.40f);

            if (_audio.volume <= 0.1f)
                break;
        }

        _audio.Stop();
        _audio.volume = defaultVolume;
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void PlayRandomBGM()
    {
        bgmIndex = UnityEngine.Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    void AllowSFX() => canPlaySFX = true;
}
