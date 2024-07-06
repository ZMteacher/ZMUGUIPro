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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZM.UGUIPro
{
    public enum GradientType
    {
        OneColor = 0,
        TwoColor = 1,
        ThreeColor = 2
    }

    [DisallowMultipleComponent]
    [Serializable]
    public class TextEffect : BaseMeshEffect
    {
        private const string OutLineShaderName = "TextPro/Text";

        private bool m_InitedParams;

        [SerializeField]
        [HideInInspector]
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
        [SerializeField] [HideInInspector] private float m_LerpValue = 0;
        [SerializeField] [HideInInspector] private bool m_OpenShaderOutLine;
        [SerializeField] [HideInInspector] private TextProOutLine m_OutlineEx;
        [SerializeField] [HideInInspector] private bool m_EnableOutLine = false;
        [SerializeField] [HideInInspector] private float m_OutLineWidth = 1;
        [SerializeField] [HideInInspector] private GradientType m_GradientType = GradientType.TwoColor;
        [SerializeField] [HideInInspector] private Color32 m_TopColor = Color.white;
        [SerializeField] [HideInInspector] private Color32 m_MiddleColor = Color.white;
        [SerializeField] [HideInInspector] private Color32 m_BottomColor = Color.white;
        [SerializeField] [HideInInspector] private Color32 m_OutLineColor = Color.black;
        [SerializeField] [HideInInspector] private Camera m_Camera;
        [SerializeField, UnityEngine.Range(0, 1)] [HideInInspector] private float m_Alpha = 1;
        [UnityEngine.Range(0.1f, 0.9f)] [SerializeField] [HideInInspector] private float m_ColorOffset = 0.5f;


        private List<UIVertex> iVertices = new List<UIVertex>();
        private Vector3[] m_OutLineDis = new Vector3[4];

        private Text m_Text;

        public Text TextGraphic
        {
            get
            {
                if (!this.m_Text && base.graphic)
                {
                    this.m_Text = base.graphic as Text;
                }
                else
                {
                    if (!base.graphic)
                        throw new Exception("No Find base Graphic!!");
                }

                return this.m_Text;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (!string.IsNullOrEmpty(this.TextGraphic.text))
            {
                this.UpdateOutLineInfos();
            }
            this.hideFlags = HideFlags.HideInInspector;
        }
        public void SetUserEffect(bool isUseTextEffect)
        {
            m_UseTextEffect = isUseTextEffect;
        }
        public void SetLerpValue(float lerpValue)
        {
            m_LerpValue = lerpValue;
        }
        public void SetCamera(Camera camera)
        {
            if (m_Camera == camera) return;
            this.m_Camera = camera;
        }
        public void SetGradientType(GradientType gradientType)
        {
            this.m_GradientType = gradientType;
        }

        public GradientType GetGradientType()
        {
            return m_GradientType;
        }

        public void SetTopColor(Color topColor)
        {
            this.m_TopColor = topColor;
        }
        public Color GetTopColor()
        {
            return m_TopColor;
        }

        public void SetMiddleColor(Color middleColor)
        {
            this.m_MiddleColor = middleColor;
        }

        public void SetBottomColor(Color bottomColor)
        {
            this.m_BottomColor = bottomColor;
        }

        public void SetEnableOutline(bool enable)
        {
            if (this.m_EnableOutLine == enable) return;
            this.m_EnableOutLine = enable;
        }

        public void SetColorOffset(float colorOffset)
        {

            this.m_ColorOffset = colorOffset;
        }

        public void SetOutLineColor(Color outLineColor)
        {
            this.m_OutLineColor = outLineColor;
            if (base.graphic && this.m_OutlineEx)
            {
                this.m_OutlineEx.SetOutLineColor(this.m_OutLineColor);
                base.graphic.SetAllDirty();
            }

        }

        public void SetOutLineWidth(float outLineWidth)
        {
            this.m_OutLineWidth = outLineWidth;
            if (base.graphic && this.m_OutlineEx)
            {
                this.m_OutlineEx.SetOutLineWidth(this.m_OutLineWidth);
                base.graphic.SetAllDirty();
            }
        }

        public void SetAlpah(float setAlphaValue)
        {
            this.m_Alpha = setAlphaValue;
            byte alphaByte = (byte)(this.m_Alpha * 255);
            this.m_TopColor.a = alphaByte;
            this.m_BottomColor.a = alphaByte;
            this.m_MiddleColor.a = alphaByte;
            this. m_OutLineColor.a = alphaByte;
            if (base.graphic && this.m_OutlineEx) {
                base.graphic.SetAllDirty();
            }
        }

        public void SetShaderOutLine(bool outlineUseShader)
        {
            if (!m_UseTextEffect) return;
            if (!this.m_OutlineEx)
            {
                this.m_OutlineEx = this.gameObject.GetComponent<TextProOutLine>();
                if (!this.m_OutlineEx)
                    this.m_OutlineEx = this.gameObject.AddComponent<TextProOutLine>();
                this.m_OutlineEx.graphic = base.graphic;
            }
            else
            {
                this.m_OutlineEx.enabled = true;
            }
            this.m_OutlineEx.hideFlags = HideFlags.HideInInspector;
            this.m_OpenShaderOutLine = outlineUseShader;
            this.UpdateOutLineInfos();
        }

        public void UpdateOutLineInfos()
        {
            if (!m_UseTextEffect) return;
            if (!this.m_OutlineEx) return;
            this.m_OutlineEx.SwitchShaderOutLine(this.m_OpenShaderOutLine);
            this.m_OutlineEx.SetUseThree(this.m_GradientType == GradientType.ThreeColor);
            this.m_OutlineEx.SetOutLineColor(this.m_OutLineColor);
            this.m_OutlineEx.SetOutLineWidth(this.m_OutLineWidth);
            this.UpdateOutLineMaterial();
            if (base.graphic != null)
            {
                this.OpenShaderParams();
                base.graphic.SetAllDirty();
            }
        }


        private void OpenShaderParams()
        {
            if (!m_UseTextEffect) return;
            if (base.graphic && !this.m_InitedParams)
            {
                if (base.graphic.canvas)
                {
                    var v1 = graphic.canvas.additionalShaderChannels;
                    var v2 = AdditionalCanvasShaderChannels.TexCoord1;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }

                    v2 = AdditionalCanvasShaderChannels.TexCoord2;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }

                    v2 = AdditionalCanvasShaderChannels.TexCoord3;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }

                    v2 = AdditionalCanvasShaderChannels.Tangent;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }

                    v2 = AdditionalCanvasShaderChannels.Normal;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }
                    this.m_InitedParams = true;
                }
            }
        }

        private void _ProcessVertices(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }
            if (!m_UseTextEffect) return;
            var count = vh.currentVertCount;
            if (count == 0)
                return;

            /*
             *  TL--------TR
             *  |          |^
             *  |          ||
             *  CL--------CR
             *  |          ||
             *  |          |v
             *  BL--------BR
             * **/


            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = UIVertex.simpleVert;
                vh.PopulateUIVertex(ref vertex, i);
                this.iVertices.Add(vertex);
            }
            vh.Clear();

            for (int i = 0; i < this.iVertices.Count; i += 4)
            {

                UIVertex TL = GeneralUIVertex(this.iVertices[i + 0]);
                UIVertex TR = GeneralUIVertex(this.iVertices[i + 1]);
                UIVertex BR = GeneralUIVertex(this.iVertices[i + 2]);
                UIVertex BL = GeneralUIVertex(this.iVertices[i + 3]);

                //先绘制上四个
                UIVertex CR = default(UIVertex);
                UIVertex CL = default(UIVertex);

                //如果是OneColor模式，则颜色不做二次处理
                if (this.m_GradientType == GradientType.OneColor)
                {

                }
                else
                {
                    TL.color = this.m_TopColor;
                    TR.color = this.m_TopColor;
                    BL.color = this.m_BottomColor;
                    BR.color = this.m_BottomColor;
                }


                if (this.m_EnableOutLine)
                {

                    if (!this.m_OpenShaderOutLine)
                    {
                        if (this.m_OutlineEx)
                        {
                            this.m_OutlineEx.enabled = false;
                        }

                        this.m_OutLineDis[0].Set(-this.m_OutLineWidth, this.m_OutLineWidth, 0); //LT
                        this.m_OutLineDis[1].Set(this.m_OutLineWidth, this.m_OutLineWidth, 0); //RT
                        this.m_OutLineDis[2].Set(-this.m_OutLineWidth, -this.m_OutLineWidth, 0); //LB
                        this.m_OutLineDis[3].Set(this.m_OutLineWidth, -this.m_OutLineWidth, 0); //RB


                        for (int j = 0; j < 4; j++)
                        {
                            //四个方向
                            UIVertex o_TL = GeneralUIVertex(TL);
                            UIVertex o_TR = GeneralUIVertex(TR);
                            UIVertex o_BR = GeneralUIVertex(BR);
                            UIVertex o_BL = GeneralUIVertex(BL);


                            o_TL.position += this.m_OutLineDis[j];
                            o_TR.position += this.m_OutLineDis[j];
                            o_BR.position += this.m_OutLineDis[j];
                            o_BL.position += this.m_OutLineDis[j];

                            o_TL.color = this.m_OutLineColor;
                            o_TR.color = this.m_OutLineColor;
                            o_BR.color = this.m_OutLineColor;
                            o_BL.color = this.m_OutLineColor;

                            vh.AddVert(o_TL);
                            vh.AddVert(o_TR);

                            if (this.m_GradientType == GradientType.ThreeColor)
                            {
                                UIVertex o_CR = default(UIVertex);
                                UIVertex o_CL = default(UIVertex);

                                o_CR = GeneralUIVertex(this.iVertices[i + 2]);
                                o_CL = GeneralUIVertex(this.iVertices[i + 3]);
                                //var New_S_Point = this.ConverPosition(o_TR.position, o_BR.position, this.m_ColorOffset);

                                o_CR.position.y = Mathf.Lerp(o_TR.position.y, o_BR.position.y, this.m_ColorOffset);
                                o_CL.position.y = Mathf.Lerp(o_TR.position.y, o_BR.position.y, this.m_ColorOffset);

                                if (Mathf.Approximately(TR.uv0.x, BR.uv0.x))
                                {
                                    o_CR.uv0.y = Mathf.Lerp(TR.uv0.y, BR.uv0.y, this.m_ColorOffset);
                                    o_CL.uv0.y = Mathf.Lerp(TL.uv0.y, BL.uv0.y, this.m_ColorOffset);
                                }
                                else
                                {
                                    o_CR.uv0.x = Mathf.Lerp(TR.uv0.x, BR.uv0.x, this.m_ColorOffset);
                                    o_CL.uv0.x = Mathf.Lerp(TL.uv0.x, BL.uv0.x, this.m_ColorOffset);
                                }

                                o_CR.color = this.m_OutLineColor;
                                o_CL.color = this.m_OutLineColor;


                                vh.AddVert(o_CR);
                                vh.AddVert(o_CL);
                            }

                            vh.AddVert(o_BR);
                            vh.AddVert(o_BL);
                        }
                    }
                }

                if (this.m_GradientType == GradientType.ThreeColor && this.m_EnableOutLine && this.m_OpenShaderOutLine)
                {
                    UIVertex t_TL = GeneralUIVertex(TL);
                    UIVertex t_TR = GeneralUIVertex(TR);
                    UIVertex t_BR = GeneralUIVertex(BR);
                    UIVertex t_BL = GeneralUIVertex(BL);

                    // t_TL.color.a = 0;
                    // t_TR.color.a = 0;
                    // t_BR.color.a = 0;
                    // t_BL.color.a = 0;
                    //vh.AddVert(t_TL);
                    //vh.AddVert(t_TR);

                    //vh.AddVert(t_BR);
                    //vh.AddVert(t_BL);
                }

                vh.AddVert(TL);
                vh.AddVert(TR);

                if (this.m_GradientType == GradientType.ThreeColor)
                {
                    CR = GeneralUIVertex(this.iVertices[i + 2]);
                    CL = GeneralUIVertex(this.iVertices[i + 3]);
                    //var New_S_Point = this.ConverPosition(TR.position, BR.position, this.m_ColorOffset);

                    CR.position.y = Mathf.Lerp(TR.position.y, BR.position.y - 0.1f, this.m_ColorOffset);
                    CL.position.y = Mathf.Lerp(TR.position.y, BR.position.y, this.m_ColorOffset);



                    if (Mathf.Approximately(TR.uv0.x, BR.uv0.x))
                    {
                        CR.uv0.y = Mathf.Lerp(TR.uv0.y, BR.uv0.y, this.m_ColorOffset);
                        CL.uv0.y = Mathf.Lerp(TL.uv0.y, BL.uv0.y, this.m_ColorOffset);
                    }
                    else
                    {
                        CR.uv0.x = Mathf.Lerp(TR.uv0.x, BR.uv0.x, this.m_ColorOffset);
                        CL.uv0.x = Mathf.Lerp(TL.uv0.x, BL.uv0.x, this.m_ColorOffset);
                    }


                    CR.color = this.m_MiddleColor;
                    CL.color = this.m_MiddleColor;
                    // CR.color = Color32.Lerp(this.m_MiddleColor, this.m_BottomColor, this.m_LerpValue);
                    // CL.color = Color32.Lerp(this.m_MiddleColor, this.m_BottomColor, this.m_LerpValue);
                    vh.AddVert(CR);
                    vh.AddVert(CL);
                }

                //绘制下四个
                if (this.m_GradientType == GradientType.ThreeColor)
                {
                    vh.AddVert(CL);
                    vh.AddVert(CR);
                }
                vh.AddVert(BR);
                vh.AddVert(BL);




            }

            for (int i = 0; i < vh.currentVertCount; i += 4)
            {
                vh.AddTriangle(i + 0, i + 1, i + 2);
                vh.AddTriangle(i + 2, i + 3, i + 0);
            }
        }
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!m_UseTextEffect) return;
            this.iVertices.Clear();
            //if (m_Text.text.Equals("Bonus"))
            //{
            //    Debuger.ColorLog(LogColor.Yellow, "Bonus>>>>>>>>>>>>>Start>>>>>>>>>>>>>>Bonus>>>>>>");
            //}
            this._ProcessVertices(vh);

            if (this.m_EnableOutLine && this.m_OutlineEx)
            {
                this.m_OutlineEx.ModifyMesh(vh);
            }
            //if (m_Text.text.Equals("Bonus"))
            //{
            //    Debuger.ColorLog(LogColor.Cyan, "Bonus>>>>>>>>>>>>>End>>>>>>>>>>>>>>Bonus>>>>>>");
            //}
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (this.m_OpenShaderOutLine)
            {
                if (!m_UseTextEffect) return;
                this.UpdateOutLineMaterial();
                this.Refresh();
            }
        }
