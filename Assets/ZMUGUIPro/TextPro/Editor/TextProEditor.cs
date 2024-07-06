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
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;
namespace ZM.UGUIPro
{
    [CustomEditor(typeof(TextPro), true)]
    [CanEditMultipleObjects]
    public class TextProEditor : GraphicEditor
    {
        private static bool m_TextSpacingPanelOpen = false;
        private static bool m_VertexColorPanelOpen = false;
        private static bool m_TextShadowPanelOpen = false;
        private static bool m_TextOutlinePanelOpen = false;
        private static bool m_LocalizationTextPanelOpen = false;
        private static bool m_TextEffectPanelOpen = false;

        private string[] titles_E = { "字符间距", "顶点颜色", "阴影", "多语言", "描边&渐变" };
        private string[] titles_C = { "Text Spacing", "Vertex Color", "Shadow", "LocalizationText", "Outline & Gradient" };

        SerializedProperty m_Text;
        SerializedProperty m_FontData;

        //text spacing
        SerializedProperty m_UseTextSpacing;
        SerializedProperty m_TextSpacing;

        // Ver Color
        SerializedProperty m_UseVertexColor;
        SerializedProperty m_VertexColorFilter;
        SerializedProperty m_VertexColorOffset;
        SerializedProperty m_VertexTopLeft;
        SerializedProperty m_VertexTopRight;
        SerializedProperty m_VertexBottomLeft;
        SerializedProperty m_VertexBottomRight;

        //Shadow
        SerializedProperty m_UseShadow;
        SerializedProperty m_ShadowColorTopLeft;
        SerializedProperty m_ShadowColorTopRight;
        SerializedProperty m_ShadowColorBottomLeft;
        SerializedProperty m_ShadowColorBottomRight;
        SerializedProperty m_ShadowEffectDistance;

        //Localization
        SerializedProperty m_UseLocalization;
        SerializedProperty m_Key;
        SerializedProperty m_ChangeFont;

        //TextEffect
        SerializedProperty m_UseTextEffect;
        SerializedProperty m_GradientType;
        SerializedProperty m_TopColor;
        SerializedProperty m_OpenShaderOutLine;
        SerializedProperty m_MiddleColor;
        SerializedProperty m_BottomColor;
        SerializedProperty m_ColorOffset;
        SerializedProperty m_EnableOutLine;
        SerializedProperty m_OutLineColor;
        SerializedProperty m_OutLineWidth;
        SerializedProperty m_Camera;
        SerializedProperty m_LerpValue;
        SerializedProperty m_Alpha;
        SerializedProperty m_TextEffect;

