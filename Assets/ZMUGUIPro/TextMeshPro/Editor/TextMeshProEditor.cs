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
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
namespace ZM.UGUIPro
{
    [CustomEditor(typeof(TextMeshPro), true)]
    [CanEditMultipleObjects]
    public class TextMeshProEditor : TMP_BaseEditorPanel
    {
        static readonly GUIContent k_RaycastTargetLabel = new GUIContent("Raycast Target", "Whether the text blocks raycasts from the Graphic Raycaster.");
        static readonly GUIContent k_MaskableLabel = new GUIContent("Maskable", "Determines if the text object will be affected by UI Mask.");

        private static bool m_LocalizationTextPanelOpen = false;

        private string[] titles_E = { "多语言" };
        private string[] titles_C = { "LocalizationText", };

        SerializedProperty m_Text;
        SerializedProperty m_FontData;


        //Localization
        SerializedProperty m_UseLocalization;
        SerializedProperty m_Key;


        protected override void OnEnable()
        {
            base.OnEnable();


            m_Text = serializedObject.FindProperty("Text");
            m_FontData = serializedObject.FindProperty("FontAsset");

            //Localization
            m_UseLocalization = serializedObject.FindProperty("m_LocalizationTextExtend.m_UseLocalization");
            m_Key = serializedObject.FindProperty("m_LocalizationTextExtend.m_Key");

            m_LocalizationTextPanelOpen = EditorPrefs.GetBool("UGUIPro.m_LocalizationTextPanelOpen", m_LocalizationTextPanelOpen);

            m_RaycastTargetProp = serializedObject.FindProperty("m_RaycastTarget");
            m_MaskableProp = serializedObject.FindProperty("m_Maskable");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            //EditorGUILayout.PropertyField(m_Text);
            //EditorGUILayout.PropertyField(m_FontData);
            //AppearanceControlsGUI();
            //RaycastControlsGUI();
            TextProGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void TextProGUI()
        {
            GUI.enabled = false;
            //EditorGUILayout.ObjectField("Graphic", ((TextEffect)m_TextEffect.objectReferenceValue).TextGraphic, typeof(Text), false);
            GUI.enabled = true;


            TextMeshProDrawEditor.LocalizationGUI(
                     GetTitle(0),
                   ref m_LocalizationTextPanelOpen,
                   0f,
                   m_UseLocalization,
                   m_Key
                   );


            if (GUI.changed)
            {

                EditorPrefs.SetBool("UGUIPro.m_LocalizationTextPanelOpen", m_LocalizationTextPanelOpen);

            }
        }

        string GetTitle(int index)
        {
            return UGUIProSetting.EditorLanguageType == 0 ? titles_E[index] : titles_C[index];
        }

        SerializedProperty m_RaycastTargetProp;
        private SerializedProperty m_MaskableProp;



        protected override void DrawExtraSettings()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 24);

            if (GUI.Button(rect, new GUIContent("<b>Extra Settings</b>"), TMP_UIStyleManager.sectionHeader))
                Foldout.extraSettings = !Foldout.extraSettings;

            GUI.Label(rect, (Foldout.extraSettings ? k_UiStateLabel[0] : k_UiStateLabel[1]), TMP_UIStyleManager.rightLabel);
            if (Foldout.extraSettings)
            {
                //EditorGUI.indentLevel += 1;

                DrawMargins();

                DrawGeometrySorting();

                DrawIsTextObjectScaleStatic();

                DrawRichText();

                DrawRaycastTarget();

                DrawMaskable();

                DrawParsing();

                DrawSpriteAsset();

                DrawStyleSheet();

                DrawKerning();

                DrawPadding();

                //EditorGUI.indentLevel -= 1;

            }

            //增加部分
            //EditorGUILayout.PropertyField(ageProp, new GUIContent("age"));
            //EditorGUILayout.PropertyField(nameProp, new GUIContent("name"));
            //EditorGUILayout.PropertyField(boolProp, new GUIContent("isBoy"));
            //EditorGUILayout.PropertyField(enumProp, new GUIContent("enum"));
        }

        protected void DrawRaycastTarget()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_RaycastTargetProp, k_RaycastTargetLabel);
            if (EditorGUI.EndChangeCheck())
            {
                // Change needs to propagate to the child sub objects.
                Graphic[] graphicComponents = m_TextComponent.GetComponentsInChildren<Graphic>();
                for (int i = 1; i < graphicComponents.Length; i++)
                    graphicComponents[i].raycastTarget = m_RaycastTargetProp.boolValue;

                m_HavePropertiesChanged = true;
            }
        }

        protected void DrawMaskable()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_MaskableProp, k_MaskableLabel);
            if (EditorGUI.EndChangeCheck())
            {
                m_TextComponent.maskable = m_MaskableProp.boolValue;

                // Change needs to propagate to the child sub objects.
                MaskableGraphic[] maskableGraphics = m_TextComponent.GetComponentsInChildren<MaskableGraphic>();
                for (int i = 1; i < maskableGraphics.Length; i++)
                    maskableGraphics[i].maskable = m_MaskableProp.boolValue;

                m_HavePropertiesChanged = true;
            }
        }

        // Method to handle multi object selection
        protected override bool IsMixSelectionTypes()
        {
            GameObject[] objects = Selection.gameObjects;
            if (objects.Length > 1)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i].GetComponent<TextMeshProUGUI>() == null)
                        return true;
                }
            }
            return false;
        }
        protected override void OnUndoRedo()
        {
            int undoEventId = Undo.GetCurrentGroup();
            int lastUndoEventId = s_EventId;

            if (undoEventId != lastUndoEventId)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    //Debug.Log("Undo & Redo Performed detected in Editor Panel. Event ID:" + Undo.GetCurrentGroup());
                    TMPro_EventManager.ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(true, targets[i] as TextMeshProUGUI);
                    s_EventId = undoEventId;
                }
            }
        }
    }

}