using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    public GameObject self;

    public Button backInGame;
    public Button backStartScene;

    void Awake()
    {
        backInGame.onClick.AddListener(OnBackInGame);
        backStartScene.onClick.AddListener(OnBackStartScene);
        self.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Show();
        }
    }

    public void Show()
    {
        self.SetActive(true);
    }

    public void OnBackInGame()
    {
        self.SetActive(false);
    }

    public void OnBackStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
