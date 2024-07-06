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
namespace ZM.UGUIPro
{

    [System.Serializable]
    public class TextEffectExtend
    {
        [SerializeField]
        private bool m_UseTextEffect;
        public bool UseTextEffect
        {
            get
            {
                return m_UseTextEffect;
            }
            set
            {
                m_UseTextEffect = value;
            }
        }
        public bool UseOutLine { get { return m_EnableOutLine; } }
        [SerializeField] private float m_LerpValue = 0;
        [SerializeField] private bool m_OpenShaderOutLine=true;
        [SerializeField] private TextProOutLine m_OutlineEx;
        [SerializeField] private bool m_EnableOutLine = false;
        [SerializeField] private float m_OutLineWidth = 1;
        [SerializeField] public GradientType m_GradientType = GradientType.OneColor;
        [SerializeField] private Color32 m_TopColor = Color.white;
        [SerializeField] private Color32 m_MiddleColor = Color.white;
        [SerializeField] private Color32 m_BottomColor = Color.white;
        [SerializeField] private Color32 m_OutLineColor = Color.yellow;
        [SerializeField] private Camera m_Camera;
        [SerializeField, UnityEngine.Range(0, 1)] private float m_Alpha = 1;
        [UnityEngine.Range(0.1f, 0.9f)] [SerializeField] private float m_ColorOffset = 0.5f;
        [SerializeField] public TextEffect m_TextEffect;
        public void SaveSerializeData(TextPro TextPro)
        {
            m_TextEffect = TextPro.GetComponent<TextEffect>();
            if (m_TextEffect == null)
            {
                int insid = TextPro.GetInstanceID();

                TextPro[] textProAry = Transform.FindObjectsOfType<TextPro>();
                for (int i = 0; i < textProAry.Length; i++)
                {
                    if (textProAry[i].GetInstanceID() == insid)
                    {
                        m_TextEffect = textProAry[i].gameObject.AddComponent<TextEffect>();
                        m_TextEffect.hideFlags = HideFlags.HideInInspector;
                        break;
                    }
                }
            }

            if (m_Camera == null)
            {
                GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                if (mainCamera != null)
                {
                    m_Camera = mainCamera.GetComponent<Camera>();
                }
                else
                {
                    m_Camera= Transform.FindObjectOfType<Camera>();
                }
            }
            if (m_Camera==null)
            {
                Debug.LogError("not find Main Camera in Scenes");
            }
        }
        public void SetAlpha(float alpha)
        {
            m_TextEffect.SetAlpah(alpha);
        }
    }
}