#endif

        public void Refresh()
        {
            if (base.graphic)
            {
                base.graphic.SetVerticesDirty();
            }

        }

        private void UpdateOutLineMaterial()
        {
            if (!m_UseTextEffect) return;
#if !UNITY_EDITOR
            if (base.graphic && base.graphic.material == base.graphic.defaultMaterial)
            {
                Shader shader = Shader.Find(OutLineShaderName);
                if (shader)
                {
                    base.graphic.material = new Material(shader);
                }
            }
#else
            if (!Application.isPlaying)
            {
                if (base.graphic && base.graphic.material == base.graphic.defaultMaterial)
                {
                    Material material= UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/ZMUGUIPro/TextPro/Shaders/TextPro-Text.mat");
                    if (material==null)
                        Debug.LogError("Text Out Line Material Not Find  Plase Check Material Path!");
                    base.graphic.material = material;   
                }
            }
            else
            {
                if (base.graphic && base.graphic.material == base.graphic.defaultMaterial)
                {
                    Shader shader = Shader.Find(OutLineShaderName);
                    if (shader)
                    {
                        base.graphic.material = new Material(shader);
                    }
                }
            }
#endif


            if (base.graphic)
            {
                Texture fontTexture = null;
                if (this.TextGraphic)
                {
                    if (this.graphic && this.TextGraphic.font)
                    {
                        fontTexture = this.TextGraphic.font.material.mainTexture;
                    }

                    if (base.graphic.material && base.graphic.material != base.graphic.defaultMaterial)
                        base.graphic.material.mainTexture = fontTexture;
                }
            }
        }

        public static UIVertex GeneralUIVertex(UIVertex vertex)
        {
            UIVertex result = UIVertex.simpleVert;
            result.normal = new Vector3(vertex.normal.x, vertex.normal.y, vertex.normal.z);
            result.position = new Vector3(vertex.position.x, vertex.position.y, vertex.position.z);
            result.tangent = new Vector4(vertex.tangent.x, vertex.tangent.y, vertex.tangent.z, vertex.tangent.w);
            result.uv0 = new Vector2(vertex.uv0.x, vertex.uv0.y);
            result.uv1 = new Vector2(vertex.uv1.x, vertex.uv1.y);
            result.color = vertex.color;
            return result;
        }
    }
}
