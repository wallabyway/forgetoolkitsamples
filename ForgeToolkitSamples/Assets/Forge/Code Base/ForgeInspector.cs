﻿//
// Copyright (c) Autodesk, Inc. All rights reserved.
// 
// This computer source code and related instructions and comments are the
// unpublished confidential and proprietary information of Autodesk, Inc.
// and are protected under Federal copyright and state trade secret law.
// They may not be disclosed to, copied or used by any third party without
// the prior written consent of Autodesk, Inc.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if FORGE_HUX
using HoloToolkit.Unity.InputModule;
using HUX.Receivers;
using HUX.Interaction;
using HUX.Focus;
#else
using UnityEngine.EventSystems;
#endif


namespace Autodesk.Forge.ForgeToolkit {

	#if FORGE_HUX
	using __InteractionReceiver__ =InteractionReceiver ;
	using __IPointerClickHandler__ =IEmptyInterface ;
	#else
	using __InteractionReceiver__ =MonoBehaviour ;
	using __IPointerClickHandler__ =IPointerClickHandler ;
	#endif

	public class ForgeInspector : __InteractionReceiver__, __IPointerClickHandler__ {

		#region Fields
		#if !FORGE_HUX
		[Tooltip ("Target Interactible Object to receive events for")]
		public List<GameObject> Interactibles =new List<GameObject> () ;
		#endif
		[LabelOverride ("Property Panel Template Prefab")]
		public GameObject _propertyPanel = null ;
		public LayerMask _interactiableLayers = ~0;
		protected Dictionary<GameObject, GameObject> _propertyPanels =new Dictionary<GameObject, GameObject> () ;
		protected Camera _mainCamera;

		#endregion

		#region Unity APIs
		protected void Start () {
			_mainCamera = Camera.main; //Cache the call to improve performance
			RefreshInteractiblesList () ;
		}

		//#if !FORGE_HUX
		//void Update () {
		//	if ( Input.GetMouseButtonDown (0) ) // if left button pressed...
		//		OnTapped () ;
		//}
		//#endif

		#endregion

		#region InteractionReceiver interface
		#if FORGE_HUX
		protected override void OnTapped (GameObject obj, InteractionManager.InteractionEventArgs eventArgs) {
			ForgeProperties properties =obj.GetComponentInParent<ForgeProperties> () ;
			if ( properties == null )
				return ;

			GameObject propertyPanel =null ;
			if ( _propertyPanels.ContainsKey (obj) ) {
				propertyPanel =_propertyPanels [obj] ;
			} else {
				propertyPanel =GameObject.Instantiate (_propertyPanel) ;
				_propertyPanels.Add (obj, propertyPanel) ;
			}

			Vector3 position =eventArgs.GazeRay.GetPoint (2f) ; //eventArgs.Position ;
			Vector3 normal =-eventArgs.GazeRay.direction.normalized ;
			RaycastHit info ;
			int layerMask =LayerMask.NameToLayer ("UI") ;
			if ( Physics.Raycast (eventArgs.GazeRay, out info, 5f, layerMask) ) {
				position =info.point - 0.3f * eventArgs.GazeRay.direction.normalized ;
				normal =info.normal ;
			}

			propertyPanel.GetComponent<ForgePropertiesPanel> ().LoadProperties (
				properties, position, normal
			) ;
		}

		protected override void OnHoldStarted (GameObject obj, InteractionManager.InteractionEventArgs eventArgs) {
			base.OnHoldStarted (obj, eventArgs) ;
		}

		protected override void OnFocusEnter (GameObject obj, FocusArgs args) {
			base.OnFocusEnter (obj, args) ;
		}

		protected override void OnFocusExit (GameObject obj, FocusArgs args) {
			base.OnFocusExit (obj, args) ;
		}
		#else
		protected void OnTapped () {
			Ray GazeRay = _mainCamera.ScreenPointToRay (Input.mousePosition) ;
			RaycastHit info ;

			if ( !Physics.Raycast (GazeRay, out info, 100, _interactiableLayers) )
				return ;

			GameObject obj = info.transform.gameObject ;
			if ( !Interactibles.Contains (obj) )
				return ;

			ForgeProperties properties =obj.GetComponentInParent<ForgeProperties> () ;
			if ( properties == null )
				return ;

			GameObject propertyPanel =null ;
			if ( _propertyPanels.ContainsKey (obj) ) {
				propertyPanel =_propertyPanels [obj] ;
			} else {				
				propertyPanel =GameObject.Instantiate (_propertyPanel, transform) ;
				_propertyPanels.Add (obj, propertyPanel) ;

				//temp fix
				if (transform.localScale.x < 0){
					Vector3 scale = propertyPanel.transform.localScale;
					scale.x *= -1;
					propertyPanel.transform.localScale = scale;
				}
			}

			Vector3 position =info.point - 0.3f * GazeRay.direction.normalized ;
			Vector3 normal =info.normal ;

			propertyPanel.GetComponent<ForgePropertiesPanel> ().LoadProperties (
				properties, position, normal
			) ;
		}

		public void OnPointerClick (PointerEventData eventData) {
			if(eventData.button == PointerEventData.InputButton.Left)
				OnTapped () ;
		}
		#endif

		#endregion

		#region Methods
		public void RefreshInteractiblesList () {
			Interactibles.Clear () ;
			MeshRenderer[] renders =gameObject.GetComponentsInChildren<MeshRenderer> () ;
			foreach ( MeshRenderer rd in renders ) {
				if ( !Interactibles.Contains (rd.gameObject) )
					Interactibles.Add (rd.gameObject) ;
			}
		}

		#endregion

	}

}