using Autodesk.Forge.ForgeToolkit;
using UnityEngine;

public class UIDataPanel : MonoBehaviour
{
	void Start()
	{
		var prop = GetComponentInParent<ForgeProperties>();
		var panel = GetComponent<ForgePropertiesPanel>();
		panel.LoadProperties(prop, transform.position, -transform.forward);
	}
}
