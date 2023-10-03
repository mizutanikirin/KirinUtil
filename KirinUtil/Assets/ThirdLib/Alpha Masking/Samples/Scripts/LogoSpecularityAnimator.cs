using UnityEngine;
using System.Collections;

public class LogoSpecularityAnimator : MonoBehaviour
{
	public float startX = 0;
	public float endX = 0;
	public float speed = 0;
	public float delay = 0;

	void Start ()
	{
		StartCoroutine(AnimateSpecularity());
	}

	void Update ()
	{
	
	}

	private IEnumerator AnimateSpecularity ()
	{
		while (true)
		{
			transform.position = new Vector3(startX, transform.position.y, transform.position.z);

			while (transform.position.x < endX)
			{
				transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
				yield return null;
			}

			yield return new WaitForSeconds(delay);
		}
	}
}
