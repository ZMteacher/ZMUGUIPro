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
using UnityEngine.UI;
namespace ZM.UGUIPro
{
    [System.Serializable]
    public class TextProBase : Text, IMeshModifier
    {
        [SerializeField]
        public string winName;
        [SerializeField]
        TextSpacingExtend m_TextSpacingExtend = new TextSpacingExtend();
        [SerializeField]
        VertexColorExtend m_VertexColorExtend = new VertexColorExtend();
        [SerializeField]
        TextShadowExtend m_TextShadowExtend = new TextShadowExtend();
        [SerializeField]
        TextOutlineExtend m_TextOutlineExtend = new TextOutlineExtend();
        [SerializeField]
        LocalizationTextExtend m_LocalizationTextExtend = new LocalizationTextExtend();
        [SerializeField]
        TextEffectExtend m_TextEffectExtend = new TextEffectExtend();
        public TextSpacingExtend TextSpacingExtend { get { return m_TextSpacingExtend; } }

        public VertexColorExtend VertexColorExtend { get { return m_VertexColorExtend; } }

        public TextShadowExtend TextShadowHandler { get { return m_TextShadowExtend; } }

        public TextOutlineExtend TextOutlineHandler { get { return m_TextOutlineExtend; } }

        public LocalizationTextExtend LocalizationTextExtend { get { return m_LocalizationTextExtend; } }
        public TextEffectExtend TextEffectExtend { get { return m_TextEffectExtend; } }

        protected override void Awake()
        {
            base.Awake();
            if (LocalizationTextExtend.UseLocalization)
                LocalizationTextExtend.Initializa(this);
            m_LocalizationTextExtend.UpdateFont();
            if (LocalizationTextExtend.ChangeFont)
                LocalizationTextExtend.InitFontListener(this);
        }
        protected override void Start()
        {
            base.Start();
            m_LocalizationTextExtend.UpdateFont();
            m_LocalizationTextExtend.UpdateText();

            //WindowBehaviour.PopWindowListener+= OnWindowShow;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            //WindowBehaviour.PopWindowListener -= OnWindowShow;
            if (LocalizationTextExtend.UseLocalization)
                LocalizationTextExtend.Release();
            if (LocalizationTextExtend.ChangeFont)
                LocalizationTextExtend.RemoveFontListener();
        }
      
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
            if (m_TextSpacingExtend.UseTextSpacing)
                m_TextSpacingExtend.PopulateMesh(toFill);
            if (m_VertexColorExtend.UseVertexColor)
                m_VertexColorExtend.PopulateMesh(toFill, rectTransform, color);
            if (m_TextShadowExtend.UseShadow)
                m_TextShadowExtend.PopulateMesh(toFill, rectTransform, color);
            if (m_TextOutlineExtend.UseOutline)
                m_TextOutlineExtend.PopulateMesh(toFill);
        }
        public void ModifyMesh(VertexHelper vh)
        {
            //if (m_TextShadowExtend.UseShadow)
            //    m_TextShadowExtend.PopulateMesh(vh, rectTransform, color);
        }
        public void ModifyMesh(Mesh mesh)
        {

        }
        public void SetTextAlpha(float alpha)
        {
            if (m_TextEffectExtend.UseTextEffect && m_TextEffectExtend.m_GradientType != 0)
            {
                m_TextEffectExtend.SetAlpha(alpha);
            }
            else
            {
                Color32 color32 = color;
                color32.a = (byte)(alpha * 255);
                color = color32;
            }

        }
        public void SetOutLineColor(Color32 color)
        {
            if (!m_TextEffectExtend.UseTextEffect) return;
            m_TextEffectExtend.m_TextEffect.SetOutLineColor(color);
            m_TextEffectExtend.UseTextEffect = false;
            m_TextEffectExtend.UseTextEffect = true;
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }
#endif
    }
}
