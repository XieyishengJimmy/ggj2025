using UnityEngine;
using UnityEngine.UI;

public class DialogueEntity : MonoBehaviour
{
    public string dialogueId;

    public void Init(DialogueData dialogueData)
    {
        this.dialogueId = dialogueData.id;
        GetComponentInChildren<Text>().text = dialogueData.content;
        GetComponentInChildren<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        ChatMgr.Instance.DeleteDialogue(dialogueId);
    }
}