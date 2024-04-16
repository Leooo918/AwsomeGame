using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MyDialogSystem 
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager instance = null;

        private DialogStatus playerStatsus = null;      //ππ ¿˙¿Â¿Ã∂˚ µÓµÓ
        [SerializeField] private Dialog dialog = null;
        [SerializeField] private List<DialogSO> dialogSO = new List<DialogSO>();

        //[Space(16)]
        //[SerializeField]private RectTransform dialogBackground = null;

        public DialogStatus Status => playerStatsus;

        public bool isReadingDialog { get; private set; }


        private void Awake()
        {
            instance = this;
            playerStatsus = GetComponent<DialogStatus>();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartDialog();
            }
        }

        public void StartDialog()
        {
            if (dialogSO[playerStatsus.curProgress] != null)
            {
                isReadingDialog = true;
                dialog.gameObject.SetActive(true);
                dialog.Init(dialogSO[playerStatsus.curProgress]);
                dialog.StartDialog();
            }
        }

        public void StartDialog(int idx)
        {
            if (dialogSO[idx] != null)
            {
                isReadingDialog = true;
                dialog.gameObject.SetActive(true);
                dialog.Init(dialogSO[idx]);
                dialog.StartDialog();
            }
        }

        public void EndDialog()
        {
            dialog.StopCoroutine("WaitNextScript");
            dialog.StopCoroutine("ReadingLine");
            isReadingDialog = false;
        }
    }

}

