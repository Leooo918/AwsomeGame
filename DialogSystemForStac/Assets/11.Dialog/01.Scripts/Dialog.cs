using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using DialogSystem;
using Cinemachine;

public class Dialog : MonoBehaviour
{
    private Transform canvas;
    private RectTransform dialogParent;
    private RectTransform bottomBar;
    private TextMeshProUGUI nameTxt;
    private TextMeshProUGUI detailsTxt;
    private GameObject characterImg;

    //private Transform timer;
    //private Image timerFill;
    //private TextMeshProUGUI timerTxt;

    //private bool isOptionSelectTimeEnd;
    //private float timerTime;
    //private float timerTimeDown;

    private DialogSO curDialog;
    private ScriptSO curScript;
    private string curTxt;
    private bool isReadingTxt = false;

    private bool isOptionSelected;
    [SerializeField] private float bottomBarMoveTime = 0.5f;

    private Coroutine readlineCoroutine;
    private Coroutine readtextCoroutine;
    private Coroutine waitNextScriptCorotine;
    private Coroutine cameraControllCoroutine;

    private bool isBottomBarReveal = false;

    private CameraSetting[] cameraSettings;


    private void Update()
    {
        //if (timerTimeDown > 0)
        //{
        //    timerTimeDown -= Time.deltaTime;
        //    timerTxt.SetText($"{Math.Round(timerTimeDown, 1)}s");
        //    timerFill.fillAmount = timerTimeDown / timerTime;
        //    if (timerTimeDown <= 0)
        //    {
        //        isOptionSelectTimeEnd = true;
        //        timerTimeDown = 0f;
        //    }
        //}

        if (isReadingTxt == true && GetInput())
        {
            //StartCoroutine("SkipLine");
        }
    }


    public void StartDialog()
    {
        curScript = curDialog.scriptList[0];

        if (readlineCoroutine != null)
            StopCoroutine(readlineCoroutine);

        readlineCoroutine = StartCoroutine("ReadSingleLine");
    }

    IEnumerator ReadSingleLine()
    {
        if (waitNextScriptCorotine != null)
            StopCoroutine(waitNextScriptCorotine);

        cameraSettings = curScript.cameraSettings;

        NoScriptSO n = curScript as NoScriptSO;
        OptionSO o = curScript as OptionSO;
        NormalScriptSO normal = curScript as NormalScriptSO;

        if (n || o)
        {
            if (isBottomBarReveal == true)
            {
                HideBottomBar();
                yield return new WaitForSeconds(bottomBarMoveTime);
            }
            //yield return StartCoroutine("SetBeforeAnimation");
            //yield return StartCoroutine("SetAfterAnimation");
        }
        if (normal)
        {
            if (isBottomBarReveal == false)
            {
                RevealBottomBar();
                yield return new WaitForSeconds(bottomBarMoveTime);
            }
            SetImage();
            nameTxt.SetText(normal.character.name);
            //yield return StartCoroutine("SetBeforeAnimation");
            if (readtextCoroutine != null)
                StopCoroutine(readtextCoroutine);

            readtextCoroutine = StartCoroutine("ReadTexts");
            yield return readtextCoroutine;
            //yield return StartCoroutine("SetAfterAnimation");
        }

        if (waitNextScriptCorotine != null)
            StopCoroutine(waitNextScriptCorotine);

        cameraControllCoroutine = StartCoroutine("CameraControllRoutine");
        yield return cameraControllCoroutine;

        waitNextScriptCorotine = StartCoroutine("WaitNextScript");
    }

