using UnityEngine;

public class WayMoreSatisfyingExploder : MonoBehaviour
{
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
			foreach (Rigidbody rb in bodies)
			{
				rb.isKinematic = false;
				rb.AddExplosionForce(10f, transform.position, 5f, 1f, ForceMode.Impulse);
			}
		}
	}
}
