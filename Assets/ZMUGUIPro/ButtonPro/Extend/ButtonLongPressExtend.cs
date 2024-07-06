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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

 namespace ZM.UGUIPro {	
	[System.Serializable]
	public class ButtonLongPressExtend
	{
	    [SerializeField]
	    private bool m_IsUseLongPress;
	    [Header("长按时间")]
	    [SerializeField]
	    private float m_Duration;
	    [SerializeField]
	    private ButtonClickEvent m_ButtonLongPressEvent;
	    private float m_PointerDownTime;
	
	    public void OnPointerDown()
	    {
			m_PointerDownTime = Time.realtimeSinceStartup;
	    }
	    public void OnUpdateSelected()
	    {
            if (m_Duration>=0&&Time.realtimeSinceStartup - m_PointerDownTime >= m_Duration)
	        {
                m_ButtonLongPressEvent?.Invoke();
	            EventSystem.current.SetSelectedGameObject(null);
	        }
	    }
		public void AddListener(UnityAction callback,float duration)
		{
			m_Duration = duration;
			m_IsUseLongPress = true;
            m_ButtonLongPressEvent.AddListener(callback);
        }
	}
}
