using UnityEngine;
using UnityEngine.XR;

public class VRHead : MonoBehaviour
{
	public float floorAdjustment;

	void Awake()
	{
		if (XRSettings.enabled && XRDevice.GetTrackingSpaceType() == TrackingSpaceType.RoomScale)
		{
			Vector3 pos = transform.position;
			pos.y = floorAdjustment;
			transform.position = pos;
		}

		if (XRSettings.enabled)
			InputTracking.Recenter();
	}
}
