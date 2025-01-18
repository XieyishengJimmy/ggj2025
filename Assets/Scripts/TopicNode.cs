using System.Collections.Generic;

/// <summary>
/// 话题节点
/// </summary>
public class TopicNode
{
    public string id;
    public string content;
    // 对话列表
    public List<string> dialogueIds = new List<string>();
}
