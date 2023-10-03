using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KirinUtil.Demo
{
    public class DragSample : MonoBehaviour
    {
        [SerializeField] private UIDragManager dragManager;
        [SerializeField] private GameObject addDragObj;
        [SerializeField] private GameObject addAdsorptionObj;

        // Start is called before the first frame update
        void Start()
        {
            dragManager.StartDrag();
        }


        //----------------------------------
        //  event
        //----------------------------------
        public void Draged(GameObject dragObj)
        {
            print("[Draged] " + dragObj.name + ": " + dragObj.transform.localPosition);
        }

        public void Fit(GameObject fitObj, GameObject adsorptionObj)
        {
            print("[Fit] " + fitObj.name + ": " + adsorptionObj.name);
        }

        public void Clicked(GameObject clickObj)
        {
            print("[Clicked] " + clickObj.name + ": " + clickObj.transform.localPosition);
        }


        //----------------------------------
        //  btn
        //----------------------------------
        public void AddDrag()
        {
            dragManager.AddDrag(addDragObj);
            addDragObj.GetComponent<Image>().color = new Color(0.4556843f, 0.8584906f, 0.1417319f);
        }

        public void RemoveDrag()
        {
            dragManager.RemoveDrag(addDragObj);
            addDragObj.GetComponent<Image>().color = new Color(0.9411765f, 0.9411765f, 0.9411765f);
        }

        public void AddAdsorption()
        {
            dragManager.AddAdsorption(addAdsorptionObj);
            addAdsorptionObj.GetComponent<Image>().color = new Color(0.6784314f, 0.6784314f, 0.6784314f);
        }

        public void RemoveAdsorption()
        {
            dragManager.RemoveAdsorption(addAdsorptionObj);
            addAdsorptionObj.GetComponent<Image>().color = new Color(0.9411765f, 0.9411765f, 0.9411765f);
        }

    }
}
