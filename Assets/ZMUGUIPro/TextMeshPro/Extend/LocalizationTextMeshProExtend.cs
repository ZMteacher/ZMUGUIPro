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

namespace ZM.UGUIPro
{
    [System.Serializable]
    public class LocalizationTextMeshProExtend
    {

        [SerializeField]
        private bool m_UseLocalization;
        public bool UseLocalization
        {
            get
            {
                return m_UseLocalization;
            }
            set
            {
                m_UseLocalization = value;
            }
        }
        [SerializeField]
        private string m_Key;
        public string Key { get { return m_Key; } }

        private TextPro mTextPro;

        public void Initializa(TextProBase textPro)
        {
            mTextPro =(TextPro) textPro;
            LocalizationManager.Instance.AddLanguageChangeListener(UpdateText);
        }
        public void Release()
        {
            LocalizationManager.Instance.RemoveLanguageChangeListener(UpdateText);
        }
        public void UpdateText()
        {
            if (m_UseLocalization == false)
                return;
            if (mTextPro != null)
            {
                mTextPro.text = LocalizationManager.Instance.GetLocalizationText(m_Key);
            }

        }
    }
}
