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
    public Transform SendContent;
    public GameObject TopicPrefab;
    public GameObject OtherDialoguePrefab;
    public GameObject SelfDialoguePrefab;
    public Text remainingDeleteCountText;
    public Button restartBtn;
    public Text targetText;

    private LevelData currentLevel;
    private int remainingDeleteCount;

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
        InitTopics(currentLevel.topicIds);
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
    /// 选择话题
    /// </summary>
    public void SelectTopic(string topicId)
    {
        StartCoroutine(SendTopic(topicId));
    }

    public void SendDialogue(string dialogueId)
    {
        DialogueData dialogueData = LevelMgr.GetDialogueData(dialogueId);
        GameObject dialogueObj = Instantiate(dialogueData.isSelf ? SelfDialoguePrefab : OtherDialoguePrefab, messageSR.content);
        dialogueObj.GetComponent<DialogueEntity>().Init(dialogueData);

        // 滑动条置底
        Canvas.ForceUpdateCanvases();
        messageSR.verticalNormalizedPosition = 0f;
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
#if UNITY_EDITOR
            Debug.Log("对话顺序: " + dialogue.dialogueId);
#endif
        }

#if UNITY_EDITOR
        for (int i = 0; i < dialogueIds.Count; i++)
        {
            Debug.Log("对话顺序: " + dialogueIds[i] + "|" + currentLevel.correctOrder[i]);
        }
#endif

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

}