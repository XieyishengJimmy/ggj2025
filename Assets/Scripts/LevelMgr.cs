using System.Collections.Generic;

public static class LevelMgr
{
    public static Dictionary<string, LevelNode> levels = new Dictionary<string, LevelNode>();
    public static Dictionary<string, TopicNode> topics = new Dictionary<string, TopicNode>();
    public static Dictionary<string, DialogueNode> nodes = new Dictionary<string, DialogueNode>();

    public static LevelNode GetLevel(string id)
    {
        return levels.GetValueOrDefault(id);
    }

    public static void AddLevel(LevelNode level)
    {
        levels[level.id] = level;
    }

    public static void Clear()
    {
        levels.Clear();
        topics.Clear();
        nodes.Clear();
    }

    public static DialogueNode GetNode(string id)
    {
        return nodes.GetValueOrDefault(id);
    }

    public static void AddNode(DialogueNode node)
    {
        nodes[node.id] = node;
    }

    public static TopicNode GetTopic(string id)
    {
        return topics.GetValueOrDefault(id);
    }

    public static void AddTopic(TopicNode topic)
    {
        topics[topic.id] = topic;
    }
}