using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    public void Initialize()
    {
        // 清空现有数据
        LevelMgr.Clear();

        // 加载所有关卡数据
        LevelData[] allLevels = Resources.LoadAll<LevelData>("Levels");
        foreach (var level in allLevels)
        {
            var levelNode = new LevelNode
            {
                id = level.id,
                levelName = level.levelName,
                description = level.description,
                otherAvatar = level.otherAvatar,
                otherName = level.otherName,
                selfAvatar = level.selfAvatar,
                selfName = level.selfName,
                topicIds = level.topicIds,
                correctOrder = level.correctOrder
            };
            LevelMgr.AddLevel(levelNode);
        }

        // 加载所有TopicData资源
        TopicData[] allTopics = Resources.LoadAll<TopicData>("Topics");
        foreach (var topic in allTopics)
        {
            var topicNode = new TopicNode
            {
                id = topic.id,
                content = topic.content,
                dialogueIds = topic.dialogueIds
            };
            LevelMgr.AddTopic(topicNode);
        }

        // 加载所有DialogueData资源
        DialogueData[] allDialogues = Resources.LoadAll<DialogueData>("Dialogues");
        foreach (var dialogue in allDialogues)
        {
            var node = new DialogueNode
            {
                id = dialogue.id,
                isSelf = dialogue.isSelf,
                content = dialogue.content,
                image = dialogue.image
            };
            LevelMgr.AddNode(node);
        }
    }

}
