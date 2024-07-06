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
--------------------------------------------------------------------*/using System.Linq;
using UnityEngine;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

namespace ZM.UGUIPro
{
    [CustomEditor(typeof(FilletImage), true)]
    //[CanEditMultipleObjects]
    public class FilletImageEditor : ImageEditor
    {

        SerializedProperty m_Radius;
        SerializedProperty m_TriangleNum;
        SerializedProperty m_Sprite;


        protected override void OnEnable()
        {
            base.OnEnable();

            m_Sprite = serializedObject.FindProperty("m_Sprite");
            m_Radius = serializedObject.FindProperty("Radius");
            m_TriangleNum = serializedObject.FindProperty("TriangleNum");

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SpriteGUI();
            AppearanceControlsGUI();
            RaycastControlsGUI();
            bool showNativeSize = m_Sprite.objectReferenceValue != null;
            m_ShowNativeSize.target = showNativeSize;
            NativeSizeButtonGUI();
            EditorGUILayout.PropertyField(m_Radius);
            EditorGUILayout.PropertyField(m_TriangleNum);
            this.serializedObject.ApplyModifiedProperties();
        }




    }
}

