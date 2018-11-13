using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
	public Animator doorAnim;

	private void OnTriggerEnter(Collider other)
	{
		doorAnim.SetBool("InDoorway", true);
	}

	private void OnTriggerExit(Collider other)
	{
		doorAnim.SetBool("InDoorway", false);
	}
}
