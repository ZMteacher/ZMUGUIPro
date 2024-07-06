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
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace ZM.UGUIPro
{
    /// <summary>
    /// UGUI描边
    /// </summary>
    public class TextProOutLine : UIBehaviour
    {
        [SerializeField] [HideInInspector] public Color OutlineColor = Color.white;
        [SerializeField] [HideInInspector] public Graphic graphic;
        [SerializeField, Range(0, 10)] [HideInInspector] public float OutlineWidth = 0;

        private static List<UIVertex> m_VetexList = new List<UIVertex>();
        private bool m_ThreeColor;
        private bool m_OpenState;
        private float outLineWidth = 0;
        protected override void Awake()
        {
            base.Awake();
            outLineWidth = OutlineWidth;
        }
 

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
            this.hideFlags = HideFlags.HideInInspector;
        }
#endif

        public void SetUseThree(bool useThree)
        {
            this.m_ThreeColor = useThree;
        }

        public void SetOutLineWidth(float setOutlineWidth)
        {
            this.OutlineWidth = setOutlineWidth;
            SetDirty();
        }

        public void SetOutLineColor(Color setOutlineColor)
        {
            this.OutlineColor = setOutlineColor;
            SetDirty();
        }

        public void SetDirty() 
        {
            if (this.graphic != null)
            {
                this.graphic.SetVerticesDirty();
            }
        }

        public void SwitchShaderOutLine(bool switchState)
        {
            this.m_OpenState = switchState;
        }

        public void ModifyMesh(VertexHelper vh)
        {
            if (!this.m_OpenState) return;
            vh.GetUIVertexStream(m_VetexList);

            if (!this.m_ThreeColor)
            {
                this._ProcessVertices();
            }
            else
            {
                _ProcessVertices2();
            }


            vh.Clear();
            vh.AddUIVertexTriangleStream(m_VetexList);
        }


        private void _ProcessVertices2()
        {
            bool isUpRect = false;
            for (int i = 0, count = m_VetexList.Count - 6; i <= count; i += 6)
            {
                isUpRect = i % 12 < 6 ? true : false;
                var v1 = m_VetexList[i];
                var v2 = m_VetexList[i + 1];
                var v3 = m_VetexList[i + 2];

                var v4 = m_VetexList[i + 3];
                var v5 = m_VetexList[i + 4];
                var v6 = m_VetexList[i + 5];

                bool v1_offset = isUpRect ? true : false;
                bool v2_offset = isUpRect ? true : false;
                bool v3_offset = isUpRect ? false : true;
                bool v4_offset = isUpRect ? false : true;
                bool v5_offset = isUpRect ? false : true;
                bool v6_offset = isUpRect ? true : false;

                // 计算原顶点坐标中心点
                //
                var minX = _Min(v1.position.x, v2.position.x, v3.position.x, v4.position.x, v5.position.x, v6.position.x);
                var minY = _Min(v1.position.y, v2.position.y, v3.position.y, v4.position.y, v5.position.y, v6.position.y);
                var maxX = _Max(v1.position.x, v2.position.x, v3.position.x, v4.position.x, v5.position.x, v6.position.x);
                var maxY = _Max(v1.position.y, v2.position.y, v3.position.y, v4.position.y, v5.position.y, v6.position.y);
                var posCenter = new Vector2(minX + maxX, minY + maxY) * 0.5f;
                // 计算原始顶点坐标和UV的方向
                //
                Vector2 triX, triY, uvX, uvY;
                Vector2 triX1, triY1, uvX1, uvY1;

                Vector2 pos1 = v1.position;
                Vector2 pos2 = v2.position;
                Vector2 pos3 = v3.position;

                Vector2 pos4 = v4.position;
                Vector2 pos5 = v5.position;
                Vector2 pos6 = v6.position;

                if (Mathf.Abs(Vector2.Dot((pos2 - pos1).normalized, Vector2.right))
                    > Mathf.Abs(Vector2.Dot((pos3 - pos2).normalized, Vector2.right)))
                {
                    triX = pos2 - pos1;
                    triY = pos3 - pos2;
                    uvX = v2.uv0 - v1.uv0;
                    uvY = v3.uv0 - v2.uv0;
                }
                else
                {
                    triX = pos3 - pos2;
                    triY = pos2 - pos1;
                    uvX = v3.uv0 - v2.uv0;
                    uvY = v2.uv0 - v1.uv0;
                }

                if (Mathf.Abs(Vector2.Dot((pos5 - pos4).normalized, Vector2.right))
                    > Mathf.Abs(Vector2.Dot((pos6 - pos5).normalized, Vector2.right)))
                {
                    triX1 = pos5 - pos4;
                    triY1 = pos6 - pos5;
                    uvX1 = v5.uv0 - v4.uv0;
                    uvY1 = v6.uv0 - v5.uv0;
                }
                else
                {
                    triX1 = pos6 - pos5;
                    triY1 = pos5 - pos4;
                    uvX1 = v6.uv0 - v5.uv0;
                    uvY1 = v5.uv0 - v4.uv0;
                }

                // 计算原始UV框
                var uvMin = _Min(v1.uv0, v2.uv0, v3.uv0, v4.uv0, v5.uv0, v6.uv0);
                var uvMax = _Max(v1.uv0, v2.uv0, v3.uv0, v4.uv0, v5.uv0, v6.uv0);
                //OutlineColor 和 OutlineWidth 也传入，避免出现不同的材质球
                var col_rg = new Vector2(OutlineColor.r, OutlineColor.g); //描边颜色 用uv3 和 tangent的 zw传递
                var col_ba1 = new Vector4(0, 0, OutlineColor.b, OutlineColor.a);
                var col_ba2 = new Vector4(0, 0, OutlineColor.b, OutlineColor.a);
                var col_ba3 = new Vector4(0, 0, OutlineColor.b, OutlineColor.a);
                var col_ba4 = new Vector4(0, 0, OutlineColor.b, OutlineColor.a);
                var col_ba5 = new Vector4(0, 0, OutlineColor.b, OutlineColor.a);
                var col_ba6 = new Vector4(0, 0, OutlineColor.b, OutlineColor.a);
                var normal = new Vector3(0, 0, OutlineWidth); //描边的宽度 用normal的z传递

                // 为每个顶点设置新的Position和UV，并传入原始UV框
                v1 = _SetNewPosAndUV(v1, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax, v1_offset);
                v1.uv3 = col_rg;
                v1.tangent = col_ba1;
                v1.normal = normal;
                v2 = _SetNewPosAndUV(v2, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax, v2_offset);
                v2.uv3 = col_rg;
                v2.tangent = col_ba2;
                v2.normal = normal;
                v3 = _SetNewPosAndUV(v3, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax, v3_offset);
                v3.uv3 = col_rg;
                v3.tangent = col_ba3;
                v3.normal = normal;

                v4 = _SetNewPosAndUV(v4, this.OutlineWidth, posCenter, triX1, triY1, uvX1, uvY1, uvMin, uvMax, v4_offset);
                v4.uv3 = col_rg;
                v4.tangent = col_ba4;
                v4.normal = normal;

                v5 = _SetNewPosAndUV(v5, this.OutlineWidth, posCenter, triX1, triY1, uvX1, uvY1, uvMin, uvMax, v5_offset);
                v5.uv3 = col_rg;
                v5.tangent = col_ba5;
                v5.normal = normal;

                v6 = _SetNewPosAndUV(v6, this.OutlineWidth, posCenter, triX1, triY1, uvX1, uvY1, uvMin, uvMax, v6_offset);
                v6.uv3 = col_rg;
                v6.tangent = col_ba6;
                v6.normal = normal;

                m_VetexList[i] = v1;
                m_VetexList[i + 1] = v2;
                m_VetexList[i + 2] = v3;
                m_VetexList[i + 3] = v4;
                m_VetexList[i + 4] = v5;
                m_VetexList[i + 5] = v6;

            }
        }

        private void _ProcessVertices()
        {
            for (int i = 0, count = m_VetexList.Count - 3; i <= count; i += 3)
            {
                var v1 = m_VetexList[i];
                var v2 = m_VetexList[i + 1];
                var v3 = m_VetexList[i + 2];
                // 计算原顶点坐标中心点
                //
                var minX = _Min(v1.position.x, v2.position.x, v3.position.x);
                var minY = _Min(v1.position.y, v2.position.y, v3.position.y);
                var maxX = _Max(v1.position.x, v2.position.x, v3.position.x);
                var maxY = _Max(v1.position.y, v2.position.y, v3.position.y);
                var posCenter = new Vector2(minX + maxX, minY + maxY) * 0.5f;
                // 计算原始顶点坐标和UV的方向
                //
                Vector2 triX, triY, uvX, uvY;
                Vector2 pos1 = v1.position;
                Vector2 pos2 = v2.position;
                Vector2 pos3 = v3.position;
                if (Mathf.Abs(Vector2.Dot((pos2 - pos1).normalized, Vector2.right))> Mathf.Abs(Vector2.Dot((pos3 - pos2).normalized, Vector2.right)))
                {
                    triX = pos2 - pos1;
                    triY = pos3 - pos2;
                    uvX = v2.uv0 - v1.uv0;
                    uvY = v3.uv0 - v2.uv0;
                }
                else
                {
                    triX = pos3 - pos2;
                    triY = pos2 - pos1;
                    uvX = v3.uv0 - v2.uv0;
                    uvY = v2.uv0 - v1.uv0;
                }

                // 计算原始UV框
                var uvMin = _Min(v1.uv0, v2.uv0, v3.uv0);
                var uvMax = _Max(v1.uv0, v2.uv0, v3.uv0);
                //OutlineColor 和 OutlineWidth 也传入，避免出现不同的材质球
                var col_rg = new Vector2(OutlineColor.r, OutlineColor.g); //描边颜色 用uv3 和 tangent的 zw传递
                var col_ba = new Vector4(0, 0, OutlineColor.b, OutlineColor.a);
                var normal = new Vector3(0, 0, OutlineWidth); //描边的宽度 用normal的z传递

                // 为每个顶点设置新的Position和UV，并传入原始UV框
                v1 = _SetNewPosAndUV(v1, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax, name: gameObject.name);
                v1.uv3 = col_rg;
                v1.tangent = col_ba;
                v1.normal = normal;

                v2 = _SetNewPosAndUV(v2, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax, name: gameObject.name);
                v2.uv3 = col_rg;
                v2.tangent = col_ba;
                v2.normal = normal;

                v3 = _SetNewPosAndUV(v3, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax,name:gameObject.name);
                v3.uv3 = col_rg;
                v3.tangent = col_ba;
                v3.normal = normal;

                m_VetexList[i] = v1;
                m_VetexList[i + 1] = v2;
                m_VetexList[i + 2] = v3;
            }
        }


        private static UIVertex _SetNewPosAndUV(UIVertex pVertex, float pOutLineWidth,Vector2 pPosCenter,Vector2 pTriangleX, Vector2 pTriangleY,
            Vector2 pUVX, Vector2 pUVY,Vector2 pUVOriginMin, Vector2 pUVOriginMax,bool needOffset = true,string name="")
        {
            // Position
            var pos = pVertex.position;
            var posXOffset = pos.x > pPosCenter.x ? pOutLineWidth : -pOutLineWidth;
            var posYOffset = pos.y > pPosCenter.y ? pOutLineWidth : -pOutLineWidth;
            pos.x += posXOffset;
            pos.y += posYOffset;
            pVertex.position = pos;
#if UNITY_2020_1_OR_NEWER
            var uv = pVertex.uv0;
            Vector2 vx = new Vector2(uv.x, uv.y);
            //Debuger.Log($"name:{name} pUVX:{pUVX}  pTriangleX.magnitude:{Mathf.Min(pTriangleX.magnitude, 18) }     pTriangleY.magnitude:{Mathf.Min(pTriangleY.magnitude, 18) } posXOffset:{posXOffset}  Dot:{(Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1)}");
            vx += pUVX / Mathf.Min(pTriangleX.magnitude, 18) * posXOffset * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
            vx += pUVY / Mathf.Min(pTriangleY.magnitude, 18) * posYOffset * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);
            uv = new Vector4(vx.x, vx.y, uv.z, uv.w);
            pVertex.uv0 = uv;
#else
            var uv = pVertex.uv0;
            uv += pUVX / pTriangleX.magnitude * posXOffset * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
            uv += pUVY / pTriangleY.magnitude * posYOffset * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);
            pVertex.uv0 = uv;
