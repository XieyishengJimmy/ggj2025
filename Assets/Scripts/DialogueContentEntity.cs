using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueContentEntity : MonoBehaviour
{
    public Transform dialogueBG;
    public Text dialogueText;
    public Sprite[] sprites;

    private float frameRate = 0.1f;
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
            //while (true)
            //{
            for (int i = 0; i < sprites.Length - 1; i++)
            {
                image.sprite = sprites[i];
                yield return new WaitForSeconds(frameRate);
            }

            // if (!isLoop)
            //break;
            //}
        }

    }

}