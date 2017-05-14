using UnityEngine;
using System.Collections;

/// <summary>
/// ͨ��OnMouseDown��OnMouseUp�����û��ǲ����������˵������������ǽ����ڽ�����ק
/// ������Collider��GUIElement
/// ��Override OnMouseRealClick()
/// </summary>
public class RealClickListener : FLEventDispatcherMono
{
	#region Fields
	public static float maxDurationForClick = 0.6f;
	public static float maxMouseMovementForClick = 10;
	private static GUILayer hitTestMaskGUILayer;
	protected float _lastTimeMouseDown = 0;
	protected Vector3 _mousePosWhenDown = Vector3.zero;
	
	#endregion

	#region Properties
	public static GUILayer HitTestMaskGUILayer {
		get {
			if (hitTestMaskGUILayer == null) {
				Debug.Log ("RealClickListener.HitTestMaskGUILayer is set to MainCamera's GUILayer.");
				Camera guiCamera = Camera.main;
				hitTestMaskGUILayer = guiCamera.GetComponent<GUILayer> ();
			}
			return hitTestMaskGUILayer;
		}set {
			hitTestMaskGUILayer = value;
		}
	}
	#endregion

	#region Functions
	protected virtual void OnMouseRealClick ()
	{
		SendMessage ("OnMouseClick", SendMessageOptions.DontRequireReceiver);
		DispatchEvent (new FLMouseEvent (FLMouseEvent.CLICK));
	}

	protected virtual void OnMouseDown ()
	{
		// OnMouseDown will be triggered even the mono has been disabled.
		// I think the user wouldn't like the RealClick event is dispatched when disabled.
		if (!enabled)
			return;
		
		_lastTimeMouseDown = Time.time;
		_mousePosWhenDown = Input.mousePosition;
	}

	protected virtual void OnMouseUpAsButton ()
	{
		// OnMouseUpAsButton will be triggered even the mono has been disabled.
		// I think the user wouldn't like the RealClick event is dispatched when disabled.
		if (!enabled)
			return;
		
		if (GetComponent<GUITexture>() == null) {
			if (HitTestMaskGUILayer && HitTestMaskGUILayer.HitTest (Input.mousePosition) != null)
				return;
		}
		
		bool hasMouse = Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer;
		if (hasMouse) {
			float duration = Time.time - _lastTimeMouseDown;
			float movement = Vector3.Distance (Input.mousePosition, _mousePosWhenDown);
		
			if (duration < maxDurationForClick && movement < maxMouseMovementForClick)
				OnMouseRealClick ();
		} else
			OnMouseRealClick ();
		
	}
	#endregion
}
