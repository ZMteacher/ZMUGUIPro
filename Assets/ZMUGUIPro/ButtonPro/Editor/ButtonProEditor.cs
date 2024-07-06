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
using UnityEditor.UI;
using UnityEngine;

 namespace ZM.UGUIPro {	
	
	[CustomEditor(typeof(ButtonPro), true)]
	[CanEditMultipleObjects]
	public class ButtonProEditor : ButtonEditor
	{
	    private string[] titles_E = { "双击模式", "长按模式" };
	    private string[] titles_C = { "Double Click Mode", "Long Press Mode" };
	
	    private static bool m_DoubleClickPanelOpen = false;
	    private static bool m_LongPressPanelOpen = false;
 
		SerializedProperty m_ClickInterval;
	    SerializedProperty m_IsUseDoubleClick;
	    SerializedProperty m_ButtonClickedEvent;
	
	
	    SerializedProperty m_IsUseLongPress;
	    SerializedProperty m_Duration;
	    SerializedProperty m_ButtonLongPressEvent;

		SerializedProperty m_IsUsePressScale;
		SerializedProperty m_NormalScale;
		SerializedProperty m_PressScale;

		SerializedProperty m_IsUseClickAudio;


		protected override void OnEnable()
	    {
	        base.OnEnable();
	        m_ClickInterval = serializedObject.FindProperty("m_ButtonDoubleClickExtend.m_ClickInterval");
	        m_IsUseDoubleClick = serializedObject.FindProperty("m_ButtonDoubleClickExtend.m_IsUseDoubleClick");
	        m_ButtonClickedEvent = serializedObject.FindProperty("m_ButtonDoubleClickExtend.m_ButtonClickedEvent");
	
	        m_IsUseLongPress = serializedObject.FindProperty("m_ButtonLongPressExtend.m_IsUseLongPress");
	        m_Duration = serializedObject.FindProperty("m_ButtonLongPressExtend.m_Duration");
	        m_ButtonLongPressEvent = serializedObject.FindProperty("m_ButtonLongPressExtend.m_ButtonLongPressEvent");

            m_DoubleClickPanelOpen = EditorPrefs.GetBool("UGUIPro.m_DoubleClickPanelOpen", m_DoubleClickPanelOpen);
            m_LongPressPanelOpen = EditorPrefs.GetBool("UGUIPro.m_LongPressPanelOpen", m_LongPressPanelOpen);

			m_IsUsePressScale= serializedObject.FindProperty("m_ButtonScaleExtend.m_IsUsePressScale");
			m_NormalScale = serializedObject.FindProperty("m_ButtonScaleExtend.m_NormalScale");
			m_PressScale = serializedObject.FindProperty("m_ButtonScaleExtend.m_PressScale");

			m_IsUseClickAudio = serializedObject.FindProperty("m_ButtonAudioExtend.m_IsUseClickAudio");
		}
	
	
	    public override void OnInspectorGUI()
	    {
	        base.OnInspectorGUI();
	        serializedObject.Update();
	        ButtonProGUI();
	        serializedObject.ApplyModifiedProperties();
	    }
	    public void ButtonProGUI()
	    {
	
	        ButtonProDrawEditor.DrawDoubleClickGUI(GetTitle(0), ref m_DoubleClickPanelOpen, m_IsUseDoubleClick, m_ClickInterval, m_ButtonClickedEvent);
	        ButtonProDrawEditor.DrawLongPressGUI(GetTitle(1), ref m_LongPressPanelOpen,m_IsUseLongPress,m_Duration,m_ButtonLongPressEvent );
			ButtonProDrawEditor.DrawDoubleClickGUI("按下缩放", ref m_LongPressPanelOpen, m_IsUsePressScale, m_NormalScale, m_PressScale);
			ButtonProDrawEditor.DrawSignGUI("按钮音效", ref m_LongPressPanelOpen, m_IsUseClickAudio);
			
			if (GUI.changed)
	        {
	            EditorPrefs.SetBool("UGUIPro.m_DoubleClickPanelOpen", m_DoubleClickPanelOpen);
	            EditorPrefs.SetBool("UGUIPro.m_LongPressPanelOpen", m_LongPressPanelOpen);
	        }
	    }
	    string GetTitle(int index) { return UGUIProSetting.EditorLanguageType == 0 ? titles_E[index] : titles_C[index]; }
	}
}
