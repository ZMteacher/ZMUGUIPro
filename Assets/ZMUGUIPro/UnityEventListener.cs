using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
namespace ZM.UGUIPro
{
public class UnityEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler,
IPointerExitHandler, IPointerUpHandler, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
{
	public delegate void VoidDelegate(GameObject go, PointerEventData data);
	public event VoidDelegate onPointerClick;
	public event VoidDelegate onPointerDown;
	public event VoidDelegate onPointerUp;
	public event VoidDelegate onPointerEnter;
	public event VoidDelegate onPointerExit;
	public event VoidDelegate onSelect;
	public event VoidDelegate onUpdateSelected;
	public event VoidDelegate onBeginDrag;
	public event VoidDelegate onCancel;
	public event VoidDelegate onDeselect;
	public event VoidDelegate onDrag;
	public event VoidDelegate onDrop;
	public event VoidDelegate onEndDrag;
	public event VoidDelegate onInitializePotentialDrag;
	public event VoidDelegate onMove;
	public event VoidDelegate onScroll;
	public event VoidDelegate onSubmit;
	public event VoidDelegate onToggleValueChange;
	public event VoidDelegate onToggleSelect;

	event EventCallBack onPointerClickCallBack;
	event EventCallBack onPointerDownCallBack;
	event EventCallBack onPointerUpCallBack;
	event EventCallBack onPointerEnterCallBack;
	event EventCallBack onPointerExitCallBack;
	event EventCallBack onSelectCallBack;
	event EventCallBack onUpdateSelectedCallBack;
	event EventCallBack onBeginDragCallBack;
	event EventCallBack onCancelCallBack;
	event EventCallBack onDeselectCallBack;
	event EventCallBack onDragCallBack;
	event EventCallBack onDropCallBack;
	event EventCallBack onEndDragCallBack;
	event EventCallBack onInitializePotentialDragCallBack;
	event EventCallBack onMoveCallBack;
	event EventCallBack onScrollCallBack;
	event EventCallBack onSubmitCallBack;
	event EventCallBack onToggleValueChangeCallBack;

	public event VoidDelegate onPointerRightClick;
	public event VoidDelegate onPointerRightDoubleClick;
	public event VoidDelegate onPointerLeftClick;
	public event VoidDelegate onPointerLeftDoubleClick;

	public delegate void EventCallBack();
	public delegate void EventsCallBack(ref int count);
	event EventCallBack awakeCallBack;
	event EventCallBack onApplicationPauseCallBack;
	event EventCallBack onApplicationFocusCallBack;
	event EventCallBack onApplicationQuitCallBack;
	event EventCallBack onBecameVisibleCallBack;
	event EventCallBack onBecameInVisibleCallBack;
	event EventCallBack onCollisionEnterCallBack;
	event EventCallBack onCollisionExitCallBack;
	event EventCallBack onCollisionStayCallBack;
	event EventCallBack onDestroyCallBack;
	event EventCallBack onDisableCallBack;
	event EventCallBack onEnableCallBack;
	event EventsCallBack onMouseDownCallBack;
	event EventCallBack onMouseEnterCallBack;
	event EventCallBack onMouseExitCallBack;
	event EventCallBack onMouseOverCallBack;
	event EventCallBack onMouseUpCallBack;
	event EventCallBack onTriggerEnterCallBack;
	event EventCallBack onTriggerExitCallBack;
	event EventCallBack onTriggerStayCallBack;
	event EventCallBack startCallBack;

	const int LeftMouseID = -1;
	const int RightMouseID = -2;
	const int CenterMouseID = -3;
	const int MouseClick = 1;
	const int DoubleMouseClick = 2;

	#region Get

	static public UnityEventListener Get(GameObject go)
	{
		UnityEventListener listener = go.GetComponent<UnityEventListener>();
		if (listener == null) listener = go.AddComponent<UnityEventListener>();
		return listener;
	}

	static public UnityEventListener Get(Transform go)
	{
		UnityEventListener listener = go.GetComponent<UnityEventListener>();
		if (listener == null) listener = go.gameObject.AddComponent<UnityEventListener>();
		return listener;
	}

	#endregion


	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		onPointerClickCallBack?.Invoke();
		onPointerClick?.Invoke(gameObject, eventData);

	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		onPointerDownCallBack?.Invoke();
		onPointerDown?.Invoke(gameObject, eventData);
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		onPointerUpCallBack?.Invoke();
		onPointerUp?.Invoke(gameObject, eventData);
	}
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		onPointerEnterCallBack?.Invoke();
		onPointerEnter?.Invoke(gameObject, eventData);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		onPointerExitCallBack?.Invoke();
		onPointerExit?.Invoke(gameObject, eventData);
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		onBeginDragCallBack?.Invoke();
		onBeginDrag?.Invoke(gameObject, eventData);
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		onDragCallBack?.Invoke();
		onDrag?.Invoke(gameObject, eventData);
	}

	void IDropHandler.OnDrop(PointerEventData eventData)
	{
		onDropCallBack?.Invoke();
		onDrag?.Invoke(gameObject, eventData);
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		onEndDragCallBack?.Invoke();
		onEndDrag?.Invoke(gameObject, eventData);
	}
}

}
