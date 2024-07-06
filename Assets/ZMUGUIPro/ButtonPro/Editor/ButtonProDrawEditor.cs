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
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

 namespace ZM.UGUIPro {
	public class ButtonProDrawEditor  
	{
	    [MenuItem("GameObject/UI/Button Pro")]
	    public static void CreateButtonPro()
	    {
	        RectTransform buttonProRectTrans = new GameObject("Button Pro", typeof(RectTransform), typeof(Image),typeof(ButtonPro)).GetComponent<RectTransform>();
	        Text text = new GameObject("Text Pro",typeof(RectTransform), typeof(TextPro)).GetComponent<Text>();
	        ResetInCanvasFor((RectTransform)buttonProRectTrans.transform);
	        text.transform.SetParent(buttonProRectTrans);
	        text.transform.localPosition = Vector3.zero;
	        text.transform.localScale = Vector3.one;
	        text.transform.rotation = Quaternion.identity;
	        text.color = Color.black;
	        text.text = "Button Pro";
	        text.fontSize = 24;
	        text.supportRichText = false;
	        text.alignment = TextAnchor.MiddleCenter;
	        text.raycastTarget = false;
	        buttonProRectTrans.sizeDelta= text.rectTransform.sizeDelta = new Vector2(163,50);
	        buttonProRectTrans.localPosition = Vector3.zero;
	    }
	
	
	
	    public static void DrawDoubleClickGUI(string title, ref bool m_PanelOpen, SerializedProperty isuseDoubleClick , SerializedProperty clickInterval, SerializedProperty evetnts)
	    {
	        LayoutFrameBox(() =>
	        {
	            EditorGUILayout.PropertyField(isuseDoubleClick);
	            if (isuseDoubleClick.boolValue)
	            {
	                EditorGUILayout.PropertyField(clickInterval);
	                EditorGUILayout.PropertyField(evetnts);
	            }
	        }, title, ref m_PanelOpen, true);
	    }
	    public static void DrawLongPressGUI(string title, ref bool m_PanelOpen, SerializedProperty isuseLongPress, SerializedProperty duration, SerializedProperty evetnts)
	    {
	        LayoutFrameBox(() =>
	        {
	            EditorGUILayout.PropertyField(isuseLongPress);
	            if (isuseLongPress.boolValue)
	            {
	                EditorGUILayout.PropertyField(duration);
	                EditorGUILayout.PropertyField(evetnts);
	            }
	        }, title, ref m_PanelOpen, true);
	    }
		public static void DrawSignGUI(string title, ref bool m_PanelOpen, SerializedProperty isuseLongPress)
		{
			LayoutFrameBox(() =>
			{
				EditorGUILayout.PropertyField(isuseLongPress);
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
