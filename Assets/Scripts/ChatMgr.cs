using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatMgr : MonoBehaviour
{
    public static ChatMgr Instance;

    public Image avatar;

    public Text nicknameText;
    public ScrollRect messageSR;
    public RectTransform sendBg;
    public Button msgBarBtn;
    public Transform SendContent;
    public GameObject TopicPrefab;
    public GameObject OtherDialoguePrefab;
    public GameObject SelfDialoguePrefab;

    public GuideUI guideUIPanel;
    public Button screenshotBtn;
    public Image shareResults;
    public Text remainingDeleteCountText;
    public Button nextLevelBtn;
    public Button restartBtn;
    public Text targetText;

    public bool enableButton = true;

    private LevelData currentLevel;
    private int remainingDeleteCount;
    private bool isSendMsgShow = true;
    private Dictionary<string, Sprite[]> avatarResource = new Dictionary<string, Sprite[]>();
    private Coroutine initDialogueCoroutine;
    private Coroutine avatarCoroutine;
    private List<string> deletedDialogueIds = new List<string>();

    void Awake()
    {
        if (!Utils.InitGame)
        {
            SceneManager.LoadScene("StartScene");
        }
        Instance = this;
        screenshotBtn.onClick.AddListener(OnScreenshot);
        nextLevelBtn.onClick.AddListener(NextLevel);
        restartBtn.onClick.AddListener(RestartLevel);
        msgBarBtn.onClick.AddListener(OnMsgBarBtn);
    }

    void Start()
    {
        // InitAvatarResource();
        StartLevel(GameMgr.Instance.currentLevelId);
        // StartLevel(2);
        guideUIPanel.OpenGuideUI();
    }

    /// <summary>
    /// 初始化聊天界面
    /// </summary>
    public void InitChat(LevelData level)
    {
        // 初始化对话列表
        GameMgr.Instance.currentLevelId = int.Parse(level.id);
        currentLevel = level;
        remainingDeleteCount = currentLevel.deleteCount;
        nicknameText.text = currentLevel.otherName;

        shareResults.gameObject.SetActive(false);
        nextLevelBtn.gameObject.SetActive(false);

        LoadAvatar(currentLevel.avatarPath);

        messageSR.content.ClearChildren();
        SendContent.ClearChildren();

        deletedDialogueIds.Clear();

        remainingDeleteCountText.text = "X" + remainingDeleteCount.ToString();
        targetText.text = "分享目标：\n\n" + currentLevel.description;
        InitTopics(currentLevel.topicIds);

        ShowSendContent();

        if (initDialogueCoroutine != null)
        {
            StopCoroutine(initDialogueCoroutine);
        }
        initDialogueCoroutine = StartCoroutine(InitDialogue());
    }

    /// <summary>
    /// 初始化头像帧动画资源
    /// </summary>
    public void InitAvatarResource()
    {
        string n = "Body3_Normal";
        var res = Resources.LoadAll<Sprite>("Arts/Body3/Normal");
        avatarResource[n] = res;

        n = "Body3_Angry";
        res = Resources.LoadAll<Sprite>("Arts/Body3/Angry");
        avatarResource[n] = res;

        /*
        n = "Body4_Normal";
        res = Resources.LoadAll<Sprite>("Arts/Body4/Normal");
        avatarResource[n] = res;

        n = "Body4_Angry";
        res = Resources.LoadAll<Sprite>("Arts/Body4/Angry");
        avatarResource[n] = res;
        */

    }

    /// <summary>
    /// 加载头像
    /// </summary>
    public void LoadAvatar(string avatarPath, string avatarState = "Normal")
    {
        if (!string.IsNullOrEmpty(avatarPath))
        {
            avatar.sprite = Resources.Load<Sprite>("Arts/" + avatarPath + "_" + avatarState);
        }
        else
        {
            avatar.sprite = Resources.Load<Sprite>("Arts/Body3_Normal");
        }
        avatar.SetNativeSize();

        /*
        if (avatarCoroutine != null)
        {
            StopCoroutine(avatarCoroutine);
        }
        if (avatarPath == "Body6")
        {
            avatarPath = "Body3";
        }
        avatarCoroutine = StartCoroutine(ChatCharacterAvatarAnimation(avatarPath + "_" + avatarState));
        */
    }

    /// <summary>
    /// 聊天角色头像动画
    /// </summary>
    public IEnumerator ChatCharacterAvatarAnimation(string avatarPath)
    {
        if (avatarResource.ContainsKey(avatarPath) && avatarResource[avatarPath].Length > 1)
        {
            while (true)
            {
                for (int i = 0; i < avatarResource[avatarPath].Length; i++)
                {
                    avatar.sprite = avatarResource[avatarPath][i];
                    avatar.SetNativeSize();
                    yield return new WaitForSeconds(0.082f);
                }
            }
        }
        yield return null;
    }

    /// <summary>
    /// 初始化话题
    /// </summary>
    public void InitTopics(List<string> topicIds)
    {
        foreach (var topicId in topicIds)
        {
            GameObject topicObj = Instantiate(TopicPrefab, SendContent);
            topicObj.GetComponent<TopicEntity>().Init(LevelMgr.GetTopic(topicId));
        }

        switch (topicIds.Count)
        {
            case 3:
                SendContent.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-142, 54);
                SendContent.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -72);
                SendContent.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(141, 23);
                break;
            case 4:
                SendContent.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-161, 54);
                SendContent.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-78, -72);
                SendContent.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(58, 54);
                SendContent.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(156, -72);
                break;
            case 5:
                SendContent.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-161, 54);
                SendContent.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-161, -73);
                SendContent.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                SendContent.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(156, 54);
                SendContent.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(156, -73);
                break;
            case 6:
                SendContent.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-161, 54);
                SendContent.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-161, -62);
                SendContent.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-2, 72);
                SendContent.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(-2, -45);
                SendContent.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(156, 54);
                SendContent.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(156, -62);
                break;
        }
    }


    /// <summary>
    /// 滑动条置底
    /// </summary>
    public void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        messageSR.verticalNormalizedPosition = 0f;
    }

    // 禁用按钮
    public void DisableBtn()
    {
        enableButton = false;
        msgBarBtn.interactable = false;
        screenshotBtn.interactable = false;
        restartBtn.interactable = false;
    }

    // 启用按钮
    public void EnableBtn()
    {
        enableButton = true;
        msgBarBtn.interactable = true;
        screenshotBtn.interactable = true;
        restartBtn.interactable = true;
    }

    /// <summary>
    /// 选择话题
    /// </summary>
    public void SelectTopic(string topicId)
    {
        OnMsgBarBtn();
        StartCoroutine(SendTopic(topicId));
    }

    public void SendDialogue(string dialogueId)
    {
        Debug.Log("SendDialogue: " + dialogueId);
        DialogueData dialogueData = LevelMgr.GetDialogueData(dialogueId);
        Debug.Log("dialogueData: " + dialogueData);
        GameObject dialogueObj = Instantiate(dialogueData.isSelf ? SelfDialoguePrefab : OtherDialoguePrefab, messageSR.content);
        dialogueObj.GetComponent<DialogueEntity>().Init(dialogueData);

        ScrollToBottom();
    }

    /// <summary>
    /// 初始化对话内容
    /// </summary>
    public IEnumerator InitDialogue()
    {
        if (currentLevel.initDialogueIds.Count == 0)
        {
            yield break;
        }

        GameMgr.Instance.PlaySound("TopicSFX");

        HideSendContent();
        DisableBtn();

        yield return new WaitForSeconds(0.191f);
        SendDialogue(currentLevel.initDialogueIds[0]);

        yield return new WaitForSeconds(0.716f);
        for (int i = 1; i < currentLevel.initDialogueIds.Count; i++)
        {
            SendDialogue(currentLevel.initDialogueIds[i]);
            GameMgr.Instance.PlaySound("WordSFX");
            yield return new WaitForSeconds(0.6f);
        }

        EnableBtn();
        ShowSendContent();
    }

    /// <summary>
    /// 发送话题
    /// </summary>
    public IEnumerator SendTopic(string topicId)
    {
        GameMgr.Instance.PlaySound("TopicSFX");

        TopicData topicData = LevelMgr.GetTopic(topicId);

        DisableBtn();

        yield return new WaitForSeconds(0.191f);
        SendDialogue(topicData.dialogueIds[0]);

        yield return new WaitForSeconds(0.716f);
        if (topicData.dialogueIds.Count > 1)
        {
            for (int i = 1; i < topicData.dialogueIds.Count; i++)
            {
                SendDialogue(topicData.dialogueIds[i]);
                GameMgr.Instance.PlaySound("WordSFX");
                yield return new WaitForSeconds(0.6f);
            }
        }

        EnableBtn();
    }

    public void DeleteDialogue(string dialogueId)
    {
        StartCoroutine(DeleteDialogueCoroutine(dialogueId));
    }

    public IEnumerator DeleteDialogueCoroutine(string dialogueId)
    {
        if (deletedDialogueIds.Contains(dialogueId))
        {
            yield break;
        }
        else
        {
            deletedDialogueIds.Add(dialogueId);
        }

        if (remainingDeleteCount > 0)
        {
            remainingDeleteCount--;
            foreach (Transform child in messageSR.content)
            {
                DialogueEntity dialogue = child.GetComponent<DialogueEntity>();
                if (dialogue.dialogueId == dialogueId)
                {
                    remainingDeleteCountText.text = "X" + remainingDeleteCount.ToString();

                    DialogueContentEntity content = child.Find("DialogueContainer").GetComponentInChildren<DialogueContentEntity>();

                    yield return content.PlayAnimationCoroutine();

                    child.SetParent(null);
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
    }

    public void StartLevel(int levelId)
    {
        InitChat(LevelMgr.GetLevel(levelId));
    }

    public void NextLevel()
    {
        GameMgr.Instance.PlaySound("ButtonSFX");
        int next = int.Parse(currentLevel.id) + 1;
        if (next > LevelMgr.levels.Count)
        {
            return;
        }
        InitChat(LevelMgr.GetLevel(next));
    }

    public void RestartLevel()
    {
        GameMgr.Instance.PlaySound("ButtonSFX");
        InitChat(LevelMgr.GetLevel(int.Parse(currentLevel.id)));
    }

    /// <summary>
    /// 验证对话顺序
    /// </summary>
    public void ValidateOrder()
    {
        List<string> dialogueIds = new List<string>();
        foreach (Transform child in messageSR.content)
        {
            DialogueEntity dialogue = child.GetComponent<DialogueEntity>();
            dialogueIds.Add(dialogue.dialogueId);
        }

        // 首先检查长度是足够
        if (dialogueIds.Count < currentLevel.correctOrder.Count)
        {
#if UNITY_EDITOR
            Debug.Log("对话数量不足");
#endif
            shareResults.gameObject.SetActive(true);
            Sprite failedSprite = Resources.Load<Sprite>("Images/ShareFailed");
            shareResults.sprite = failedSprite;
            shareResults.SetNativeSize();
            nextLevelBtn.gameObject.SetActive(false);
            GameMgr.Instance.PlaySound("LoseSFX");
            return;
        }

        // 从后往前比较元素
        bool isCorrect = true;
        int correctLen = currentLevel.correctOrder.Count;
        for (int i = 0; i < correctLen; i++)
        {
            if (dialogueIds[dialogueIds.Count - 1 - i] != currentLevel.correctOrder[correctLen - 1 - i])
            {
                isCorrect = false;
                break;
            }
        }

        shareResults.gameObject.SetActive(true);

        if (isCorrect)
        {
            LoadAvatar(currentLevel.avatarPath, "Angry");
            Sprite successSprite = Resources.Load<Sprite>("Images/ShareSuccess");
            shareResults.sprite = successSprite;
            shareResults.SetNativeSize();
            nextLevelBtn.gameObject.SetActive(true);
            GameMgr.Instance.PlaySound("WinSFX");
        }
        else
        {
            Sprite failedSprite = Resources.Load<Sprite>("Images/ShareFailed");
            shareResults.sprite = failedSprite;
            shareResults.SetNativeSize();
            nextLevelBtn.gameObject.SetActive(false);
            GameMgr.Instance.PlaySound("LoseSFX");
        }
    }

    /// <summary>
    /// 截图
    /// </summary>
    public void OnScreenshot()
    {
        StartCoroutine(ScreenshotCoroutine());
    }

    public IEnumerator ScreenshotCoroutine()
    {
        DisableBtn();
        GameMgr.Instance.PlaySound("ShotScreen");
        yield return new WaitForSeconds(0.705f);
        ValidateOrder();
        EnableBtn();
    }

    public void OnMsgBarBtn()
    {
        RectTransform msgRT = messageSR.GetComponent<RectTransform>();
        RectTransform msgBarBtnRT = msgBarBtn.GetComponent<RectTransform>();
        if (isSendMsgShow)
        {
            sendBg.anchoredPosition = new Vector2(sendBg.anchoredPosition.x, sendBg.anchoredPosition.y - (sendBg.sizeDelta.y - msgBarBtnRT.sizeDelta.y));
            msgRT.sizeDelta = new Vector2(msgRT.sizeDelta.x, msgRT.sizeDelta.y + (sendBg.sizeDelta.y - msgBarBtnRT.sizeDelta.y));
            sendBg.GetComponent<Image>().color = new Color(1, 1, 1, 0f);
            SendContent.gameObject.SetActive(false);
        }
        else
        {
            sendBg.anchoredPosition = new Vector2(sendBg.anchoredPosition.x, sendBg.anchoredPosition.y + (sendBg.sizeDelta.y - msgBarBtnRT.sizeDelta.y));
            msgRT.sizeDelta = new Vector2(msgRT.sizeDelta.x, msgRT.sizeDelta.y - (sendBg.sizeDelta.y - msgBarBtnRT.sizeDelta.y));
            sendBg.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            SendContent.gameObject.SetActive(true);
        }
        ScrollToBottom();
        isSendMsgShow = !isSendMsgShow;
    }

    public void ShowSendContent()
    {
        RectTransform msgRT = messageSR.GetComponent<RectTransform>();
        RectTransform msgBarBtnRT = msgBarBtn.GetComponent<RectTransform>();
        if (!isSendMsgShow)
        {
            sendBg.anchoredPosition = new Vector2(sendBg.anchoredPosition.x, sendBg.anchoredPosition.y + (sendBg.sizeDelta.y - msgBarBtnRT.sizeDelta.y));
            msgRT.sizeDelta = new Vector2(msgRT.sizeDelta.x, msgRT.sizeDelta.y - (sendBg.sizeDelta.y - msgBarBtnRT.sizeDelta.y));
            sendBg.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            SendContent.gameObject.SetActive(true);
            isSendMsgShow = true;
        }
    }

    public void HideSendContent()
    {
        RectTransform msgRT = messageSR.GetComponent<RectTransform>();
        RectTransform msgBarBtnRT = msgBarBtn.GetComponent<RectTransform>();
        if (isSendMsgShow)
        {
            sendBg.anchoredPosition = new Vector2(sendBg.anchoredPosition.x, sendBg.anchoredPosition.y - (sendBg.sizeDelta.y - msgBarBtnRT.sizeDelta.y));
            msgRT.sizeDelta = new Vector2(msgRT.sizeDelta.x, msgRT.sizeDelta.y + (sendBg.sizeDelta.y - msgBarBtnRT.sizeDelta.y));
            sendBg.GetComponent<Image>().color = new Color(1, 1, 1, 0f);
            SendContent.gameObject.SetActive(false);
            isSendMsgShow = false;
        }
    }

}