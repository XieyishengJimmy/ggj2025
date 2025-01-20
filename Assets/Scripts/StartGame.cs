using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public RectTransform menuAndLogo;
    public Button playBtn;
    public Button makeListBtn;
    public Button exitBtn;

    public RectTransform makeListPanel;
    public Button makeListCloseBtn;

    private Vector2 menuAndLogoStartPos;
    private Vector2 makeListPanelStartPos;

    /*
    void Awake()
    {
        int height = Screen.currentResolution.height;

        if (height >= 2160) // 4K
        {
            Screen.SetResolution(3200, 1800, false);
        }
        else if (height >= 1800) // 3K
        {
            Screen.SetResolution(2560, 1440, false);
        }
        else if (height >= 1440) // 2K
        {
            Screen.SetResolution(2048, 1152, false);
        }
        else // 1080p
        {
            Screen.SetResolution(1600, 900, false);
        }
    }
    */


    // Start is called before the first frame update
    void Start()
    {
        GameMgr.Instance.Initialize();
        playBtn.onClick.AddListener(OnPlayGame);
        makeListBtn.onClick.AddListener(OnMakeList);
        exitBtn.onClick.AddListener(OnExitGame);
        makeListCloseBtn.onClick.AddListener(OnMakeListClose);
        Utils.InitGame = true;
        menuAndLogo.gameObject.SetActive(true);
        makeListPanel.gameObject.SetActive(false);

        menuAndLogoStartPos = menuAndLogo.anchoredPosition;
        makeListPanelStartPos = makeListPanel.anchoredPosition;

        // SceneManager.LoadScene("MainScene");
        GameMgr.Instance.PlayMusic(Resources.Load<AudioClip>("Audios/BGM"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayGame()
    {
        GameMgr.Instance.PlaySound("ButtonSFX");
        SceneManager.LoadScene("MainScene");
    }

    public void OnMakeList()
    {
        GameMgr.Instance.PlaySound("ButtonSFX");
        makeListPanel.anchoredPosition = new Vector2(0, makeListPanelStartPos.y);
        makeListPanel.gameObject.SetActive(true);

        Tween.UIAnchoredPosition(menuAndLogo, new Vector2(0, menuAndLogoStartPos.y), 0.5f).OnComplete(() =>
        {
            menuAndLogo.gameObject.SetActive(false);
        });
        Tween.UIAnchoredPosition(makeListPanel, makeListPanelStartPos, 0.5f);
    }

    public void OnMakeListClose()
    {
        GameMgr.Instance.PlaySound("ButtonSFX");
        menuAndLogo.anchoredPosition = new Vector2(0, menuAndLogoStartPos.y);
        menuAndLogo.gameObject.SetActive(true);
        Tween.UIAnchoredPosition(makeListPanel, new Vector2(0, makeListPanelStartPos.y), 0.5f).OnComplete(() =>
        {
            makeListPanel.gameObject.SetActive(false);
        });
        Tween.UIAnchoredPosition(menuAndLogo, menuAndLogoStartPos, 0.5f);
    }

    public void OnExitGame()
    {
        GameMgr.Instance.PlaySound("ButtonSFX");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
