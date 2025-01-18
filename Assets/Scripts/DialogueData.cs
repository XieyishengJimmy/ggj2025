using UnityEngine;

/// <summary>
/// 对话配置数据
/// </summary>
[CreateAssetMenu(fileName = "Dialogue", menuName = "对话系统/对话")]
public class DialogueData : ScriptableObject
{
    [Tooltip("对话ID Id规则: TopicId_DialogueId")]
    public string id;
    [Tooltip("是否是自己的对话")]
    public bool isSelf;
    [Tooltip("对话内容")]
    [TextArea(3, 10)]
    public string content;
    [Tooltip("对话图片路径：文件名")]
    public string image;
}