using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    // 当前关卡ID
    public int currentLevelId;

    // 对话框最大宽度
    public float dialogueMaxWidth = 500f;

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
    }

    public void WinGame()
    {
        // 胜利 下一关
#if UNITY_EDITOR
        Debug.Log("WinGame");
#endif
    }

    public void LoseGame()
    {
        // 失败 重新开始
#if UNITY_EDITOR
        Debug.Log("LoseGame");
#endif
    }

    public void EndGame()
    {
        // 结束游戏
#if UNITY_EDITOR
        Debug.Log("EndGame");
#endif
    }

}
