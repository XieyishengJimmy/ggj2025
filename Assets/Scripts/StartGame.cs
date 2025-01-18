using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button playBtn;

    // Start is called before the first frame update
    void Start()
    {
        GameMgr.Instance.Initialize();
        playBtn.onClick.AddListener(OnPlayGame);
        Utils.InitGame = true;
        // SceneManager.LoadScene("MainScene");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
