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
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

 namespace ZM.UGUIPro {
	public class ImageProDrawEditor
	{
        [MenuItem("GameObject/UI/Image Pro")]
	    public static void CreateTextPro()
	    {
	        GameObject root = new GameObject("Image Pro", typeof(RectTransform), typeof(ImagePro));
	        ResetInCanvasFor((RectTransform)root.transform);
	
	        root.transform.localPosition = Vector3.zero;
	    }
        [MenuItem("GameObject/UI/FilletImage")]
        public static void CreateFilletImage()
        {
            GameObject root = new GameObject("Fillet Image", typeof(RectTransform), typeof(FilletImage));
            ResetInCanvasFor((RectTransform)root.transform);
            root.transform.localPosition = Vector3.zero;
        }

        public static T EditorAssetLaod<T>(string path,bool isAssetbundle) where T : Object
	    {
	        return isAssetbundle ? AssetDatabase.LoadAssetAtPath<T>(path):Resources.Load<T>(path);
	    }
	    
	    public static void DrawImageMask(string title, ref bool m_PanelOpen, SerializedProperty useMask, SerializedProperty fillPercent, SerializedProperty fill, SerializedProperty triscont, SerializedProperty segements)
	    {
	        LayoutFrameBox(() =>
	        {
	            EditorGUILayout.PropertyField(useMask);
	            if (useMask.boolValue)
	            {
	                EditorGUILayout.PropertyField(fillPercent);
	                EditorGUILayout.PropertyField(fill);
	                EditorGUILayout.PropertyField(triscont);
	                EditorGUILayout.PropertyField(segements);
	            }
	        }, title, ref m_PanelOpen, true);
	    }
	
	    static string[] languages = null;
	    public static void LocalizationGUI(string title, ref bool m_PanelOpen, float space, SerializedProperty useThis, SerializedProperty ImageLocalType, SerializedProperty ImageLoadType,
	        SerializedProperty spriteDatas, SerializedProperty spritePaths, SerializedProperty staticSpritesizesv2, SerializedProperty LoadSpriteSizev2, SerializedProperty m_ImageName)
	    {
	        LayoutFrameBox(() =>
	        {
	            EditorGUILayout.PropertyField(useThis);
	            if (useThis.boolValue)
	            {
	                if (languages == null)
	                    languages = System.Enum.GetNames(typeof(LanguageType));
	                EditorGUILayout.PropertyField(ImageLocalType);
	
	                if (ImageLocalType.enumValueIndex == 0)
	                {
	
	                    HnadleLanguagesData(spriteDatas, staticSpritesizesv2);
	
	                    for (int i = 0; i < languages.Length; i++)
	                    {
                           

                            if (spriteDatas.arraySize>i	&&spriteDatas.GetArrayElementAtIndex(i)!=null)
							{
                                EditorGUILayout.PropertyField(spriteDatas.GetArrayElementAtIndex(i), new GUIContent { text = languages[i] });
                            }

	                    }
	
	                    for (int i = 0; i < spriteDatas.arraySize; i++)
	                    {
	                        if (spriteDatas.GetArrayElementAtIndex(i).objectReferenceValue != null)
	                        {
	
	                            EditorGUILayout.BeginHorizontal();
	
	                            Vector2 vectSize2 = DrawImageList(staticSpritesizesv2, i);
	
	                            Sprite sprite = (Sprite)spriteDatas.GetArrayElementAtIndex(i).objectReferenceValue;
	                            string path = AssetDatabase.GetAssetPath(sprite);
	
	                            if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture2D>(path), new GUIStyle() { contentOffset = new Vector2(10, 0) }, GUILayout.Height(140), GUILayout.Width(140)))
	                            {
	                                ImagePro imagePro = Selection.activeTransform.GetComponent<ImagePro>();
	
	                                imagePro.sprite = imagePro.overrideSprite = sprite;
	                                imagePro.GetComponent<RectTransform>().sizeDelta = Vector2.zero != vectSize2 ? vectSize2 : sprite.rect.size;
									imagePro.GraphicUpdateComplete();
									imagePro.enabled = false;
									imagePro.enabled =true;
								}
	
	                            EditorGUILayout.EndHorizontal();
	                        }
	                    }
	                    DrawUnLoadOtherAssetsButton(spritePaths,LoadSpriteSizev2);
	                }
	                else if (ImageLocalType.enumValueIndex == 1)
	                {
	                    
	                    EditorGUILayout.PropertyField(ImageLoadType);
	                    HnadleLanguagesData(spritePaths, LoadSpriteSizev2);

						if(ImageLoadType.enumValueIndex == 0)
                        {
							for (int i = 0; i < languages.Length; i++)
							{
								SerializedProperty spritePath = spritePaths.GetArrayElementAtIndex(i);
								if (string.IsNullOrEmpty(spritePath.stringValue))
									spritePath.stringValue = "Plase Set " + languages[i] + " Image Path...";
								EditorGUILayout.PropertyField(spritePath, new GUIContent { text = languages[i] });
								if (spritePath.stringValue != ("Plase Set " + languages[i] + " Image Path..."))
								{
									Texture2D texture2D = ImageLoadType.enumValueIndex == 0 ? EditorAssetLaod<Texture2D>(spritePath.stringValue, false) : EditorAssetLaod<Texture2D>(spritePath.stringValue, true);
									if (texture2D == null)
										EditorGUILayout.HelpBox(languages[i] + "   path  not find!", MessageType.Error);
								}
							}

							for (int i = 0; i < languages.Length; i++)
							{
								SerializedProperty spritePath = spritePaths.GetArrayElementAtIndex(i);

								if (spritePath.stringValue.Contains("Plase Set")) continue;

								EditorGUILayout.BeginHorizontal();

								Vector2 vectSize2 = vectSize2 = DrawImageList(LoadSpriteSizev2, i);

								Texture2D texture = ImageLoadType.enumValueIndex == 0 ? EditorAssetLaod<Texture2D>(spritePath.stringValue, false) : EditorAssetLaod<Texture2D>(spritePath.stringValue, true);
								if (GUILayout.Button(texture, new GUIStyle() { contentOffset = new Vector2(10, 0) }, GUILayout.Height(140), GUILayout.Width(140)))
								{
									ImagePro imagePro = Selection.activeTransform.GetComponent<ImagePro>();
									Sprite sprite = EditorAssetLaod<Sprite>(spritePath.stringValue, ImageLoadType.enumValueIndex == 1);
									imagePro.sprite = imagePro.overrideSprite = sprite;
									imagePro.rectTransform.sizeDelta = Vector2.zero != vectSize2 ? vectSize2 : sprite.rect.size;
								}
								EditorGUILayout.EndHorizontal();
							}

							DrawUnLoadOtherAssetsButton(spriteDatas, staticSpritesizesv2);
						}
                        else
                        {
							EditorGUILayout.PropertyField(m_ImageName);
						}
	                }
	            }
	            else
	            {
	                if (spriteDatas != null)
	                    spriteDatas = null;
	                if (spritePaths != null)
	                    spritePaths = null;
	            }
	        }, title, ref m_PanelOpen, true);
	    }
	    public static void DrawUnLoadOtherAssetsButton(SerializedProperty array1,SerializedProperty array2)
	    {
	        if (array1 != null && array1.arraySize > 0 || array2 != null && array2.arraySize > 0)
	        {
	            if (GUILayout.Button("UnLoad Other Assets", GUI.skin.GetStyle("flow node 3 on"), GUILayout.ExpandWidth(true), GUILayout.Height(27)))
	            {
	                array1.ClearArray();
	                array2.ClearArray();
	                array1 = null;
	                array2 = null;
	            }
	        }
	        else
	        {
	            GUILayout.Button("UnLoad Other Assets", GUILayout.ExpandWidth(true), GUILayout.Height(27));
	        }
	        GUILayout.Space(6);
	    }
	    public static void HnadleLanguagesData(SerializedProperty spriteProperty, SerializedProperty spriteSizesProperty)
	    {
	        if (spriteProperty == null || spriteProperty.arraySize == 0)
	            spriteProperty.arraySize = languages.Length;
	        else if (spriteProperty.arraySize > languages.Length) //Hande languages change
	        {
	            int count = spriteProperty.arraySize - languages.Length;
	            for (int i = 0; i < count; i++)
	                spriteProperty.DeleteArrayElementAtIndex(spriteProperty.arraySize - (i + 1));
	        }
	
	        if (spriteSizesProperty == null || spriteSizesProperty.arraySize == 0)
	        {
	            for (int i = 0; i < languages.Length; i++)
	                spriteSizesProperty.InsertArrayElementAtIndex(i);
	        }
	        else if (spriteSizesProperty.arraySize > languages.Length) //Hande languages change
	        {
	            int count = spriteSizesProperty.arraySize - languages.Length;
	            for (int i = 0; i < count; i++)
	                spriteSizesProperty.DeleteArrayElementAtIndex(spriteSizesProperty.arraySize - (i + 1));
	        }
	    }
	    private static Vector2 DrawImageList(SerializedProperty spriteSizes, int i)
	    {
	        EditorGUILayout.BeginVertical(GUILayout.Width(100));
	        for (int j = 0; j < 12; j++)
	        {
	            EditorGUILayout.Space();
	        }
	        EditorGUILayout.BeginHorizontal();
	        GUILayout.Label("Width");
	        EditorGUILayout.Space();
	        EditorGUILayout.Space();
	        EditorGUILayout.Space();
	        GUILayout.Label("Hieght");
	        EditorGUILayout.EndHorizontal();
	
	        EditorGUILayout.BeginHorizontal();
	
	        Vector2 vectSize2 = spriteSizes.GetArrayElementAtIndex(i).vector2Value;
	        vectSize2.x = EditorGUILayout.FloatField(vectSize2.x, GUILayout.Width(35));
	        GUILayout.Label("x", GUILayout.Width(20));
	        vectSize2.y = EditorGUILayout.FloatField(vectSize2.y, GUILayout.Width(35));
	        spriteSizes.GetArrayElementAtIndex(i).vector2Value = vectSize2;
	
	        EditorGUILayout.EndHorizontal();
	        EditorGUILayout.EndVertical();
	
	        return vectSize2;
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
	    public static void SendWillRenderCanvases()
	    {
	        GameObject obj = Selection.activeTransform.gameObject;
	        obj.SetActive(false);
	        obj.SetActive(true);
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
