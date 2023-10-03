using UnityEngine;
using System.Collections;

public class DungeonCharacterAnimator : MonoBehaviour
{

	private float _initialRandomTime = 0;
	private float _randomSpeed = 0;

	void Start ()
	{
		_initialRandomTime = Random.Range(0f, 3f);
		_randomSpeed = Random.Range(1f, 1.7f);
	}

	void Update ()
	{
		float currentPositionTime = Time.time * _randomSpeed + _initialRandomTime;
		transform.position = new Vector3(Mathf.Cos(currentPositionTime) * 3f, transform.position.y, transform.position.z);
		if (currentPositionTime % (Mathf.PI * 2f) < Mathf.PI)
		{
			transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		}
		else
		{
			transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
		}
	}
}
