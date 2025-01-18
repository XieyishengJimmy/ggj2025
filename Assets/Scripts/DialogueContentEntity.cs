using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class DialogueContentEntity : MonoBehaviour
{
    public Transform dialogueBG;
    public Text dialogueText;
    public Sprite[] sprites;

    private float frameRate = 0.082f;
    // private bool isLoop = false;
    private Image image;

    public IEnumerator PlayAnimationCoroutine()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (sprites != null && sprites.Length > 1)
        {
            // 播放音效
            GameMgr.Instance.PlaySound("BubbleExploreSFX");

            //while (true)
            //{
            for (int i = 0; i < sprites.Length - 1; i++)
            {
                dialogueText.fontSize -= dialogueText.fontSize / 3;
                image.sprite = sprites[i];
                image.SetNativeSize();
                yield return new WaitForSeconds(frameRate);
            }

            // if (!isLoop)
            //break;
            //}
        }

    }

}