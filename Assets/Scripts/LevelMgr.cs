using System.Collections.Generic;
using UnityEngine;

public static class LevelMgr
{
    public static Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();
    public static Dictionary<string, TopicData> topics = new Dictionary<string, TopicData>();
    public static Dictionary<string, DialogueData> dialogues = new Dictionary<string, DialogueData>();


    public static LevelData GetLevel(int id)
    {
        return levels.GetValueOrDefault(id.ToString());
    }

    public static void AddLevel(LevelData level)
    {
        levels[level.id] = level;
    }

    public static void Clear()
    {
        levels.Clear();
        topics.Clear();
        dialogues.Clear();
    }

    public static DialogueData GetDialogueData(string id)
    {
        return dialogues.GetValueOrDefault(id.Trim());
    }

    public static void AddDialogueData(DialogueData dialogue)
    {
        dialogues[dialogue.id] = dialogue;
    }

    public static TopicData GetTopic(string id)
    {
        return topics.GetValueOrDefault(id.Trim());
    }

    public static void AddTopic(TopicData topic)
    {
        topics[topic.id] = topic;
    }


}