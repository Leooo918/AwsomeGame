using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuUI : MonoBehaviour
{
    public void NewGame()
    {
        //SceneManager.LoadScene("���� �����Ͽ�");
        Debug.Log("���аŴ� ������");
    }

    public void LoadGame()
    {
        //SceneManager.LoadScene("���� �����Ͽ�2");
        Debug.Log("�����Ѱ����� �̵�");
    }

    public void GameExit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
}
