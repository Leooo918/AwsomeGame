using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyDialogSystem
{
    public class Dialog : MonoBehaviour
    {
        private Transform canvas;
        private RectTransform dialogParent;
        private RectTransform bottomBar;
        private TextMeshProUGUI nameTxt;
        private TextMeshProUGUI detailsTxt;
        private Image background;


        private Transform timer;
        private Image timerFill;
        private TextMeshProUGUI timerTxt;


        private bool isOptionSelectTimeEnd;
        private float timerTime;
        private float timerTimeDown;


        private DialogSO curDialog;
        private ScriptSO curScript;
        private List<DialogCharactor> charactors = new List<DialogCharactor>();
        private string curTxt;
        private bool isReadingTxt = false;


        private bool isOptionSelected;


        private void Update()
        {
            if (timerTimeDown > 0)
            {
                timerTimeDown -= Time.deltaTime;
                timerTxt.SetText($"{Math.Round(timerTimeDown, 1)}s");
                timerFill.fillAmount = timerTimeDown / timerTime;
                if (timerTimeDown <= 0)
                {
                    isOptionSelectTimeEnd = true;
                    timerTimeDown = 0f;
                }
            }

            if (isReadingTxt == true && GetInput())
            {
                StartCoroutine("SkipLine");
            }
        }


        public void StartDialog()
        {
            curScript = curDialog.scripts[0];
            StartCoroutine("ReadSingleLine");
        }

        IEnumerator ReadSingleLine()
        {
            SetImage();

            switch (curScript.scriptType)
            {
                case ScriptType.IMAGE:
                case ScriptType.OPTION:
                    HideBottomBar();

                    yield return StartCoroutine("SetBeforeAnimation");
                    yield return StartCoroutine("SetAfterAnimation");
                    break;
                case ScriptType.NORMAL:
                case ScriptType.BRANCH:
                    SetBottomBar();

                    yield return StartCoroutine("SetBeforeAnimation");
                    yield return StartCoroutine("ReadTexts");
                    yield return StartCoroutine("SetAfterAnimation");
                    break;
            }
            StartCoroutine("WaitNextScript");
        }

        IEnumerator SkipLine()
        {
            StopCoroutine("ReadSingleLine");
            StopCoroutine("SetBeforeAnimation");
            StopCoroutine("ReadTexts");
            StopCoroutine("SetAfterAnimation");

            curTxt = curScript.talkingDetails;
            detailsTxt.SetText(curTxt);

            //StartCoroutine("SetAfterAnimation");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine("WaitNextScript");
        }

        IEnumerator ReadTexts()
        {
            yield return null;
            isReadingTxt = true;

            for (int i = 0; i < curScript.talkingDetails.Length; i++)
            {
                curTxt += curScript.talkingDetails[i];
                detailsTxt.SetText(curTxt);

                if (curScript.talkingDetails[i] != ' ')
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        IEnumerator WaitNextScript()
        {
            Debug.Log("코코다요~");
            isReadingTxt = false;

            switch (curScript.scriptType)
            {
                case ScriptType.IMAGE:
                    yield return new WaitUntil(() => GetInput());

                    ImageSO i = curScript as ImageSO;
                    curScript = i.nextScript;

                    break;
                case ScriptType.NORMAL:

                    yield return new WaitUntil(() => GetInput());

                    NormalScriptSO n = curScript as NormalScriptSO;
                    curScript = n.nextScript;
                    Debug.Log("ㄷ");

                    break;
                case ScriptType.BRANCH:

                    yield return new WaitUntil(() => GetInput());

                    BranchSO b = curScript as BranchSO;

                    if (b.condition.Judge() == true) curScript = b.nextScriptOnTrue;
                    else curScript = b.nextScriptOnFalse;

                    break;
                case ScriptType.OPTION:

                    SetOption();

                    yield return new WaitUntil(() => isOptionSelected == true || isOptionSelectTimeEnd == true);

                    break;
            }

            yield return new WaitForSeconds(0.1f);

            DeleteCharactor();

            if (curScript != null)
            {
                StartCoroutine("ReadSingleLine");
            }
            else
            {
                OnEndDialog();
            }
        }


        IEnumerator SetBeforeAnimation()
        {
            foreach (DialogCharactor charactor in curScript.charactors)
            {
                GameObject c = Instantiate(charactor.charactor, background.transform);

                if (charactor.beforeReadAnimation != null)
                {
                    //CharactorAnimation anim = Instantiate(charactor.beforeReadAnimation).GetComponent<CharactorAnimation>();       //실행해라
                    //anim.Init(c.GetComponent<RectTransform>());
                    charactor.beforeReadAnimation.Init(c.GetComponent<RectTransform>());
                    charactor.beforeReadAnimation.Animation();

                }

                DialogCharactor d = new DialogCharactor(c, charactor.beforeReadAnimation, charactor.afterReadAnimation);
                charactors.Add(d);
            }

            foreach (DialogCharactor charactor in curScript.charactors)
            {
                if (charactor.beforeReadAnimation != null)
                {
                    yield return new WaitUntil(() => charactor.beforeReadAnimation.isAnimating == false);
                }
            }
        }

        IEnumerator SetAfterAnimation()
        {
            for (int i = 0; i < curScript.charactors.Count; i++)
            {
                if (charactors[i].afterReadAnimation != null)
                {
                    GameObject c = charactors[i].charactor;
                    //CharactorAnimation anim = Instantiate(curScript.charactors[i].afterReadAnimation).GetComponent<CharactorAnimation>();       //실행해라
                    //anim.Init(c.GetComponent<RectTransform>());
                    curScript.charactors[i].afterReadAnimation.Init(c.GetComponent<RectTransform>());
                    curScript.charactors[i].afterReadAnimation.Animation();
                }
            }

            foreach (DialogCharactor charactor in curScript.charactors)
            {
                if (charactor.afterReadAnimation != null)
                {
                    yield return new WaitUntil(() => charactor.afterReadAnimation.isAnimating == false);
                }
            }
        }


        private void SetImage()
        {
            if (background != null)
            {
                if (background.sprite == null)
                {
                    background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
                }
                else
                {
                    background.color = new Color(background.color.r, background.color.g, background.color.b, 255f);
                    background.sprite = curScript.background;
                }
            }
        }

        private void SetBottomBar()
        {
            bottomBar.DOAnchorPosY(-290f, 0.5f);
            curTxt = "";
            nameTxt.SetText(curScript.talkingPeopleName);
            detailsTxt.SetText(curTxt);
        }

        private void HideBottomBar()
        {
            bottomBar.DOAnchorPosY(-690f, 0.5f);
        }

        private void SetOption()
        {
            OptionSO option = curScript as OptionSO;

            if (option.doTimer == true) //시간 제한을 둔다면
            {
                //타이머를 켜라!
                timer.gameObject.SetActive(true);
                timerTime = option.selectTime;
                timerTimeDown = timerTime;
                isOptionSelectTimeEnd = false;
            }

            for (int i = option.options.Count - 1; i >= 0; i--)
            {
                Button optionBtn = Instantiate(option.optionPf, dialogParent.Find("Options")).GetComponent<Button>();
                optionBtn.transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(option.options[i]);

                int index = i;

                optionBtn.onClick.AddListener(() =>
                {
                    for (int i = 0; i < dialogParent.Find("Options").childCount; i++)
                    {
                        Destroy(dialogParent.Find("Options").GetChild(i).gameObject);
                    }
                    curScript = option.nextScriptsByOption[index];
                    isOptionSelected = true;
                });
            }
        }

        private void DeleteCharactor()
        {
            foreach (var g in charactors)
            {
                Destroy(g.charactor);
            }
            charactors.Clear();
        }

        private bool GetInput()
        {
            Debug.Log(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return));
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
            background = dialogParent.Find("Background").GetComponent<Image>();
            nameTxt = bottomBar.Find("NameTxt").GetComponent<TextMeshProUGUI>();
            detailsTxt = bottomBar.Find("Details").GetComponent<TextMeshProUGUI>();

            timer = dialogParent.Find("Timer");
            timerFill = timer.Find("Fill").GetComponent<Image>();
            timerTxt = timer.Find("Time").GetComponent<TextMeshProUGUI>();

            if (curDialog.canSkip == true)
            {
                dialogParent.transform.Find("SkipBtn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnEndDialog();
                });
            }
            else
            {
                dialogParent.transform.Find("SkipBtn").gameObject.SetActive(false);
            }
        }
    }
}

