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

 namespace ZM.UGUIPro {	
	[System.Serializable]
	public enum LocalizationImageType
	{
	    Static,
	    DynamicLoad,
	}
	[System.Serializable]
	public enum LocalImageLoadType
	{ 
	    Resources,
	    AssetBundle,
	}
	 
	[System.Serializable]
	public class LocalizationImageExtend
	{
	    [SerializeField]
	    private bool m_IsUseLocalization;
	    public bool IsUseLocalization { get { return m_IsUseLocalization; } set { m_IsUseLocalization = value; } }
	    [SerializeField]
	    private LocalizationImageType m_LocalizationImageType = LocalizationImageType.Static;
	    [SerializeField]
	    private LocalImageLoadType m_LocalImageLoadType = LocalImageLoadType.AssetBundle;
	    [SerializeField]
	    private Sprite[] m_LocalSprites = null;
	    [SerializeField]
	    private Vector2[] m_SpriteSizev2 = null;
	    [SerializeField]
	    private string[] m_LocalSpritePaths = null;
	    [SerializeField]
	    private Vector2[] m_LoadSpriteSizev2 = null;
		[SerializeField]
		private string m_ImageName = null;
	
	
	
	    private ImagePro m_ImagePro;
	
	    public void Initializa(ImageProBase imagePro)
	    {
	        m_ImagePro = (ImagePro)imagePro;
            LocalizationManager.Instance.AddLanguageChangeListener(UpdateImage);
        }
	    public void UpdateImage()
	    {
	        if (m_IsUseLocalization == false)
	            return;
	        if (m_ImagePro != null)
	        {
	            int index = LocalizationManager.Instance.GetLocalizationImageIndex();
	            Vector2 spriteSize = Vector2.zero;
	            switch (m_LocalizationImageType)
	            {
	                case LocalizationImageType.Static:
	                    m_ImagePro.sprite = m_LocalSprites[index];
	                    spriteSize = m_SpriteSizev2[index];
	                    break;
	                case LocalizationImageType.DynamicLoad:
	                    switch (m_LocalImageLoadType)
	                    {
	                        case LocalImageLoadType.Resources:
	                            m_ImagePro.sprite = Resources.Load<Sprite>(m_LocalSpritePaths[index]);
	                            spriteSize = m_LoadSpriteSizev2[index];
	                            break;
	                        case LocalImageLoadType.AssetBundle:
								var languageType = LocalizationManager.Instance.GetLanguageTypeName();
								string bundlePath = "";
								var atlasPath = bundlePath + languageType + "/" + languageType;
#if UNITY_EDITOR
								m_ImagePro.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(atlasPath + "/" + m_ImageName + ".png");
#else
								m_ImagePro.sprite = AssetsManager.Instance.LoadAtlasSprite(atlasPath, m_ImageName);
#endif
								break;
	                    }
	                    break;
	            }
	            m_ImagePro.rectTransform.sizeDelta = spriteSize == Vector2.zero ? m_ImagePro.sprite.rect.size : spriteSize;
	        }
	    }
	    public void Release()
	    {
            LocalizationManager.Instance.RemoveLanguageChangeListener(UpdateImage);
        }
	}
	
}
