using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueEntity : MonoBehaviour, IPointerClickHandler
{
    public Image avatar;
    public Transform dialogueBG;
    public Text dialogueText;
    public Image dialogueImg;

    private RectTransform rectTransform;

    [HideInInspector]
    public string dialogueId;

    public void Init(DialogueData dialogueData)
    {
        this.dialogueId = dialogueData.id;
        dialogueText.gameObject.SetActive(true);
        dialogueImg.gameObject.SetActive(false);
        dialogueText.text = dialogueData.content;

        if (!string.IsNullOrEmpty(dialogueData.image))
        {
            dialogueText.gameObject.SetActive(false);
            dialogueImg.gameObject.SetActive(true);

            Sprite sprite = Resources.Load<Sprite>("Images/" + dialogueData.image);
            dialogueImg.sprite = sprite;

            // 计算图片尺寸，保持宽高比
            float maxWidth = GameMgr.Instance.dialogueMaxWidth;
            float width = sprite.rect.width;
            float height = sprite.rect.height;

            if (width > maxWidth)
            {
                float scale = maxWidth / width;
                width = maxWidth;
                height *= scale;
            }

            dialogueImg.rectTransform.sizeDelta = new Vector2(width, height);
        }

        AdjustTextAndBackground();
    }

    /// <summary>
    /// 文本宽度检查和背景自适应
    /// </summary>
    private void AdjustTextAndBackground()
    {
        rectTransform = GetComponent<RectTransform>();
        float maxWidth = GameMgr.Instance.dialogueMaxWidth;

        RectTransform textRect = dialogueText.rectTransform;
        // 获取ContentSizeFitter组件
        ContentSizeFitter contentFitter = dialogueText.GetComponent<ContentSizeFitter>();

        // 计算文本宽度
        float textWidth = dialogueText.preferredWidth;

        if (textWidth > maxWidth)
        {
            // 如果超过最大宽度，关闭水平自适应并设置固定宽度
            if (contentFitter != null)
            {
                contentFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
            dialogueText.rectTransform.sizeDelta = new Vector2(maxWidth, dialogueText.rectTransform.sizeDelta.y);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);

        RectTransform bgRect = dialogueBG as RectTransform;

        if (dialogueImg.gameObject.activeSelf)
        {
            // 如果显示图片，根据图片大小调整背景
            RectTransform imgRect = dialogueImg.rectTransform;
            bgRect.sizeDelta = new Vector2(imgRect.sizeDelta.x + 16, imgRect.sizeDelta.y + 16);
            rectTransform.sizeDelta = bgRect.sizeDelta;
            return;
        }

        // 设置背景大小与文本大小一致
        if (bgRect != null)
        {
            bgRect.sizeDelta = new Vector2(textRect.sizeDelta.x + 16, textRect.sizeDelta.y + 16);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(bgRect);

        rectTransform.sizeDelta = new Vector2(bgRect.sizeDelta.x, bgRect.sizeDelta.y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ChatMgr.Instance.DeleteDialogue(dialogueId);
        }
    }
}