/*----------------------------------------------------------------
* Title: ZM.UGUIPro
*
* Description: TextPro ImagePro ButtonPro TextMesh Pro
* 
* Support Function: 高性能描边、本地多语言文本、图片、按钮双击模式、长按模式、文本顶点颜色渐变、双色渐变、三色渐变
* 
* Usage: 右键-TextPro-ImagePro-ButtonPro-TextMeshPro
* 
* Author: 铸梦 www.taikr.com/user/63798c7981862239d5b3da44d820a7171f0ce14d
*
* Date: 2023.4.13
*
* Modify: 
--------------------------------------------------------------------*/
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ZM.UGUIPro
{
    [System.Serializable]
    public class ButtonProBase : Button, IUpdateSelectedHandler
    {
        [SerializeField]
        private ButtonDoubleClickExtend m_ButtonDoubleClickExtend = new ButtonDoubleClickExtend();
        [SerializeField]
        private ButtonLongPressExtend m_ButtonLongPressExtend = new ButtonLongPressExtend();
        [SerializeField]
        private ButtonClickEvent m_ButtonClickEvent = new ButtonClickEvent();
        [SerializeField]
        private ButtonPressScaleExtend m_ButtonScaleExtend = new ButtonPressScaleExtend();
        [SerializeField]
        private ButtonAudioExtend m_ButtonAudioExtend = new ButtonAudioExtend();

        private Vector2 m_PressPos;
        private bool mIsPreass;
        private PointerEventData mPointerEventData;
        public Action OnPointerUpListener;
        public override void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClick();
        }
        public void OnPointerClick()
        {
            if (m_ButtonAudioExtend != null)
            {
                if (m_ButtonAudioExtend.OnButtonClick() && interactable)
                {
                    onClick?.Invoke();
                }
            }
            else
            {
                if (interactable)
                    onClick?.Invoke();
            }
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            m_PressPos = eventData.position;
            mIsPreass = true;
            mPointerEventData = eventData;
            m_ButtonLongPressExtend?.OnPointerDown();
            m_ButtonDoubleClickExtend?.OnPointerDown();
            m_ButtonScaleExtend?.OnPointerDown(transform, interactable);
            m_ButtonAudioExtend.OnPointerDown(transform);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            mPointerEventData = null;
            mIsPreass = false;
            //判断手指按下时移动的范围，超过一定的范围后不触发按钮事件，如手指移动出按钮所在区域
            if (interactable && Mathf.Abs(Vector2.Distance(m_PressPos, eventData.position)) < 10)
            {
                m_ButtonClickEvent?.Invoke();
                m_ButtonAudioExtend.OnPointerUp(this);
            }
            OnPointerUpListener?.Invoke();
            m_ButtonScaleExtend?.OnPointerUp(transform, interactable);
            EventSystem.current.SetSelectedGameObject(null);
        }
        public void OnUpdateSelected(BaseEventData eventData)
        {
            m_ButtonLongPressExtend?.OnUpdateSelected();
        }
        public void AddButtonLongListener(UnityAction callback,float duration)
        {
 
            m_ButtonLongPressExtend.AddListener(callback, duration);
        }
        public void AddButtonDoubleClickListener(UnityAction callback,float clickInterval)
        {
            m_ButtonDoubleClickExtend.AddListener(callback, clickInterval);
        }
        public void AddButtonClick(UnityAction callback)
        {
            //....
            m_ButtonClickEvent.AddListener(callback);
        }
        public void RemoveButtonClick(UnityAction callback)
        {
            m_ButtonClickEvent.RemoveListener(callback);
        }
        public void OnApplicationFocus(bool focus)
        {
            if (focus==false)
            {
                //Debuger.ColorLog(LogColor.Cyan, "OnApplicationFocus   mIsPreass:" + mIsPreass + " mPointerEventData:" + mPointerEventData);
                if (mIsPreass && mPointerEventData != null)
                {
                    OnPointerUp(mPointerEventData);
                }
            }
           
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (m_ButtonScaleExtend.UsePressScale)
            {
                transition = Transition.None;
            }
        }
#endif


    }
    [System.Serializable]
    public class ButtonClickEvent : UnityEvent
    {
        public ButtonClickEvent() { }
    }
}
