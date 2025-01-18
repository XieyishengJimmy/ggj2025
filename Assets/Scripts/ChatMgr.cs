using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatMgr : MonoBehaviour
{
    public static ChatMgr Instance;

    public Button screenshotBtn;

    public ScrollRect messageSR;
    public RectTransform sendBg;
    public Button msgBarBtn;
    public Transform SendContent;
    public GameObject TopicPrefab;
    public GameObject OtherDialoguePrefab;
    public GameObject SelfDialoguePrefab;
    public Text remainingDeleteCountText;
    public Button restartBtn;
    public Text targetText;

    private LevelData currentLevel;
    private int remainingDeleteCount;

    private bool isSendMsgShow = true;

    private List<Rect> placedTopicRects = new List<Rect>(); // 记录已放置的主题位置

    void Awake()
    {
        if (!Utils.InitGame)
        {
            SceneManager.LoadScene("StartScene");
        }
        Instance = this;
        screenshotBtn.onClick.AddListener(OnScreenshot);
        restartBtn.onClick.AddListener(RestartLevel);
        msgBarBtn.onClick.AddListener(OnMsgBarBtn);
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

        messageSR.content.ClearChildren();
        SendContent.ClearChildren();

        remainingDeleteCountText.text = remainingDeleteCount.ToString();
        targetText.text = currentLevel.description;
        // InitTopics(currentLevel.topicIds);
        InitTopicsR(currentLevel.topicIds);
    }

    public void InitTopics(List<string> topicIds)
    {
        RectTransform contentRect = SendContent.GetComponent<RectTransform>();
        placedTopicRects.Clear();

        float minSpacing = 20f;
        int maxAttempts = 50;

        foreach (var topicId in topicIds)
        {
            GameObject topicObj = Instantiate(TopicPrefab, SendContent);
            RectTransform topicRect = topicObj.GetComponent<RectTransform>();
            topicObj.GetComponent<TopicEntity>().Init(LevelMgr.GetTopic(topicId));

            // 获取内容区域和主题的矩形范围
            Rect contentBounds = new Rect(-contentRect.rect.width / 2, -contentRect.rect.height / 2,
                                        contentRect.rect.width, contentRect.rect.height);
            Rect topicBounds = new Rect(0, 0, topicRect.rect.width, topicRect.rect.height);

            Vector2 position = Vector2.zero;
            bool validPosition = false;
            int attempts = 0;

            while (!validPosition && attempts < maxAttempts)
            {
                position = CalculateRandomPosition(contentBounds, topicBounds);
                validPosition = true;

                // 检查是否与已放置的主题重叠
                foreach (var placedRect in placedTopicRects)
                {
                    if (IsRectOverlapping(
                        position.x - topicBounds.width / 2,
                        position.y - topicBounds.height / 2,
                        topicBounds.width,
                        topicBounds.height,
                        placedRect.x,
                        placedRect.y,
                        placedRect.width,
                        placedRect.height,
                        minSpacing))
                    {
                        validPosition = false;
                        break;
                    }
                }
                attempts++;
            }

            // 如果找不到合适位置，调整到边缘位置
            if (!validPosition)
            {
                position = new Vector2(
                    contentBounds.x + topicBounds.width / 2 + placedTopicRects.Count * (minSpacing + topicBounds.width),
                    contentBounds.y + topicBounds.height / 2
                );
            }

            topicRect.localPosition = position;
            placedTopicRects.Add(new Rect(
                position.x - topicBounds.width / 2,
                position.y - topicBounds.height / 2,
                topicBounds.width,
                topicBounds.height
            ));
        }
    }

    public void InitTopicsR(List<string> topicIds)
    {
        RectTransform contentRect = SendContent.GetComponent<RectTransform>();
        placedTopicRects.Clear();

        // 计算圆的半径（取内容区域宽高的较小值的一半的80%作为半径）
        float radius = Mathf.Min(contentRect.rect.width, contentRect.rect.height) * 0.4f;

        // 获取主题预制件的尺寸
        GameObject tempTopic = Instantiate(TopicPrefab, SendContent);
        RectTransform tempRect = tempTopic.GetComponent<RectTransform>();
        float topicWidth = tempRect.rect.width;
        float topicHeight = tempRect.rect.height;
        Destroy(tempTopic);

        // 计算主题之间的角度间隔
        float angleStep = 360f / topicIds.Count;

        for (int i = 0; i < topicIds.Count; i++)
        {
            GameObject topicObj = Instantiate(TopicPrefab, SendContent);
            RectTransform topicRect = topicObj.GetComponent<RectTransform>();
            topicObj.GetComponent<TopicEntity>().Init(LevelMgr.GetTopic(topicIds[i]));

            // 计算当前主题的角度
            float angle = i * angleStep;

            // 将角度转换为弧度
            float radian = angle * Mathf.Deg2Rad;

            // 计算位置
            float xPos = radius * Mathf.Cos(radian);
            float yPos = radius * Mathf.Sin(radian);

            topicRect.localPosition = new Vector2(xPos, yPos);

            // 记录放置位置
            placedTopicRects.Add(new Rect(
                xPos - topicWidth / 2,
                yPos - topicHeight / 2,
                topicWidth,
                topicHeight
            ));
        }
    }

    private bool IsRectOverlapping(float x1, float y1, float w1, float h1,
                                 float x2, float y2, float w2, float h2,
                                 float minSpacing)
    {
        return !(x1 + w1 + minSpacing < x2 ||
                 x2 + w2 + minSpacing < x1 ||
                 y1 + h1 + minSpacing < y2 ||
                 y2 + h2 + minSpacing < y1);
    }

    public Vector2 CalculateRandomPosition(Rect contentRect, Rect topicRect)
    {
        // 留出边距
        float margin = 20f;
        float minX = contentRect.xMin + topicRect.width / 2 + margin;
        float maxX = contentRect.xMax - topicRect.width / 2 - margin;
        float minY = contentRect.yMin + topicRect.height / 2 + margin;
        float maxY = contentRect.yMax - topicRect.height / 2 - margin;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    /// <summary>
    /// 滑动条置底
    /// </summary>
    public void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        messageSR.verticalNormalizedPosition = 0f;
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
        DialogueData dialogueData = LevelMgr.GetDialogueData(dialogueId);
        GameObject dialogueObj = Instantiate(dialogueData.isSelf ? SelfDialoguePrefab : OtherDialoguePrefab, messageSR.content);
        dialogueObj.GetComponent<DialogueEntity>().Init(dialogueData);

        ScrollToBottom();
    }

    /// <summary>
    /// 发送话题
    /// </summary>
    public IEnumerator SendTopic(string topicId)
    {
        TopicData topicData = LevelMgr.GetTopic(topicId);

        foreach (Transform topicObj in SendContent)
        {
            topicObj.gameObject.SetActive(false);
        }

        foreach (var dialogueId in topicData.dialogueIds)
        {
            SendDialogue(dialogueId);
            yield return new WaitForSeconds(0.5f);
        }

        foreach (Transform topicObj in SendContent)
        {
            topicObj.gameObject.SetActive(true);
        }
    }

    public void DeleteDialogue(string dialogueId)
    {
        if (remainingDeleteCount > 0)
        {
            remainingDeleteCount--;
            foreach (Transform child in messageSR.content)
            {
                DialogueEntity dialogue = child.GetComponent<DialogueEntity>();
                if (dialogue.dialogueId == dialogueId)
                {
                    child.SetParent(null);
                    Destroy(child.gameObject);
                    break;
                }
            }
            remainingDeleteCountText.text = remainingDeleteCount.ToString();
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
    public void ValidateOrder()
    {
        List<string> dialogueIds = new List<string>();
        foreach (Transform child in messageSR.content)
        {
            DialogueEntity dialogue = child.GetComponent<DialogueEntity>();
            dialogueIds.Add(dialogue.dialogueId);
        }

        // 首先检查长度是否相同
        if (dialogueIds.Count != currentLevel.correctOrder.Count)
        {
#if UNITY_EDITOR
            Debug.Log("对话顺序错误: 长度不相同");
#endif
            GameMgr.Instance.LoseGame();
            return;
        }

        // 逐个比较元素
        bool isCorrect = true;
        for (int i = 0; i < dialogueIds.Count; i++)
        {
            if (dialogueIds[i] != currentLevel.correctOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
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
        ValidateOrder();
    }

    public void OnMsgBarBtn()
    {
        RectTransform msgRT = messageSR.GetComponent<RectTransform>();
        if (isSendMsgShow)
        {
            sendBg.anchoredPosition = new Vector2(sendBg.anchoredPosition.x, sendBg.anchoredPosition.y - 392f);
            msgRT.sizeDelta = new Vector2(msgRT.sizeDelta.x, msgRT.sizeDelta.y + 392f);
        }
        else
        {
            sendBg.anchoredPosition = new Vector2(sendBg.anchoredPosition.x, sendBg.anchoredPosition.y + 392f);
            msgRT.sizeDelta = new Vector2(msgRT.sizeDelta.x, msgRT.sizeDelta.y - 392f);
        }
        ScrollToBottom();
        isSendMsgShow = !isSendMsgShow;
    }

}