using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXPool : MonoBehaviour
{
    private List<AudioSource> _audioSourceList;
    [SerializeField] public int poolSize = 10;
    public AudioMixerGroup audioMixer;
    public float defaultVolume = .05f;
    private int _index = 0;
    private bool _mute = false;

    private void Awake()
    {
        CreatePool();
    }
    private void CreatePool()
    {
        _audioSourceList = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            CreateAudioSourceItem();
        }
    }

    private void CreateAudioSourceItem()
    {
        GameObject o = new GameObject("SFX_Pool");
        o.transform.SetParent(transform);
        var oWithAudioSource = o.AddComponent<AudioSource>();
        oWithAudioSource.volume = defaultVolume;
        oWithAudioSource.outputAudioMixerGroup = audioMixer;
        _audioSourceList.Add(oWithAudioSource);
    }

    public void Play(SFXType SFXType)
    {
        if (SFXType == SFXType.NONE) return;
        SFXSetup setup = SoundManager.instance.GetSFXByType(SFXType);
        _audioSourceList[_index].clip = setup.audioClip;
        _audioSourceList[_index].Play();
        _index++;

        if (_index >= _audioSourceList.Count) _index = 0;
    }

   /* private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _mute = !_mute;
        }

        if (_mute)
        {
            _audioSourceList.ForEach(x => x.volume = 0f);
        }
        else
        {
            _audioSourceList.ForEach(x => x.volume = defaultVolume);
        }
    }*/
}