        protected override void OnEnable()
        {
            base.OnEnable();

            TextPro m_TextPlus = (TextPro)this.target;
            m_TextPlus.TextEffectExtend.SaveSerializeData(m_TextPlus);



            m_Text = serializedObject.FindProperty("m_Text");
            m_FontData = serializedObject.FindProperty("m_FontData");

            //text spacing
            m_UseTextSpacing = serializedObject.FindProperty("m_TextSpacingExtend.m_UseTextSpacing");
            m_TextSpacing = serializedObject.FindProperty("m_TextSpacingExtend.m_TextSpacing");

            // VertexColor
            m_UseVertexColor = serializedObject.FindProperty("m_VertexColorExtend.m_UseVertexColor");
            m_VertexColorFilter = serializedObject.FindProperty("m_VertexColorExtend.m_VertexColorFilter");
            m_VertexTopLeft = serializedObject.FindProperty("m_VertexColorExtend.m_VertexTopLeft");
            m_VertexTopRight = serializedObject.FindProperty("m_VertexColorExtend.m_VertexTopRight");
            m_VertexBottomLeft = serializedObject.FindProperty("m_VertexColorExtend.m_VertexBottomLeft");
            m_VertexBottomRight = serializedObject.FindProperty("m_VertexColorExtend.m_VertexBottomRight");
            m_VertexColorOffset = serializedObject.FindProperty("m_VertexColorExtend.m_VertexColorOffset");

            //Shadow
            m_UseShadow = serializedObject.FindProperty("m_TextShadowExtend.m_UseShadow");
            m_ShadowColorTopLeft = serializedObject.FindProperty("m_TextShadowExtend.m_ShadowColorTopLeft");
            m_ShadowColorTopRight = serializedObject.FindProperty("m_TextShadowExtend.m_ShadowColorTopRight");
            m_ShadowColorBottomLeft = serializedObject.FindProperty("m_TextShadowExtend.m_ShadowColorBottomLeft");
            m_ShadowColorBottomRight = serializedObject.FindProperty("m_TextShadowExtend.m_ShadowColorBottomRight");
            m_ShadowEffectDistance = serializedObject.FindProperty("m_TextShadowExtend.m_EffectDistance");


            //Localization
            m_UseLocalization = serializedObject.FindProperty("m_LocalizationTextExtend.m_UseLocalization");
            m_Key = serializedObject.FindProperty("m_LocalizationTextExtend.m_Key");
            m_ChangeFont = serializedObject.FindProperty("m_LocalizationTextExtend.m_ChangeFont");

            //TextEffect
            m_UseTextEffect = serializedObject.FindProperty("m_TextEffectExtend.m_UseTextEffect");
            m_Alpha = this.serializedObject.FindProperty("m_TextEffectExtend.m_Alpha");
            m_GradientType = this.serializedObject.FindProperty("m_TextEffectExtend.m_GradientType");
            m_TopColor = this.serializedObject.FindProperty("m_TextEffectExtend.m_TopColor");
            m_OpenShaderOutLine = this.serializedObject.FindProperty("m_TextEffectExtend.m_OpenShaderOutLine");
            m_MiddleColor = this.serializedObject.FindProperty("m_TextEffectExtend.m_MiddleColor");
            m_BottomColor = this.serializedObject.FindProperty("m_TextEffectExtend.m_BottomColor");
            m_ColorOffset = this.serializedObject.FindProperty("m_TextEffectExtend.m_ColorOffset");
            m_EnableOutLine = this.serializedObject.FindProperty("m_TextEffectExtend.m_EnableOutLine");
            m_OutLineColor = this.serializedObject.FindProperty("m_TextEffectExtend.m_OutLineColor");
            m_OutLineWidth = this.serializedObject.FindProperty("m_TextEffectExtend.m_OutLineWidth");
            m_Camera = this.serializedObject.FindProperty("m_TextEffectExtend.m_Camera");
            m_LerpValue = this.serializedObject.FindProperty("m_TextEffectExtend.m_LerpValue");
            m_TextEffect = this.serializedObject.FindProperty("m_TextEffectExtend.m_TextEffect");
            // Panel Open
            m_TextSpacingPanelOpen = EditorPrefs.GetBool("UGUIPro.m_TextSpacingPanelOpen", m_TextSpacingPanelOpen);
            m_VertexColorPanelOpen = EditorPrefs.GetBool("UGUIPro.m_VertexColorPanelOpen", m_VertexColorPanelOpen);
            m_TextShadowPanelOpen = EditorPrefs.GetBool("UGUIPro.m_TextShadowPanelOpen", m_TextShadowPanelOpen);
            m_TextOutlinePanelOpen = EditorPrefs.GetBool("UGUIPro.m_TextOutlinePanelOpen", m_TextOutlinePanelOpen);
            m_LocalizationTextPanelOpen = EditorPrefs.GetBool("UGUIPro.m_LocalizationTextPanelOpen", m_LocalizationTextPanelOpen);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Text);
            EditorGUILayout.PropertyField(m_FontData);
            AppearanceControlsGUI();
            RaycastControlsGUI();
            TextProGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void TextProGUI()
        {
            GUI.enabled = false;
            if (m_TextEffect.objectReferenceValue != null)
            {
                EditorGUILayout.ObjectField("Graphic", ((TextEffect)m_TextEffect.objectReferenceValue).TextGraphic, typeof(Text), false);
            }
            GUI.enabled = true;

            TextProDrawEditor.TextSpacingGUI(GetTitle(0), m_UseTextSpacing, m_TextSpacing, ref m_TextSpacingPanelOpen);

            TextProDrawEditor.VertexColorGUI(
                    GetTitle(1),
                  m_UseVertexColor,
                  m_VertexTopLeft,
                  m_VertexTopRight,
                  m_VertexBottomLeft,
                  m_VertexBottomRight,
                  m_VertexColorFilter,
                  m_VertexColorOffset,
                  ref m_VertexColorPanelOpen
              );

            TextProDrawEditor.TextShadowGUI(
                  GetTitle(2),
                   m_UseShadow,
                   m_ShadowColorTopLeft,
                   m_ShadowColorTopRight,
                   m_ShadowColorBottomLeft,
                   m_ShadowColorBottomRight,
                   m_ShadowEffectDistance,
                   ref m_TextShadowPanelOpen
                   );
            TextProDrawEditor.LocalizationGUI(
                     GetTitle(3),
                   ref m_LocalizationTextPanelOpen,
                   0f,
                   m_UseLocalization,
                   m_Key,
                   m_ChangeFont
                   );
            TextProDrawEditor.TextEffectGUI(GetTitle(4), m_UseTextEffect, ref m_TextEffectPanelOpen,
                  m_GradientType,
                 m_TopColor,
                 m_OpenShaderOutLine,
                 m_MiddleColor,
                 m_BottomColor,
                 m_ColorOffset,
                 m_EnableOutLine,
                 m_OutLineColor,
                 m_OutLineWidth,
                 m_Camera,
                 m_LerpValue,
                 m_Alpha,
                 (TextEffect)m_TextEffect.objectReferenceValue
               );

            if (GUI.changed)
            {
                EditorPrefs.SetBool("UGUIPro.m_TextSpacingPanelOpen", m_TextSpacingPanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_VertexColorPanelOpen", m_VertexColorPanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_TextShadowPanelOpen", m_TextShadowPanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_TextOutlinePanelOpen", m_TextOutlinePanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_LocalizationTextPanelOpen", m_LocalizationTextPanelOpen);
                EditorPrefs.SetBool("UGUIPro.m_TextEffectPanelOpen", m_TextEffectPanelOpen);
            }
        }

        string GetTitle(int index)
        {
            return UGUIProSetting.EditorLanguageType == 0 ? titles_E[index] : titles_C[index];
        }
    }
}
