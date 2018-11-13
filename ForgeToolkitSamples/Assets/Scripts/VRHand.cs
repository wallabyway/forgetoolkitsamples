using UnityEngine;
using UnityEngine.XR;

public class VRHand : MonoBehaviour
{
	public XRNode node;

	Vector3 lastPosition;
	Quaternion lastRotation;

	void Start ()
	{
		if (!UnityEngine.XR.XRSettings.enabled)
			Destroy(gameObject);
	}
	
	void Update ()
	{
		lastPosition = transform.localPosition;
		lastRotation = transform.rotation;
		transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(node);
		transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(node);
	}

	public Vector3 GetDeltaPosition()
	{
		return transform.localPosition - lastPosition;
	}

	public float GetDeltaRotation()
	{
		//return transform.localRotation * Quaternion.Inverse(lastRotation);
		return Quaternion.Angle(transform.rotation, lastRotation);
	}

	public Quaternion GetDRot()
	{
		return Quaternion.FromToRotation(transform.rotation.eulerAngles, lastRotation.eulerAngles);
	}
}
