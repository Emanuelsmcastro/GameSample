using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Singleton;

public class SoundManager : Singleton<SoundManager>
{
    public List<SFXSetup> SFXSetups;
    public List<MusicSetup> musicSetups;

    public AudioSource musicSource;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    public void PlayMusicByType(MusicType musicType)
    {
        MusicSetup musicSetup = GetMusicByType(musicType);
        if (musicSetup != null)
        {
            musicSource.clip = musicSetup.audioClip;
            musicSource.Play();
        }
    }

    public MusicSetup GetMusicByType(MusicType musicType)
    {
        MusicSetup musicSetup = musicSetups.Find(setup => setup.musicType == musicType);
        return musicSetup;
    }

    public SFXSetup GetSFXByType(SFXType SFXType)
    {
        SFXSetup SFXSetup = SFXSetups.Find(setup => setup.SFXType == SFXType);
        return SFXSetup;
    }
}


public enum MusicType
{
    TYPE1,
    TYPE2,
    TYPE3
}
[System.Serializable]
public class MusicSetup
{
    public MusicType musicType;
    public AudioClip audioClip;
}

public enum SFXType
{
    NONE,
    CollectCoin,
    PlayerDeath,
    PlayerJump
}

[System.Serializable]
public class SFXSetup
{
    public SFXType SFXType;
    public AudioClip audioClip;
}