using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class DialogDemo : MonoBehaviour
    {
        public DialogManager dialog;

        // Start is called before the first frame update
        void Start()
        {
            // �{�^��1�̃_�C�A���O
            dialog.Popup("okDialog", Vector2.zero, "abcd", DialogManager.ButtonType.OK);

        }

        public void DialogOkBtnClick(string id)
        {
            print("DialogOkBtnClick: " + id);

            // �{�^��2�̃_�C�A���O
            dialog.Popup("yesNoDialog", Vector2.zero, "abcd", DialogManager.ButtonType.YesNo);
        }

        public void DialogYesBtnClick(string id)
        {
            print("DialogYesBtnClick: " + id);

            // Toast
            dialog.Popup("toast", Vector2.zero, "abcd", DialogManager.ButtonType.None);
        }

        public void DialogNoBtnClick(string id)
        {
            print("DialogNoBtnClick: " + id);

            // Toast
            dialog.Popup("toast", Vector2.zero, "abcd", DialogManager.ButtonType.None);
        }
    }
}