    IEnumerator ReadTexts()
    {
        yield return null;
        isReadingTxt = true;

        NormalScriptSO normal = curScript as NormalScriptSO;

        for (int i = 0; i < normal.character.talkDetails.Length; i++)
        {
            curTxt += normal.character.talkDetails[i];
            detailsTxt.SetText(curTxt);

            if (normal.character.talkDetails[i] != ' ')
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator WaitNextScript()
    {
        if (readlineCoroutine != null)
        {
            StopCoroutine(readlineCoroutine);
            readlineCoroutine = null;
        }

        isReadingTxt = false;

        NoScriptSO n = curScript as NoScriptSO;
        OptionSO o = curScript as OptionSO;
        NormalScriptSO normal = curScript as NormalScriptSO;

        if (n)
        {
            yield return new WaitUntil(() => GetInput());

            curScript = n.nextScript;
        }
        else if (normal)
        {
            yield return new WaitUntil(() => GetInput());

            curScript = normal.nextScript;
            Destroy(characterImg);
        }
        else if (o)
        {
            SetOption();

            yield return new WaitUntil(() => isOptionSelected == true); //|| isOptionSelectTimeEnd == true);
            isOptionSelected = false;
        }

        yield return null;

        if (readlineCoroutine != null)
            StopCoroutine(readlineCoroutine);
        if (readtextCoroutine != null)
            StopCoroutine(readtextCoroutine);

        if (curScript != null)
        {
            if (readlineCoroutine != null)
                StopCoroutine(readlineCoroutine);

            readlineCoroutine = StartCoroutine("ReadSingleLine");
        }
        else
            OnEndDialog();
    }

    IEnumerator CameraControllRoutine()
    {
        for (int i = 0; i < cameraSettings.Length; i++)
        {
            yield return new WaitForSeconds(cameraSettings[i].delayBeforeStart);

            if (cameraSettings[i].panCamera)
            {
                Debug.Log("밍미임이미임아ㅣ미ㅏ민ㅇ림ㄴㅇ리ㅏ미ㅣㅁ읾ㅇ");
                CameraManager.Instance.PanCameraOnContact(cameraSettings[i].panDistance, cameraSettings[i].panTime, cameraSettings[i].panDirection, cameraSettings[i].returnToStartPos);

                yield return new WaitForSeconds(cameraSettings[i].panTime);
            }
            else if (cameraSettings[i].swapCameras)
            {
                CinemachineVirtualCamera vCam = GameObject.Find(cameraSettings[i].changeCameraName).GetComponent<CinemachineVirtualCamera>();
                CameraManager.Instance.ChangeCam(vCam);
            }
        }
    }

    IEnumerator SkipLine()
    {
        StopCoroutine("ReadSingleLine");
        //StopCoroutine("SetBeforeAnimation");
        StopCoroutine("ReadTexts");
        //StopCoroutine("SetAfterAnimation");

        NormalScriptSO normal = curScript as NormalScriptSO;
        curTxt = normal.character.talkDetails;
        detailsTxt.SetText(curTxt);

        //StartCoroutine("SetAfterAnimation");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("WaitNextScript");
    }

    private void HideBottomBar()
    {
        bottomBar.DOAnchorPosX(-1920f, bottomBarMoveTime)
        .OnComplete(() =>
         {
             curTxt = "";
             nameTxt.SetText("");
             detailsTxt.SetText("");
             isBottomBarReveal = false;
         });
    }

    private void RevealBottomBar()
    {
        curTxt = "";
        nameTxt.SetText("");
        detailsTxt.SetText("");

        bottomBar.DOAnchorPosX(0f, bottomBarMoveTime)
            .OnComplete(() => isBottomBarReveal = true);
    }

    private void SetImage()
    {
        NormalScriptSO normal = curScript as NormalScriptSO;

        if (normal && normal.character.imgPrefab != null)
        {
            characterImg = Instantiate(normal.character.imgPrefab, dialogParent);
            characterImg.transform.SetSiblingIndex(0);                              //캐릭터는 대화창 뒤에 있어야 도미
        }
    }

    private void SetOption()
    {
        OptionSO option = curScript as OptionSO;

        //if (option.doTimer == true) //시간 제한을 둔다면
        //{
        //    //타이머를 켜라!
        //    timer.gameObject.SetActive(true);
        //    timerTime = option.selectTime;
        //    timerTimeDown = timerTime;
        //    isOptionSelectTimeEnd = false;
        //}

        for (int i = option.options.Count - 1; i >= 0; i--)
        {
            Button optionBtn = Instantiate(option.optionPf, dialogParent.Find("Options")).GetComponent<Button>();
            optionBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(option.options[i].detail);

            int index = i;

            optionBtn.onClick.AddListener(() =>
            {
                for (int i = 0; i < dialogParent.Find("Options").childCount; i++)
                {
                    Destroy(dialogParent.Find("Options").GetChild(i).gameObject);
                }
                curScript = option.options[index].nextScript;
                isOptionSelected = true;
            });
        }
    }

    private bool GetInput()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
    }

    private void OnEndDialog()
    {
        Destroy(dialogParent.gameObject);
        DialogManager.instance.EndDialog();
    }

    public void Init(DialogSO dialog)
    {
        canvas = GameObject.Find("Canvas").transform;   //캔버스 이름 바꾸면 얘도 바꿔줘
        curDialog = dialog;

        dialogParent = Instantiate(dialog.dialogBackground, canvas).GetComponent<RectTransform>();
        bottomBar = dialogParent.transform.Find("BottomBar").GetComponent<RectTransform>();
        nameTxt = bottomBar.Find("NameTxt").GetComponent<TextMeshProUGUI>();
        detailsTxt = bottomBar.Find("Details").GetComponent<TextMeshProUGUI>();

        //timer = dialogParent.Find("Timer");
        //timerFill = timer.Find("Fill").GetComponent<Image>();
        //timerTxt = timer.Find("Time").GetComponent<TextMeshProUGUI>();

        //if(curDialog.canSkip == true)
        //{
        //    dialogParent.transform.Find("SkipBtn").GetComponent<Button>().onClick.AddListener(() =>
        //    {
        //        OnEndDialog();
        //    });
        //}
        //else
        //{
        //    dialogParent.transform.Find("SkipBtn").gameObject.SetActive(false);
        //}
    }
}
