using System.Collections;
//--------------------------------------------------------------------------
//
//  setActive(false)時とDestroy時にtweenをストップする。
//
//--------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

public class StopTween : MonoBehaviour
{

    private void OnDisable() {
        iTween.Stop(gameObject);
    }

    private void OnDestroy() {
        iTween.Stop(gameObject);
    }
}
