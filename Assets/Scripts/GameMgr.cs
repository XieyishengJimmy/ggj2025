using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    // 当前关卡ID
    public int currentLevelId;
    // 对话框最大宽度
    public float dialogueMaxWidth = 360f;
    [Header("音频设置")]
    public AudioSource musicSource; // 背景音乐播放器
    public AudioSource soundSource; // 音效播放器
    [Range(0, 1)]
    public float musicVolume = 1f; // 音乐音量
    [Range(0, 1)]
    public float soundVolume = 1f; // 音效音量

    public Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    public void Initialize()
    {
        // 清空现有数据
        LevelMgr.Clear();

        // 加载所有关卡数据
        LevelData[] allLevels = Resources.LoadAll<LevelData>("Levels");
        foreach (var level in allLevels)
        {
            var levelData = ScriptableObject.Instantiate(level);
            LevelMgr.AddLevel(levelData);
        }

        // 加载所有TopicData资源
        TopicData[] allTopics = Resources.LoadAll<TopicData>("Topics");
        foreach (var topic in allTopics)
        {
            var topicData = ScriptableObject.Instantiate(topic);
            LevelMgr.AddTopic(topicData);
        }

        // 加载所有DialogueData资源
        DialogueData[] allDialogues = Resources.LoadAll<DialogueData>("Dialogues");
        foreach (var dialogue in allDialogues)
        {
            var dialogueData = ScriptableObject.Instantiate(dialogue);
            LevelMgr.AddDialogueData(dialogueData);
        }

        currentLevelId = 1;

        // 初始化音频
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        if (soundSource == null)
            soundSource = gameObject.AddComponent<AudioSource>();

        // 设置音频源属性
        musicSource.loop = true;
        soundSource.loop = false;

        UpdateVolume();


    }

    // 播放背景音乐
    public void PlayMusic(AudioClip music)
    {
        if (music == null || musicSource == null) return;

        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlayMusic(string name)
    {
        if (sounds.ContainsKey(name))
        {
            PlayMusic(sounds[name]);
        }
        else
        {
            sounds[name] = Resources.Load<AudioClip>("Audios/" + name);
            PlayMusic(sounds[name]);
        }
    }

    // 播放音效
    public void PlaySound(AudioClip sound)
    {
        if (sound == null || soundSource == null) return;

        soundSource.PlayOneShot(sound, soundVolume);
    }

    public void PlaySound(string name)
    {
        if (sounds.ContainsKey(name))
        {
            PlaySound(sounds[name]);
        }
        else
        {
            sounds[name] = Resources.Load<AudioClip>("Audios/" + name);
            PlaySound(sounds[name]);
        }
    }

    // 更新音量
    public void UpdateVolume()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;
        if (soundSource != null)
            soundSource.volume = soundVolume;
    }

    // 停止音乐
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

}
