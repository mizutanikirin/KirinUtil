using UnityEngine;
using System.Collections;

public class NightAndDayMaskAnimator : MonoBehaviour
{
	public float maxScale = 1;
	public float speed = 0;

	void Start ()
	{
		transform.localScale = new Vector3(0, 0, 0);
	}

	void Update ()
	{
		float scale = (Mathf.Sin(Time.time * speed - Mathf.PI * 0.5f) + 1f) * 0.5f * maxScale;
		transform.localScale = new Vector3(scale, scale, scale);
	}
}
