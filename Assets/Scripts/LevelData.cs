using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 关卡配置数据
/// </summary>
[CreateAssetMenu(fileName = "Level", menuName = "对话系统/关卡")]
public class LevelData : ScriptableObject
{
    [Tooltip("关卡ID")]
    public string id;
    [Tooltip("关卡名称")]
    public string levelName;
    [Tooltip("关卡描述")]
    [TextArea(2, 5)]
    public string description;
    [Tooltip("对方头像")]
    public string otherAvatar;
    [Tooltip("对方名字")]
    public string otherName;
    [Tooltip("自己头像")]
    public string selfAvatar;
    [Tooltip("自己名字")]
    public string selfName;
    [Tooltip("删除次数")]
    public int deleteCount;
    [Tooltip("话题ID列表")]
    public List<string> topicIds = new List<string>();
    [Tooltip("正确的对话顺序")]
    public List<string> correctOrder = new List<string>();
}