using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueEntity : MonoBehaviour, IPointerClickHandler
{
    public Image avatar;
    public Transform dialogueContainer;
    public Image dialogueImg;

    private RectTransform rectTransform;
    private DialogueContentEntity dialogueContent;

    [HideInInspector]
    public string dialogueId;

    public void Init(DialogueData dialogueData)
    {
        this.dialogueId = dialogueData.id;

        // 确保DialogueContentEntity已实例化
        if (dialogueContent == null)
        {
            GameObject contentObj;
            if (dialogueData.isSelf)
            {
                if (dialogueData.content.Length <= 5)
                {
                    contentObj = Instantiate(Resources.Load<GameObject>("Prefabs/RightDialogueS"), dialogueContainer);
                }
                else if (dialogueData.content.Length > 5 && dialogueData.content.Length <= 10)
                {
                    contentObj = Instantiate(Resources.Load<GameObject>("Prefabs/RightDialogueM"), dialogueContainer);
                }
                else
                {
                    contentObj = Instantiate(Resources.Load<GameObject>("Prefabs/RightDialogueL"), dialogueContainer);
                }
            }
            else
            {
                if (dialogueData.content.Length <= 5)
                {
                    contentObj = Instantiate(Resources.Load<GameObject>("Prefabs/LeftDialogueS"), dialogueContainer);
                }
                else if (dialogueData.content.Length > 5 && dialogueData.content.Length <= 10)
                {
                    contentObj = Instantiate(Resources.Load<GameObject>("Prefabs/LeftDialogueM"), dialogueContainer);
                }
                else
                {
                    contentObj = Instantiate(Resources.Load<GameObject>("Prefabs/LeftDialogueL"), dialogueContainer);
                }
            }

            dialogueContent = contentObj.GetComponent<DialogueContentEntity>();

            if (!string.IsNullOrEmpty(dialogueData.image))
            {
                dialogueContent.gameObject.SetActive(false);
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
            else
            {
                dialogueContent.gameObject.SetActive(true);
                dialogueImg.gameObject.SetActive(false);
                dialogueContent.dialogueText.text = dialogueData.content;
            }

            AdjustTextAndBackground();
        }
    }

    /// <summary>
    /// 文本宽度检查和背景自适应
    /// </summary>
    private void AdjustTextAndBackground()
    {
        rectTransform = GetComponent<RectTransform>();
        RectTransform dcRT = dialogueContainer.GetComponent<RectTransform>();

        if (dialogueImg.gameObject.activeSelf)
        {
            // 如果显示图片，根据图片大小调整背景
            RectTransform imgRect = dialogueImg.rectTransform;
            dcRT.sizeDelta = new Vector2(imgRect.sizeDelta.x + 16, imgRect.sizeDelta.y + 16);
            rectTransform.sizeDelta = new Vector2(imgRect.sizeDelta.x + 16, imgRect.sizeDelta.y + 16);
        }
        else
        {
            // 如果显示文本，调用DialogueContentEntity中的方法处理大小
            dcRT.sizeDelta = dialogueContent.GetComponent<RectTransform>().sizeDelta;
            rectTransform.sizeDelta = dialogueContent.GetComponent<RectTransform>().sizeDelta;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ChatMgr.Instance.DeleteDialogue(dialogueId);
        }
    }
}