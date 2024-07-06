/*----------------------------------------------------------------
* Title: ZMAssetFrameWork
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
    public class ButtonAudioExtend
    {
        [SerializeField]
        private bool m_IsUseClickAudio;
     

        public void OnPointerDown(Transform trans)
        {
          
        }
        public void OnPointerUp(ButtonProBase buttonProBase)
        {
            //buttonProBase.OnPointerClick();
        }
        public bool  OnButtonClick()
        {
            if (m_IsUseClickAudio)
            {
                //通过自己的音频管理类播放对应的按钮点击音效
                //MusicManager.Instance.PlaySoundByPath(SoundDefine.clickSoundPath);
                ButtonProDemo.Instance.PlaySound();
            }
            
            return true;
        }
    }
     
}