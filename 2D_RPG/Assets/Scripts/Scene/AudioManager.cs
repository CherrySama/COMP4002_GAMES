using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;           // ��Ƶ����
        public AudioClip clip;        // ��Ƶ����
        [Range(0f, 1f)]
        public float volume = 1f;     // ����
        [Range(0.1f, 3f)]
        public float pitch = 1f;      // ����
        public bool loop = false;     // �Ƿ�ѭ��

        [HideInInspector]
        public AudioSource source;    // ��ƵԴ���
    }

    // ��Ƶ����
    //public Sound[] backgroundMusic;   // ��������
    public Sound[] soundEffects;      // ��Ч

    // ��������
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private void Awake()
    {
        // ʵ�ֵ���ģʽ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // �����л�ʱ������
        }

        // Ϊÿ����Ƶ����AudioSource���
        //InitializeAudioSources(backgroundMusic);
        InitializeAudioSources(soundEffects);
    }

    // ��ʼ����ƵԴ
    private void InitializeAudioSources(Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // ���ű�������
    //public void PlayMusic(string name)
    //{
    //    Sound s = System.Array.Find(backgroundMusic, sound => sound.name == name);
    //    if (s == null)
    //    {
    //        Debug.LogWarning("BGM " + name + " not found");
    //        return;
    //    }

    //    // ֹͣ��ǰ���ŵ����б�������
    //    foreach (Sound music in backgroundMusic)
    //    {
    //        if (music.source.isPlaying)
    //        {
    //            music.source.Stop();
    //        }
    //    }

    //    // ��������
    //    s.source.volume = s.volume * musicVolume * masterVolume;
    //    s.source.Play();
    //}

    // ������Ч
    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            return;
        }

        // ��������
        s.source.volume = s.volume * sfxVolume * masterVolume;
        s.source.Play();
    }

    // ֹͣ�ض���Ч
    public void StopSFX(string name)
    {
        Sound s = System.Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("BGM " + name + " not found");
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

    //// ֹͣ��������
    //public void StopMusic(string name)
    //{
    //    Sound s = System.Array.Find(backgroundMusic, sound => sound.name == name);
    //    if (s == null)
    //    {
    //        Debug.LogWarning("BGM " + name + " not found");
    //        return;
    //    }

    //    if (s.source.isPlaying)
    //    {
    //        s.source.Stop();
    //    }
    //}

    //// ��ͣ������Ƶ
    //public void PauseAllAudio()
    //{
    //    foreach (Sound s in backgroundMusic)
    //    {
    //        if (s.source.isPlaying)
    //        {
    //            s.source.Pause();
    //        }
    //    }

    //    foreach (Sound s in soundEffects)
    //    {
    //        if (s.source.isPlaying)
    //        {
    //            s.source.Pause();
    //        }
    //    }
    //}

    //// �ָ���������ͣ����Ƶ
    //public void ResumeAllAudio()
    //{
    //    foreach (Sound s in backgroundMusic)
    //    {
    //        if (!s.source.isPlaying)
    //        {
    //            s.source.UnPause();
    //        }
    //    }

    //    foreach (Sound s in soundEffects)
    //    {
    //        if (!s.source.isPlaying)
    //        {
    //            s.source.UnPause();
    //        }
    //    }
    //}

    // ����������
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    //// ������������
    //public void SetMusicVolume(float volume)
    //{
    //    musicVolume = Mathf.Clamp01(volume);
    //    UpdateMusicVolumes();
    //}

    // ������Ч����
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateSFXVolumes();
    }

    // ������������
    private void UpdateAllVolumes()
    {
        //UpdateMusicVolumes();
        UpdateSFXVolumes();
    }

    // ������������
    //private void UpdateMusicVolumes()
    //{
    //    foreach (Sound s in backgroundMusic)
    //    {
    //        s.source.volume = s.volume * musicVolume * masterVolume;
    //    }
    //}

    // ������Ч����
    private void UpdateSFXVolumes()
    {
        foreach (Sound s in soundEffects)
        {
            s.source.volume = s.volume * sfxVolume * masterVolume;
        }
    }
}