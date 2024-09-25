using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [SerializeField] private Slider allSlider, bgmSlider, sfxSlider;

    private void Start()
    {
        allSlider.value = AudioManager.Instance.volumeSaveData.allVolume;
        bgmSlider.value = AudioManager.Instance.volumeSaveData.bgmVolume;
        sfxSlider.value = AudioManager.Instance.volumeSaveData.sfxVolume;
    }

    private void Update()
    {
        AudioManager.Instance.SetVolume(allSlider.value, bgmSlider.value, sfxSlider.value);
    }
}
