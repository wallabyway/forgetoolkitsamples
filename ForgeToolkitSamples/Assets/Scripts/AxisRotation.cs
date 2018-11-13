using UnityEngine;

public class AxisRotation : MonoBehaviour
{
	public Vector3 percentagePerAxis;
	public float rotationSpeed;
	
	
	void Update ()
	{
		transform.Rotate(percentagePerAxis * rotationSpeed * Time.deltaTime);	
	}
}
