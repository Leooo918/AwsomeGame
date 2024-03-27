using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuUI : MonoBehaviour
{
    public void NewGame()
    {
        //SceneManager.LoadScene("어디로 가야하오");
        Debug.Log("어디론거는 가겠지");
    }

    public void LoadGame()
    {
        //SceneManager.LoadScene("어디로 가야하오2");
        Debug.Log("저장한곳으로 이동");
    }

    public void GameExit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
}
