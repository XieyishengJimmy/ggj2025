using UnityEngine;
using UnityEngine.UI;

public class TopicEntity : MonoBehaviour
{
    [HideInInspector]
    public string topicId;
    private Button self;

    public void Init(TopicData topicData)
    {
        this.topicId = topicData.id;
        GetComponentInChildren<Text>().text = topicData.content;
        self = GetComponent<Button>();
        self.onClick.AddListener(OnClick);
        self.interactable = true;
    }

    public void OnClick()
    {
        self.interactable = false;
        ChatMgr.Instance.SelectTopic(topicId);
    }
}
