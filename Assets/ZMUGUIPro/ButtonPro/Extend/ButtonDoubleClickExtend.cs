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
using UnityEngine;
using UnityEngine.Events;

namespace ZM.UGUIPro {	
	[System.Serializable]
	public class ButtonDoubleClickExtend
	{
	    [SerializeField]
	    private bool m_IsUseDoubleClick;
	    [Header("有效时间")]
	    [SerializeField]
	    private float m_ClickInterval;
	    [SerializeField]
	    private float m_LastPointerDownTime;
	    [SerializeField]
	    private ButtonClickEvent m_ButtonClickedEvent;
	    public   void OnPointerDown()
	    {
	        m_LastPointerDownTime = Time.realtimeSinceStartup - m_LastPointerDownTime < m_ClickInterval ? 0 : Time.realtimeSinceStartup;
	        if (m_LastPointerDownTime == 0)
	            m_ButtonClickedEvent?.Invoke();
	    }
		public void AddListener(UnityAction callback,float clickInterval)
		{
			m_ClickInterval = clickInterval;
			m_IsUseDoubleClick = true;
            m_ButtonClickedEvent.AddListener(callback);
            
        }
	}
}
