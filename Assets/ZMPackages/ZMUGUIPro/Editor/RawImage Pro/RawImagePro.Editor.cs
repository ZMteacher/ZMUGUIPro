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
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

 namespace ZM.UGUIPro {	
	[CustomEditor(typeof(RawImagePro), true)]
	[CanEditMultipleObjects]
	public class RawImageProEditor : RawImageEditor
	{
 
	    private static bool m_RawImageMaskPanelOpen = false;
	    
		SerializedProperty m_IsUseMask;
 
	    SerializedProperty m_Radius;
	    SerializedProperty m_TriangleNum;
	    SerializedProperty m_Scale;
	
	    protected override void OnEnable()
	    {
	        base.OnEnable();
	        
			//ImageMask
			m_IsUseMask = serializedObject.FindProperty("m_RawImageProMaskExtend.IsUseMask");
 	        m_Radius = serializedObject.FindProperty("m_RawImageProMaskExtend.Radius");
	        m_TriangleNum = serializedObject.FindProperty("m_RawImageProMaskExtend.TriangleNum");
	        m_Scale = serializedObject.FindProperty("m_RawImageProMaskExtend.Scale");
	
	        m_RawImageMaskPanelOpen = EditorPrefs.GetBool("UGUIPro.m_RawImageMaskPanelOpen", m_RawImageMaskPanelOpen);
	    }
	    public override void OnInspectorGUI()
	    {
	        base.OnInspectorGUI();
	        
	        RawImageProGUI();
	
	        serializedObject.ApplyModifiedProperties();
	    }
	    void RawImageProGUI()
	    {
 	        RawImageProDrawEditor.DrawImageMask("裁剪遮罩", ref m_RawImageMaskPanelOpen, m_IsUseMask,  m_Radius, m_TriangleNum, m_Scale);
	        if (GUI.changed)
	        {
	            EditorPrefs.SetBool("UGUIPro.m_RawImageMaskPanelOpen", m_RawImageMaskPanelOpen);
	        }
	    }
	}
}
