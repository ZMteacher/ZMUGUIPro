/*----------------------------------------------------------------
* Title: ZM.UGUIPro
*
* Description: TextPro ImagePro ButtonPro TextMesh Pro
* 
* Support Function: 高性能描边、本地多语言文本、图片、按钮双击模式、长按模式、文本顶点颜色渐变、双色渐变、三色渐变
* 
* Usage: 右键-TextPro-ImagePro-ButtonPro-TextMeshPro
* 
* Author: 铸梦 https://www.yxtown.com/user/38633b977fadc0db8e56483c8ee365a2cafbe96b
*
* Date: 2023.4.13
*
* Modify: 
--------------------------------------------------------------------*/
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

 namespace ZM.UGUIPro {
	public class RawImageProDrawEditor
	{
	  
	    public static void DrawImageMask(string title, ref bool m_PanelOpen, SerializedProperty useMask,  SerializedProperty fill, SerializedProperty triscont, SerializedProperty segements)
	    {
	        LayoutFrameBox(() =>
	        {
	            EditorGUILayout.PropertyField(useMask);
	            if (useMask.boolValue)
	            {
	                EditorGUILayout.PropertyField(fill);
	                EditorGUILayout.PropertyField(triscont);
	                EditorGUILayout.PropertyField(segements);
	            }
	        }, title, ref m_PanelOpen, true);
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
