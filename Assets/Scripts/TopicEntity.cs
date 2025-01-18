using UnityEngine;
using UnityEngine.UI;

public class TopicEntity : MonoBehaviour
{
    public string topicId;

    public void Init(TopicData topicData)
    {
        this.topicId = topicData.id;
        GetComponentInChildren<Text>().text = topicData.content;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        ChatMgr.Instance.SelectTopic(topicId);
    }
}
