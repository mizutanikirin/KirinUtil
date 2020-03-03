using UnityEngine;

public class BillBoard : MonoBehaviour {

    public GameObject targetCamera;
    private Quaternion adjustEuler;// = Quaternion.Euler(270, 0, 0);

    // Use this for initialization
    void Start() {
        adjustEuler = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        if (targetCamera != null) {
            transform.rotation = targetCamera.transform.rotation;
            transform.rotation *= adjustEuler;
        }
    }
}