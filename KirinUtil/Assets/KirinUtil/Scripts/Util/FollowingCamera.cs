using UnityEngine;

namespace KirinUtil {
	/// <summary>
	/// http://qiita.com/sakano/items/918c090f484c0610619d
    /// targetを中心にしてマウスで回転させることができる
	/// Cameraにattactして使う
	/// </summary>

	[ExecuteInEditMode, DisallowMultipleComponent]
    public class FollowingCamera : MonoBehaviour {
        public GameObject target; // an object to follow
        public Vector3 offset; // offset form the target object

        [SerializeField] private float distance = 4.0f; // distance from following object
        [SerializeField] private float polarAngle = 45.0f; // angle with y-axis
        [SerializeField] private float azimuthalAngle = 45.0f; // angle with x-axis

        [SerializeField] private float minDistance = 1.0f;
        [SerializeField] private float maxDistance = 20.0f;
        [SerializeField] private float minPolarAngle = 5.0f;
        [SerializeField] private float maxPolarAngle = 75.0f;
        [SerializeField] private float mouseXSensitivity = 5.0f;
        [SerializeField] private float mouseYSensitivity = 5.0f;
        [SerializeField] private float scrollSensitivity = 5.0f;

        void LateUpdate() {
            if (Input.GetMouseButton(0)) {
                updateAngle(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
            updateDistance(Input.GetAxis("Mouse ScrollWheel"));

            var lookAtPos = target.transform.position + offset;
            updatePosition(lookAtPos);
            transform.LookAt(lookAtPos);
        }

        void updateAngle(float x, float y) {
            x = azimuthalAngle - x * mouseXSensitivity;
            azimuthalAngle = Mathf.Repeat(x, 360);

            y = polarAngle + y * mouseYSensitivity;
            polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
        }

        void updateDistance(float scroll) {
            scroll = distance - scroll * scrollSensitivity;
            distance = Mathf.Clamp(scroll, minDistance, maxDistance);
        }

        void updatePosition(Vector3 lookAtPos) {
            var da = azimuthalAngle * Mathf.Deg2Rad;
            var dp = polarAngle * Mathf.Deg2Rad;
            transform.position = new Vector3(
                lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
                lookAtPos.y + distance * Mathf.Cos(dp),
                lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
        }
    }
}