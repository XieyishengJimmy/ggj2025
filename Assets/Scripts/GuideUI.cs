using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuideUI : MonoBehaviour,IPointerDownHandler
{
    public void OpenGuideUI()
    {
        this.gameObject.SetActive(true);
    }


    public void CloseGuideUI()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CloseGuideUI();
    }

}
