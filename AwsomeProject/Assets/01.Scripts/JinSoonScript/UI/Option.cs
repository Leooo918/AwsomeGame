using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Option : MonoBehaviour, IManageableUI
{
    private string _path;
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [Space(10)]
    [SerializeField] private RectTransform _optionRect;
    [SerializeField] private float _easeingDelay = 0.5f;
    [SerializeField] private Vector2 _openOffset;
    [SerializeField] private Vector2 _closeOffset;
    private Tween tween;

    private float _master;
    private float _bgm;
    private float _sfx;

    public bool isOpened = false;

    private void Awake()
    {
        _path = Path.Combine(Application.dataPath, "Option.txt");

        _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChange);
        _bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChange);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChange);
    }
    public void OnMasterVolumeChange(float value)
    {
        _master = value;
        _mixer.SetFloat("Master", value);
    }
    public void OnBGMVolumeChange(float value)
    {
        _bgm = value;
        _mixer.SetFloat("BGM", value);
    }
    public void OnSFXVolumeChange(float value)
    {
        _sfx = value;
        _mixer.SetFloat("SFX", value);
    }

    public void GoToTitle()
    {
        GameManager.Instance.GoToTitle();
    }

    public void RestartGame()
    {
        GameManager.Instance.Restart();
    }
    public void Open()
    {
        if (tween != null && tween.active)
            tween.Kill();

        tween = _optionRect.DOAnchorPos(_openOffset, _easeingDelay)
            .OnComplete(() => Time.timeScale = 0f);
        isOpened = true;
    }
    public void Close()
    {
        Time.timeScale = 1f;
        if (tween != null && tween.active)
            tween.Kill();

        tween = _optionRect.DOAnchorPos(_closeOffset, _easeingDelay);
        isOpened = false;
    }

    public void Save()
    {
        string str = $"{_master},{_bgm},{_sfx}";
        File.WriteAllText(_path, str);
    }

    public void Load()
    {
        string str = File.ReadAllText(_path);
        string[] s = str.Split(',');
        OnMasterVolumeChange(float.Parse(s[0]));
        OnBGMVolumeChange(float.Parse(s[1]));
        OnSFXVolumeChange(float.Parse(s[2]));
    }

    public void Init()
    {
        _optionRect.anchoredPosition = _closeOffset;
    }
}
