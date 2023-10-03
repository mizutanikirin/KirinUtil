using UnityEngine;
using System.Collections;

public class FogOfWarAlphaPositioner : MonoBehaviour
{
	public Transform dungeonCharacter;

	void Start ()
	{
	
	}

	void Update ()
	{
		transform.position = new Vector3(dungeonCharacter.position.x, dungeonCharacter.position.y, transform.position.z);
	}
}
