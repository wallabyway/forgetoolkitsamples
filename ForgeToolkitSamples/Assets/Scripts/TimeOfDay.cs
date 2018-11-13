using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
	[SerializeField] string inputName = "RightOVRTrigger";

	bool isHolding;
	Transform sunTransform;

	void Update ()
	{
		if (Input.GetAxis(inputName) <= 0f && sunTransform != null)
		{
			sunTransform.parent = null;
			sunTransform = null;
			return;
		}

		if (Input.GetAxis(inputName) > 0f)
		{
			if (sunTransform != null)
				return;

			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit[] hits = Physics.RaycastAll(ray, 100);

			foreach (RaycastHit hit in hits)
			{
				if (hit.transform.CompareTag("Sun"))
				{
					sunTransform = hit.transform;
					sunTransform.parent = transform;
				}
			}
		}
	}
}
