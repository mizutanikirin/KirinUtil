using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class UIDemo : MonoBehaviour
    {
        // UILine
        [SerializeField] private GameObject lineStartPosObj;
        [SerializeField] private GameObject lineEndPosObj;
        [SerializeField] private UILine line;

        // Start is called before the first frame update
        void Start()
        {
            // 線を引く
            line.Draw(lineStartPosObj.transform.localPosition, lineEndPosObj.transform.localPosition, 5);
        }

        // Update is called once per frame
        void Update()
        {

        }




        //----------------------------------
        //  ToggleButton
        //----------------------------------
        #region ToggleButton
        public void ToggleOn()
        {
            print("ToggleOn");
        }

        public void ToggleOff()
        {
            print("ToggleOff");
        }
        #endregion
    }
}