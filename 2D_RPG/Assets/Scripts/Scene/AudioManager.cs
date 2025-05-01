using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;           // 音频名称
        public AudioClip clip;        // 音频剪辑
        [Range(0f, 1f)]
        public float volume = 1f;     // 音量
        [Range(0.1f, 3f)]
        public float pitch = 1f;      // 音高
        public bool loop = false;     // 是否循环

        [HideInInspector]
        public AudioSource source;    // 音频源组件
    }

    // 音频数组
    //public Sound[] backgroundMusic;   // 背景音乐
    public Sound[] soundEffects;      // 音效

    // 音量控制
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private void Awake()
    {
        // 实现单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 场景切换时不销毁
        }

        // 为每个音频创建AudioSource组件
        //InitializeAudioSources(backgroundMusic);
        InitializeAudioSources(soundEffects);
    }

    // 初始化音频源
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

    // 播放背景音乐
    //public void PlayMusic(string name)
    //{
    //    Sound s = System.Array.Find(backgroundMusic, sound => sound.name == name);
    //    if (s == null)
    //    {
    //        Debug.LogWarning("BGM " + name + " not found");
    //        return;
    //    }

    //    // 停止当前播放的所有背景音乐
    //    foreach (Sound music in backgroundMusic)
    //    {
    //        if (music.source.isPlaying)
    //        {
    //            music.source.Stop();
    //        }
    //    }

    //    // 更新音量
    //    s.source.volume = s.volume * musicVolume * masterVolume;
    //    s.source.Play();
    //}

    // 播放音效
    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            return;
        }

        // 更新音量
        s.source.volume = s.volume * sfxVolume * masterVolume;
        s.source.Play();
    }

    // 停止特定音效
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

    //// 停止背景音乐
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

    //// 暂停所有音频
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

    //// 恢复所有已暂停的音频
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

    // 更新主音量
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    //// 更新音乐音量
    //public void SetMusicVolume(float volume)
    //{
    //    musicVolume = Mathf.Clamp01(volume);
    //    UpdateMusicVolumes();
    //}

    // 更新音效音量
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateSFXVolumes();
    }

    // 更新所有音量
    private void UpdateAllVolumes()
    {
        //UpdateMusicVolumes();
        UpdateSFXVolumes();
    }

    // 更新音乐音量
    //private void UpdateMusicVolumes()
    //{
    //    foreach (Sound s in backgroundMusic)
    //    {
    //        s.source.volume = s.volume * musicVolume * masterVolume;
    //    }
    //}

    // 更新音效音量
    private void UpdateSFXVolumes()
    {
        foreach (Sound s in soundEffects)
        {
            s.source.volume = s.volume * sfxVolume * masterVolume;
        }
    }
}