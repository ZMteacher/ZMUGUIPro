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
    public static class TextMeshProDrawEditor
    {

        [MenuItem("GameObject/UI/TextMeshPro")]
        public static void CreateTextPro()
        {
            GameObject root = new GameObject("TextMeshPro", typeof(RectTransform), typeof(TextMeshPro));
            ResetInCanvasFor((RectTransform)root.transform);
            root.GetComponent<TextMeshPro>().text = "TextMeshPro";
            var text = root.GetComponent<TextMeshPro>();
            text.text = "Text Pro";
            text.color = Color.white;
            text.raycastTarget = false;
            text.rectTransform.sizeDelta = new Vector2(200, 50);
            text.fontSize = 24;
            text.alignment = TMPro.TextAlignmentOptions.Midline;
            root.transform.localPosition = Vector3.zero;
        }


        private static bool isCheckLoaclData = false;
        public static void LocalizationGUI(string title, ref bool m_PanelOpen, float space, SerializedProperty useThis, SerializedProperty key)
        {
            LayoutFrameBox(() =>
            {
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
