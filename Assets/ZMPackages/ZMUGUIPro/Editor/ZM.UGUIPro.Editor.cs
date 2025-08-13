using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ZM.UGUIPro;

public class ZMUGUIProEditor 
{
    #region UI组件创建

    [MenuItem("GameObject/ZM UGUIPro/Image Pro",priority = 0)]
    public static void CreateImagePro()
    {
        GameObject root = new GameObject("Image Pro", typeof(RectTransform), typeof(ImagePro));
        ResetInCanvasFor((RectTransform)root.transform);
	
        root.transform.localPosition = Vector3.zero;
    }
    [MenuItem("GameObject/ZM UGUIPro/RawImage Pro",priority = 0)]
    public static void CreateRawImagePro()
    {
        GameObject root = new GameObject("RawImage Pro", typeof(RectTransform), typeof(RawImagePro));
        ResetInCanvasFor((RectTransform)root.transform);
	
        root.transform.localPosition = Vector3.zero;
    }
    [MenuItem("GameObject/ZM UGUIPro/Mirror Image",priority = 1)]
    public static void CreateMirrorImage()
    {
        GameObject root = new GameObject("Mirror Image", typeof(RectTransform), typeof(MirrorImage));
        ResetInCanvasFor((RectTransform)root.transform);

        root.transform.localPosition = Vector3.zero;
    }
   
    [MenuItem("GameObject/ZM UGUIPro/Fillet Image",priority = 2)]
    public static void CreateFilletImage()
    {
        GameObject root = new GameObject("Fillet Image", typeof(RectTransform), typeof(FilletImage));
        ResetInCanvasFor((RectTransform)root.transform);
        root.transform.localPosition = Vector3.zero;
    }
    
    [MenuItem("GameObject/ZM UGUIPro/Text Pro",priority =3)]
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
    
    [MenuItem("GameObject/ZM UGUIPro/TextMesh Pro",priority = 4)]
    public static void CreateTextMeshPro()
    {
        GameObject root = new GameObject("TextMeshPro", typeof(RectTransform), typeof(TextMeshPro));
        ResetInCanvasFor((RectTransform)root.transform);
        root.GetComponent<TextMeshPro>().text = "TextMeshPro";
        var text = root.GetComponent<TextMeshPro>();
        text.text = "TextMesh Pro";
        text.color = Color.white;
        text.raycastTarget = false;
        text.rectTransform.sizeDelta = new Vector2(200, 50);
        text.fontSize = 24;
        text.alignment = TMPro.TextAlignmentOptions.Midline;
        root.transform.localPosition = Vector3.zero;
    }
    [MenuItem("GameObject/ZM UGUIPro/Button Pro",priority = 5)]
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
    #endregion

    #region Canvas获取

    /// <summary>
    /// 重置Canvas环境
    /// </summary>
    /// <param name="root"></param>
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
    /// <summary>
    /// 是否在Canvas中
    /// </summary>
    /// <param name="tf"></param>
    /// <returns></returns>
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
	
    /// <summary>
    /// 获取并创建Canvas
    /// </summary>
    /// <returns></returns>
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

    #endregion
    
}
