using UnityEngine;
using System.Collections;

public class ShipMover : MonoBehaviour
{

	private Vector3 _primaryPosition = Vector3.zero;

	void Start ()
	{
		_primaryPosition = transform.position;
	}


	void Update ()
	{
		transform.position = _primaryPosition + new Vector3(0, Mathf.Sin(Time.time * 2f) * 0.2f, 0);
		transform.eulerAngles = new Vector3(0, 0, Mathf.Cos(-Time.time * 2f) * 5f);
	}
}
