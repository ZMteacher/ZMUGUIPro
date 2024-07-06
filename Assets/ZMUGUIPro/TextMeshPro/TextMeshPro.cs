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
using TMPro;
using UnityEngine;
using ZM.UGUIPro;
namespace ZM.UGUIPro
{
    [System.Serializable]
    public class TextMeshPro : TextMeshProUGUI
    {
        [SerializeField]
        LocalizationTextExtend m_LocalizationTextExtend = new LocalizationTextExtend();


        protected override void Awake()
        {
            base.Awake();
            if (m_LocalizationTextExtend.UseLocalization)
                m_LocalizationTextExtend.Initializa(this);
            m_LocalizationTextExtend.UpdateFont();

            m_LocalizationTextExtend.UpdateFont();
            m_LocalizationTextExtend.UpdateText();
        }
 
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (m_LocalizationTextExtend.UseLocalization)
                m_LocalizationTextExtend.Release();

        }

    }
}