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
using UnityEditor.UI;
using UnityEngine.UI;

 namespace ZM.UGUIPro {	
	[CustomEditor(typeof(ImagePro), true)]
	[CanEditMultipleObjects]
	public class ImageProEditor : ImageEditor
	{
	    private string[] titles_E = { "多语言图片", "裁剪遮罩" };
	    private string[] titles_C = { "LocalizationImage", "ImageMask" };
	    private static bool m_LocalizationPanelOpen = false;
	    private static bool m_ImageMaskPanelOpen = false;
	
	    SerializedProperty m_IsUseLocalization;
	    SerializedProperty m_LocalizationImageType;
	    SerializedProperty m_LocalImageLoadType;
	    SerializedProperty m_LocalSprites;
	    SerializedProperty m_SpriteSizev2;
	    SerializedProperty m_LocalSpritePaths;
	    SerializedProperty m_LoadSpriteSizev2;
		SerializedProperty m_ImageName;


		SerializedProperty m_IsUseMask;
	    SerializedProperty m_FillPercent;
	    SerializedProperty m_Fill;
	    SerializedProperty m_TrisCont;
	    SerializedProperty m_Segements;
	
	    protected override void OnEnable()
	    {
	        base.OnEnable();
	        //localization
	        m_IsUseLocalization = serializedObject.FindProperty("m_LocalizationImage.m_IsUseLocalization");
	        m_LocalizationImageType = serializedObject.FindProperty("m_LocalizationImage.m_LocalizationImageType");
	        m_LocalImageLoadType = serializedObject.FindProperty("m_LocalizationImage.m_LocalImageLoadType");
	        m_LocalSprites = serializedObject.FindProperty("m_LocalizationImage.m_LocalSprites");
	        m_SpriteSizev2 = serializedObject.FindProperty("m_LocalizationImage.m_SpriteSizev2");
	        m_LocalSpritePaths = serializedObject.FindProperty("m_LocalizationImage.m_LocalSpritePaths");
	        m_LoadSpriteSizev2 = serializedObject.FindProperty("m_LocalizationImage.m_LoadSpriteSizev2");
			m_ImageName = serializedObject.FindProperty("m_LocalizationImage.m_ImageName");

			//ImageMask
			m_IsUseMask = serializedObject.FindProperty("m_ImageMaskExtend.m_IsUseMaskImage");
	        m_FillPercent = serializedObject.FindProperty("m_ImageMaskExtend.m_FillPercent");
	        m_Fill = serializedObject.FindProperty("m_ImageMaskExtend.m_Fill");
	        m_TrisCont = serializedObject.FindProperty("m_ImageMaskExtend.m_TrisCont");
	        m_Segements = serializedObject.FindProperty("m_ImageMaskExtend.m_Segements");
	
	        m_ImageMaskPanelOpen = EditorPrefs.GetBool("UGUIPro.m_ImageMaskPanelOpen", m_ImageMaskPanelOpen);
	        m_LocalizationPanelOpen = EditorPrefs.GetBool("UGUIPro.m_LocalizationPanelOpen", m_LocalizationPanelOpen);
	    }
	    public override void OnInspectorGUI()
	    {
	        base.OnInspectorGUI();
	        ImageProGUI();
	
	        serializedObject.ApplyModifiedProperties();
	    }
	    void ImageProGUI()
	    {
	        ImageProDrawEditor.LocalizationGUI(GetTitle(0), ref m_LocalizationPanelOpen, 0, m_IsUseLocalization, m_LocalizationImageType, m_LocalImageLoadType, m_LocalSprites, m_LocalSpritePaths, m_SpriteSizev2, m_LoadSpriteSizev2, m_ImageName);
	        ImageProDrawEditor.DrawImageMask(GetTitle(1), ref m_ImageMaskPanelOpen, m_IsUseMask, m_FillPercent, m_Fill, m_TrisCont, m_Segements);
	        if (GUI.changed)
	        {
	            EditorPrefs.SetBool("UGUIPro.m_ImageMaskPanelOpen", m_ImageMaskPanelOpen);
	            EditorPrefs.SetBool("UGUIPro.m_LocalizationPanelOpen", m_LocalizationPanelOpen);
	        }
	    }
	
	    string GetTitle(int index)
	    {
	        return UGUIProSetting.EditorLanguageType == 0 ? titles_E[index] : titles_C[index];
	    }
	}
}
