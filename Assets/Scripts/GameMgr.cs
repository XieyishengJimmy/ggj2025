using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    // 当前关卡ID
    public int currentLevelId;

    public void Initialize()
    {
        // 清空现有数据
        LevelMgr.Clear();

        // 加载所有关卡数据
        LevelData[] allLevels = Resources.LoadAll<LevelData>("Levels");
        foreach (var level in allLevels)
        {
            var levelData = new LevelData
            {
                id = level.id,
                levelName = level.levelName,
                description = level.description,
                otherAvatar = level.otherAvatar,
                otherName = level.otherName,
                selfAvatar = level.selfAvatar,
                selfName = level.selfName,
                deleteCount = level.deleteCount,
                topicIds = level.topicIds,
                correctOrder = level.correctOrder
            };
            LevelMgr.AddLevel(levelData);
        }

        // 加载所有TopicData资源
        TopicData[] allTopics = Resources.LoadAll<TopicData>("Topics");
        foreach (var topic in allTopics)
        {
            var topicData = new TopicData
            {
                id = topic.id,
                content = topic.content,
                dialogueIds = topic.dialogueIds
            };
            LevelMgr.AddTopic(topicData);
        }

        // 加载所有DialogueData资源
        DialogueData[] allDialogues = Resources.LoadAll<DialogueData>("Dialogues");
        foreach (var dialogue in allDialogues)
        {
            var dialogueData = new DialogueData
            {
                id = dialogue.id,
                isSelf = dialogue.isSelf,
                content = dialogue.content,
                image = dialogue.image
            };
            LevelMgr.AddDialogueData(dialogueData);
        }
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
