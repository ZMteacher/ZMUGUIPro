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
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace ZM.UGUIPro
{
    public static class TextProDrawEditor
    {

        [MenuItem("GameObject/UI/Text Pro",priority =0)]
        public static void CreateTextPro()
        {
            GameObject root = new GameObject("Text Pro", typeof(RectTransform), typeof(TextPro));
            ResetInCanvasFor((RectTransform)root.transform);
            root.GetComponent<TextPro>().text = "Text Pro";
            var text = root.GetComponent<TextPro>();
            text.text = "Text Pro";
            text.color = Color.white;
            text.raycastTarget = false;
            text.rectTransform.sizeDelta = new Vector2(200, 50);
            text.fontSize = 24;
            text.alignment = TextAnchor.MiddleCenter;
            root.transform.localPosition = Vector3.zero;
        }

        public static void TextSpacingGUI(string title, SerializedProperty m_UseTextSpacing, SerializedProperty m_TextSpacing, ref bool m_TextSpacingPanelOpen)
        {
            LayoutFrameBox(() =>
            {
                EditorGUILayout.PropertyField(m_UseTextSpacing);
                if (m_UseTextSpacing.boolValue)
                {
                    Space();
                    LayoutHorizontal(() =>
                    {
                        EditorGUI.PropertyField(GUIRect(0, 18), m_TextSpacing, new GUIContent());
                    });
                }
            }, title, ref m_TextSpacingPanelOpen, true);
        }

        public static void VertexColorGUI(string title, SerializedProperty m_UseVertexColor, SerializedProperty m_VertexTopLeft, SerializedProperty m_VertexTopRight, SerializedProperty m_VertexBottomLeft, SerializedProperty m_VertexBottomRight, SerializedProperty m_VertexColorFilter, SerializedProperty m_VertexColorOffset, ref bool m_VertexColorPanelOpen)
        {
            LayoutFrameBox(() =>
            {
                EditorGUILayout.PropertyField(m_UseVertexColor);
                if (m_UseVertexColor.boolValue)
                {
                    Space();
                    LayoutHorizontal(() =>
                    {
                        EditorGUI.PropertyField(GUIRect(0, 18), m_VertexTopLeft, new GUIContent());
                        Space();
                        EditorGUI.PropertyField(GUIRect(0, 18), m_VertexTopRight, new GUIContent());
                    });
                    Space();
                    LayoutHorizontal(() =>
                    {
                        EditorGUI.PropertyField(GUIRect(0, 18), m_VertexBottomLeft, new GUIContent());
                        Space();
                        EditorGUI.PropertyField(GUIRect(0, 18), m_VertexBottomRight, new GUIContent());
                    });
                    Space();
                    m_VertexColorFilter.enumValueIndex = (int)(VertexColorExtend.ColorFilterType)EditorGUILayout.EnumPopup(
                        new GUIContent("Filter"), (VertexColorExtend.ColorFilterType)m_VertexColorFilter.enumValueIndex
                    );
                    Vector2 newOffset = EditorGUILayout.Vector2Field("Offset", m_VertexColorOffset.vector2Value);
                    newOffset.x = Mathf.Clamp(newOffset.x, -1f, 1f);
                    newOffset.y = Mathf.Clamp(newOffset.y, -1f, 1f);
                    m_VertexColorOffset.vector2Value = newOffset;
                    Space();
                }
            }, title, ref m_VertexColorPanelOpen, true);
        }

        public static void TextShadowGUI(string title, SerializedProperty m_UseShadow, SerializedProperty m_ShadowColorTopLeft, SerializedProperty m_ShadowColorTopRight,
               SerializedProperty m_ShadowColorBottomLeft, SerializedProperty m_ShadowColorBottomRight, SerializedProperty m_ShadowEffectDistance, ref bool m_TextShadowPanelOpen)
        {
            LayoutFrameBox(() =>
            {
                EditorGUILayout.PropertyField(m_UseShadow);
                if (m_UseShadow.boolValue)
                {
                    Space();
                    LayoutHorizontal(() =>
                    {
                        EditorGUI.PropertyField(GUIRect(0, 18), m_ShadowColorTopLeft, new GUIContent());
                        Space();
                        EditorGUI.PropertyField(GUIRect(0, 18), m_ShadowColorTopRight, new GUIContent());
                    });
                    Space();
                    LayoutHorizontal(() =>
                    {
                        EditorGUI.PropertyField(GUIRect(0, 18), m_ShadowColorBottomLeft, new GUIContent());
                        Space();
                        EditorGUI.PropertyField(GUIRect(0, 18), m_ShadowColorBottomRight, new GUIContent());
                    });
                    Space();
                    EditorGUILayout.PropertyField(m_ShadowEffectDistance);
                }
            }, title, ref m_TextShadowPanelOpen, true);
        }
     
        public static void TextEffectGUI(string title, SerializedProperty m_UseTextEffect, ref bool m_TextEffectPanelOpen,
        SerializedProperty m_GradientType,
        SerializedProperty m_TopColor,
        SerializedProperty m_OpenShaderOutLine,
        SerializedProperty m_MiddleColor,
        SerializedProperty m_BottomColor,
        SerializedProperty m_ColorOffset,
        SerializedProperty m_EnableOutLine,
        SerializedProperty m_OutLineColor,
        SerializedProperty m_OutLineWidth,
        SerializedProperty m_Camera,
        SerializedProperty m_LerpValue,
        SerializedProperty m_Alpha,
        TextEffect m_TextEffect)
        {
            LayoutFrameBox(() =>
            {
         
                EditorGUILayout.PropertyField(m_UseTextEffect);
                m_TextEffect?.SetUserEffect(m_UseTextEffect.boolValue);
                if (m_UseTextEffect.boolValue)
                {
                    Space();
                    //_alpha = m_Alpha.floatValue;
                    //_alpha = EditorGUILayout.Slider("Alpha", _alpha, 0, 1);
                    EditorGUILayout.PropertyField(m_Alpha);
                    EditorGUILayout.PropertyField(m_GradientType);
                    EditorGUILayout.PropertyField(m_Camera);
                    EditorGUILayout.PropertyField(m_EnableOutLine);
                    m_TextEffect?.SetEnableOutline(m_EnableOutLine.boolValue);
                    m_TextEffect?.SetCamera((Camera)m_Camera.objectReferenceValue);
               

                    if (m_EnableOutLine.boolValue)
                    {
                        EditorGUILayout.PropertyField(m_OutLineWidth);
                        EditorGUILayout.PropertyField(m_LerpValue);
                        m_TextEffect.SetLerpValue(m_LerpValue.floatValue);
                        m_TextEffect.SetOutLineWidth(m_OutLineWidth.floatValue);
                        m_OpenShaderOutLine.boolValue = EditorGUILayout.Toggle("Open Shader OutLine", m_OpenShaderOutLine.boolValue);
                        m_TextEffect.SetShaderOutLine(m_OpenShaderOutLine.boolValue);
                    }

                    m_TextEffect.SetGradientType((GradientType)m_GradientType.enumValueIndex);
                    if (m_GradientType.enumValueIndex == 2)
                    {
                        EditorGUILayout.PropertyField(m_MiddleColor);
                        m_TextEffect.SetMiddleColor(m_MiddleColor.colorValue);
                    }
                    if (m_GradientType.enumValueIndex != 0)
                    {
                        EditorGUILayout.PropertyField(m_TopColor);
                        EditorGUILayout.PropertyField(m_BottomColor);
                        m_TextEffect.SetBottomColor(m_BottomColor.colorValue);
                        m_TextEffect.SetTopColor(m_TopColor.colorValue);
                    }

                    if (m_EnableOutLine.boolValue)
                    {
                        EditorGUILayout.PropertyField(m_OutLineColor);
                        m_TextEffect.SetOutLineColor(m_OutLineColor.colorValue);
                    }
                    m_TextEffect.SetAlpah(m_Alpha.floatValue);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(m_ColorOffset);
                    m_TextEffect.SetColorOffset(m_ColorOffset.floatValue);
                    m_TextEffect.UpdateOutLineInfos();

                }

            }, title, ref m_TextEffectPanelOpen, true);
        }
        private static bool isCheckLoaclData = false;
        public static void LocalizationGUI(string title, ref bool m_PanelOpen, float space, SerializedProperty useThis, SerializedProperty key, SerializedProperty changeFont)
        {
            LayoutFrameBox(() =>
            {
                EditorGUILayout.PropertyField(changeFont);
                EditorGUILayout.PropertyField(useThis);
                if (useThis.boolValue)
                {
                    EditorGUILayout.PropertyField(key);
                    isCheckLoaclData = EditorGUILayout.Toggle("CheckLocalizationData", isCheckLoaclData);
                    if (isCheckLoaclData)
                    {
                        if (key != null)
                        {
                            LocalizationData data = LocalizationManager.Instance.GetLocalizationData(key.stringValue);
                            if (data != null)
                            {
                                GUILayout.Space(10);
                                FieldInfo[] propertyInfos = data.GetType().GetFields();
                                for (int i = 0; i < propertyInfos.Length; i++)
                                {
                                    FieldInfo info = propertyInfos[i];
                                    EditorGUILayout.TextField(info.Name + ":", info.GetValue(data).ToString(), new GUIStyle() { normal = new GUIStyleState() { textColor = Color.blue } });
                                }
                            }
                            else
                            {
                                EditorGUILayout.HelpBox("The key value not find!", MessageType.Error);
                            }
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("The key value not find!", MessageType.Error);
                        }
                    }
                }
            }, title, ref m_PanelOpen, true);
        }
        public static void SimpleUseGUI(string title, ref bool m_PanelOpen, float space, SerializedProperty useThis, params SerializedProperty[] sps)
        {
            LayoutFrameBox(() =>
            {
                EditorGUILayout.PropertyField(useThis);
                if (useThis.boolValue)
                {
                    foreach (var s in sps)
                    {
                        if (s != null)
                        {
                            EditorGUILayout.PropertyField(s);
                        }
                    }
                }
            }, title, ref m_PanelOpen, true);
        }

        private static void ResetInCanvasFor(RectTransform root)
        {
            root.SetParent(Selection.activeTransform);
            if (!InCanvas(root))
            {
                Transform canvasTF = GetCreateCanvas();
                root.SetParent(canvasTF);
            }
            if (!Transform.FindObjectOfType<UnityEngine.EventSystems.EventSystem>())
            {
                GameObject eg = new GameObject("EventSystem");
                eg.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eg.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            root.localScale = Vector3.one;
            root.localPosition = new Vector3(root.localPosition.x, root.localPosition.y, 0f);
            Selection.activeGameObject = root.gameObject;
        }


        private static bool InCanvas(Transform tf)
        {
            while (tf.parent)
            {
                tf = tf.parent;
                if (tf.GetComponent<Canvas>())
                {
                    return true;
                }
            }
            return false;
        }

        private static Transform GetCreateCanvas()
        {
            Canvas c = Object.FindObjectOfType<Canvas>();
            if (c)
            {
                return c.transform;
            }
            else
            {
                GameObject g = new GameObject("Canvas");
                c = g.AddComponent<Canvas>();
                c.renderMode = RenderMode.ScreenSpaceOverlay;
                g.AddComponent<CanvasScaler>();
                g.AddComponent<GraphicRaycaster>();
                return g.transform;
            }
        }

        private static void LayoutFrameBox(System.Action action, string label, ref bool open, bool box = false)
        {
            bool _open = open;
            LayoutVertical(() =>
            {
                _open = GUILayout.Toggle(
                    _open,
                    label,
                    GUI.skin.GetStyle("foldout"),
                    GUILayout.ExpandWidth(true),
                    GUILayout.Height(18)
                );
                if (_open)
                {
                    action();
                }
            }, box);
            open = _open;
        }

        private static Rect GUIRect(float width, float height)
        {
            return GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(width <= 0), GUILayout.ExpandHeight(height <= 0));
        }


        private static void Space(float space = 4f)
        {
            GUILayout.Space(space);
        }

        private static void LayoutHorizontal(System.Action action, bool box = false)
        {
            if (box)
            {
                GUIStyle style = new GUIStyle(GUI.skin.box);
                GUILayout.BeginHorizontal(style);
            }
            else
            {
                GUILayout.BeginHorizontal();
            }
            action();
            GUILayout.EndHorizontal();
        }


        private static void LayoutVertical(System.Action action, bool box = false)
        {
            if (box)
            {
                GUIStyle style = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(6, 6, 2, 2)
                };
                GUILayout.BeginVertical(style);
            }
            else
            {
                GUILayout.BeginVertical();
            }
            action();
            GUILayout.EndVertical();
        }
    }
}
