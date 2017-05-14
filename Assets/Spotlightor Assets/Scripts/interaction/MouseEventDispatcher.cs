using UnityEngine;
using System.Collections;

public class MouseEventDispatcher : RealClickListener {

	private bool mouseDownOnMe = false;
	protected virtual void OnMouseEnter ()
	{
		if (!enabled)
			return;
		
		DispatchEvent (new FLMouseEvent (FLMouseEvent.ROLL_OVER));
	}

	protected virtual void OnMouseExit ()
	{
		if (!enabled)
			return;
		this.mouseDownOnMe = false;
		DispatchEvent (new FLMouseEvent (FLMouseEvent.ROLL_OUT));
	}

	protected override void OnMouseDown ()
	{
		base.OnMouseDown ();
		
		if (!enabled)
			return;
		this.mouseDownOnMe = true;
		DispatchEvent (new FLMouseEvent (FLMouseEvent.DOWN));
	}

	protected override void OnMouseUpAsButton ()
	{
		base.OnMouseUpAsButton ();
		
		if (!enabled)
			return;
		this.mouseDownOnMe = false;
		DispatchEvent (new FLMouseEvent (FLMouseEvent.UP));
	}

	protected virtual void OnMouseOver ()
	{
		if (!enabled)
			return;
		
		if (Input.GetMouseButton (0) && this.mouseDownOnMe == true)
			DispatchEvent (new FLMouseEvent (FLMouseEvent.HOLD_DOWN));
	}
	
}
