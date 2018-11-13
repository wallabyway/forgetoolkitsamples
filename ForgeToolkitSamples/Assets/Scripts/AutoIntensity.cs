using UnityEngine;

public class AutoIntensity : MonoBehaviour
{
	public Gradient nightDayColor;
	public AnimationCurve lightIntensity;
	public AnimationCurve ambientIntensity;
	public AnimationCurve atmosphericThickness;
	public ReflectionProbe reflections;

	Light mainLight;
	Material skyMat;
	float originalAtmosphereThickness;
	Quaternion lastRotation;

	void Reset()
	{
		reflections = GameObject.FindObjectOfType<ReflectionProbe>();
	}

	void Start () 
	{
		mainLight = GetComponent<Light>();
		skyMat = RenderSettings.skybox;
		originalAtmosphereThickness = skyMat.GetFloat("_AtmosphereThickness");
	}

	void Update () 
	{
		if (transform.rotation != lastRotation)
		{
			AdvanceTime();
			lastRotation = transform.rotation;
		}
	}

	public void AdvanceTime()
	{
		//only using Day values to cut in half
		float amount = Vector3.Dot(transform.forward, Vector3.down) * .5f;
		amount = Mathf.Clamp01( amount);

		mainLight.intensity = lightIntensity.Evaluate(amount);
		mainLight.color = nightDayColor.Evaluate(amount);

		skyMat.SetFloat("_AtmosphereThickness", atmosphericThickness.Evaluate(amount));

		RenderSettings.ambientLight = mainLight.color;
		RenderSettings.ambientIntensity = ambientIntensity.Evaluate(amount);

		reflections.RenderProbe();
	}

	void OnDisable()
	{
		skyMat.SetFloat("_AtmosphereThickness", originalAtmosphereThickness);
	}
}
