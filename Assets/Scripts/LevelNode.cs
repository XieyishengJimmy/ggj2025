using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode
{
    // 关卡ID
    public string id;
    // 关卡名称
    public string levelName;
    // 关卡描述
    public string description;
    // 关卡图片路径
    //public string image;
    // 关卡背景音乐
    //public string bgm;
    // 对方的头像
    public string otherAvatar;
    // 对方的名字
    public string otherName;
    // 自己的头像
    public string selfAvatar;
    // 自己的名字
    public string selfName;
    // 话题列表
    public List<string> topicIds = new List<string>();
    // 正确的对话顺序
    public List<string> correctOrder = new List<string>();
}
