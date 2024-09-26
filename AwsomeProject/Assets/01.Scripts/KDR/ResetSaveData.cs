using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResetSaveData : MonoBehaviour
{
    private string _localPath;

    private void Start()
    {
        _localPath = Path.Combine(Application.persistentDataPath, "SaveDatas/bins");

        File.SetAttributes(_localPath, FileAttributes.Normal); //���� �б� ���� ����

        if (Directory.Exists(_localPath)) //���� Ž��
        {
            Directory.Delete(_localPath, true);
        }
    }
}
