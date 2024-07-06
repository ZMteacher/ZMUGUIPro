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

 namespace ZM.UGUIPro {	
	public class UnityEditorUtility
	{
	    private static GUIStyle gSelectButtonStyle;
	    public static GUIStyle gCommonButtonStyle;
	    public static GUIContent GetUIContent(string name, string conent,string tooltip="")
	    {
	        GUIContent content = EditorGUIUtility.IconContent(name.Trim(), "测试文字显示");
	        content.text = conent;
	        content.tooltip = tooltip;
	 
	        return content;
	    }
	    public static GUIStyle GetSelectButtonStyle()
	    {
	        if (gSelectButtonStyle==null)
	        {
	            gSelectButtonStyle = GetGUIStyle("flow node 3 on");
	        }
	        return gSelectButtonStyle;
	    }
	    public static GUIStyle GetGUIStyle(string stylename)
	    {
	        GUIStyle guiStyle = null;
	        foreach (var style in GUI.skin.customStyles)
	        {
	            if (string.Equals(style.name.ToLower(), stylename.ToLower()))
	            {
	                guiStyle = style;
	                break;
	            }
	        };
	        guiStyle.normal.textColor = Color.yellow;
	        return guiStyle;
	    }
	}
}
