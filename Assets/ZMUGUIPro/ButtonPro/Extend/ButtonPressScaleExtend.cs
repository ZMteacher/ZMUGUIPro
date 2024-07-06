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
using UnityEngine.EventSystems;

namespace ZM.UGUIPro
{
    [System.Serializable]
    public class ButtonPressScaleExtend
    {
        [SerializeField]
        private bool m_IsUsePressScale = true;
        public bool UsePressScale { get { return m_IsUsePressScale; } }
        [Header("默认缩放")]
        [SerializeField]
        private Vector3 m_NormalScale=Vector3.one;
        [Header("按下缩放")]
        [SerializeField]
        private Vector3 m_PressScale=new Vector3(1.1f, 1.1f, 1.1f);

        public  void OnPointerDown(Transform trans,bool interactable)
        {
            if (m_IsUsePressScale && interactable)
            {
                trans.localScale = m_PressScale;
            }
        }

        public  void OnPointerUp(Transform trans, bool interactable)
        {
            if (m_IsUsePressScale && interactable)
            {
                trans.localScale = m_NormalScale;
            }
        }
    }
}