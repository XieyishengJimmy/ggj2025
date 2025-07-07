using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 话题配置数据
/// </summary>
[CreateAssetMenu(fileName = "Topic", menuName = "对话系统/话题")]
public class TopicData : ScriptableObject
{
    [Tooltip("话题ID")]
    public string id;
    [Tooltip("话题内容")]
    [TextArea(3, 10)]
    public string content;
    [Tooltip("对话ID列表")]
    public List<string> dialogueIds = new List<string>();
}

public class NewNum
{

}