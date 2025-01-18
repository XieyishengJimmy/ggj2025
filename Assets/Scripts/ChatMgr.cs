using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMgr : MonoBehaviour
{
    public static ChatMgr Instance;

    public Button screenshotBtn;

    public Transform ChatContent;
    public Transform SendContent;
    public GameObject TopicPrefab;
    public GameObject OtherDialoguePrefab;
    public GameObject SelfDialoguePrefab;

    private LevelData currentLevel;
    private int remainingDeleteCount;
    private List<string> currentDialogueIds = new List<string>();

    void Awake()
    {
        Instance = this;
        screenshotBtn.onClick.AddListener(OnScreenshot);
    }

    void Start()
    {
        StartLevel(GameMgr.Instance.currentLevelId);
    }

    public void InitChat(LevelData level)
    {
        // 初始化对话列表
        currentLevel = level;
        remainingDeleteCount = currentLevel.deleteCount;
        ChatContent.ClearChildren();
        SendContent.ClearChildren();
        foreach (var topicId in currentLevel.topicIds)
        {
            GameObject topicObj = Instantiate(TopicPrefab, SendContent);
            topicObj.GetComponent<TopicEntity>().Init(LevelMgr.GetTopic(topicId));
            currentDialogueIds.Add(topicId);
        }
    }

    /// <summary>
    /// 选择话题
    /// </summary>
    public void SelectTopic(string topicId)
    {
        TopicData topicData = LevelMgr.GetTopic(topicId);
        foreach (var dialogueId in topicData.dialogueIds)
        {
            SendDialogue(dialogueId);
        }
    }

    public void SendDialogue(string dialogueId)
    {
        DialogueData dialogueData = LevelMgr.GetDialogueData(dialogueId);
        GameObject dialogueObj = Instantiate(dialogueData.isSelf ? SelfDialoguePrefab : OtherDialoguePrefab, ChatContent);
        dialogueObj.GetComponent<DialogueEntity>().Init(dialogueData);
    }

    public void DeleteDialogue(string dialogueId)
    {
        if (remainingDeleteCount > 0)
        {
            remainingDeleteCount--;
            foreach (Transform child in ChatContent)
            {
                DialogueData dialogue = child.GetComponent<DialogueData>();
                if (dialogue.id == dialogueId)
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
            currentDialogueIds.Remove(dialogueId);
        }
    }

    public void StartLevel(int levelId)
    {
        InitChat(LevelMgr.GetLevel(levelId));
    }

    public void NextLevel()
    {
        InitChat(LevelMgr.GetLevel(int.Parse(currentLevel.id) + 1));
    }

    public void RestartLevel()
    {
        InitChat(LevelMgr.GetLevel(int.Parse(currentLevel.id)));
    }

    /// <summary>
    /// 验证对话顺序
    /// </summary>
    public void ValidateOrder(List<string> dialogueIds)
    {
        if (currentLevel.correctOrder == dialogueIds)
        {
            GameMgr.Instance.WinGame();
        }
        else
        {
            GameMgr.Instance.LoseGame();
        }
    }

    // 返回到主菜单
    public void BackToMenu()
    {
        // 返回到主菜单
    }

    public void OnScreenshot()
    {
        ValidateOrder(currentDialogueIds);
    }

}