#endif
            pVertex.uv1 = pUVOriginMin; //uv1 uv2 可用  tangent  normal 在缩放情况 会有问题
            pVertex.uv2 = pUVOriginMax;

            return pVertex;
        }


        private static float _Min(float pA, float pB, float pC, float pD, float pE, float pF)
        {
            return Mathf.Min(Mathf.Min(Mathf.Min(Mathf.Min(Mathf.Min(pA, pB), pC), pD), pE), pF);
        }


        private static float _Max(float pA, float pB, float pC, float pD, float pE, float pF)
        {
            return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(pA, pB), pC), pD), pE), pF);
        }


        private static float _Min(float pA, float pB, float pC, float pD)
        {
            return Mathf.Min(Mathf.Min(Mathf.Min(pA, pB), pC), pD);
        }


        private static float _Max(float pA, float pB, float pC, float pD)
        {
            return Mathf.Max(Mathf.Max(Mathf.Max(pA, pB), pC), pD);
        }


        private static float _Min(float pA, float pB, float pC)
        {
            return Mathf.Min(Mathf.Min(pA, pB), pC);
        }


        private static float _Max(float pA, float pB, float pC)
        {
            return Mathf.Max(Mathf.Max(pA, pB), pC);
        }

        private static Vector2 _Min(Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD)
        {
            return new Vector2(_Min(pA.x, pB.x, pC.x, pD.x), _Min(pA.y, pB.y, pC.y, pD.y));
        }


        private static Vector2 _Max(Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD)
        {
            return new Vector2(_Max(pA.x, pB.x, pC.x, pD.x), _Max(pA.y, pB.y, pC.y, pD.y));
        }

        private static Vector2 _Min(Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD, Vector2 pE, Vector2 pF)
        {
            return new Vector2(_Min(pA.x, pB.x, pC.x, pD.x, pE.x, pF.x), _Min(pA.y, pB.y, pC.y, pD.y, pE.y, pF.y));
        }


        private static Vector2 _Max(Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD, Vector2 pE, Vector2 pF)
        {
            return new Vector2(_Max(pA.x, pB.x, pC.x, pD.x, pE.x, pF.x), _Max(pA.y, pB.y, pC.y, pD.y, pE.y, pF.y));
        }

        private static Vector2 _Min(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Min(pA.x, pB.x, pC.x), _Min(pA.y, pB.y, pC.y));
        }


        private static Vector2 _Max(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Max(pA.x, pB.x, pC.x), _Max(pA.y, pB.y, pC.y));
        }
    }
}
