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
using UnityEngine;
using UnityEngine.UI;

namespace ZM.UGUIPro
{
    [System.Serializable]
    public class TextOutlineExtend
    {
        public bool UseOutline
        {
            get { return m_UseOutline; }
            set { m_UseOutline = value; }
        }
        [SerializeField]
        private bool m_UseOutline;

        [SerializeField]
        private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

        [SerializeField]
        private Vector2 m_EffectDistance = new Vector2(1f, -1f);

        private const float kMaxEffectDistance = 600f;
        public Color EffectColor
        {
            get { return m_EffectColor; }
            set
            {
                m_EffectColor = value;
            }
        }

        public Vector2 EffectDistance
        {
            get { return m_EffectDistance; }
            set
            {
                if (value.x > kMaxEffectDistance)
                    value.x = kMaxEffectDistance;
                if (value.x < -kMaxEffectDistance)
                    value.x = -kMaxEffectDistance;

                if (value.y > kMaxEffectDistance)
                    value.y = kMaxEffectDistance;
                if (value.y < -kMaxEffectDistance)
                    value.y = -kMaxEffectDistance;

                if (m_EffectDistance == value)
                    return;

                m_EffectDistance = value;

            }
        }

        protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            UIVertex vt;
            var neededCapacity = verts.Count + end - start;
            if (verts.Capacity < neededCapacity)
                verts.Capacity = neededCapacity;

            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                verts.Add(vt);

                Vector3 v = vt.position;
                v.x += x;
                v.y += y;
                vt.position = v;
                var newColor = color;
                newColor.a = (byte)((newColor.a * verts[i].color.a) / 255);
                vt.color = newColor;
                verts[i] = vt;
            }
        }

        public void PopulateMesh(VertexHelper vh)
        {
            if (UseOutline)
            {
                List<UIVertex> verts = new List<UIVertex>();
                vh.GetUIVertexStream(verts);

                var neededCpacity = verts.Count * 5;
                if (verts.Capacity < neededCpacity)
                    verts.Capacity = neededCpacity;

                var start = 0;
                var end = verts.Count;
                ApplyShadowZeroAlloc(verts, EffectColor, start, verts.Count, EffectDistance.x, EffectDistance.y);

                start = end;
                end = verts.Count;
                ApplyShadowZeroAlloc(verts, EffectColor, start, verts.Count, EffectDistance.x, -EffectDistance.y);

                start = end;
                end = verts.Count;
                ApplyShadowZeroAlloc(verts, EffectColor, start, verts.Count, -EffectDistance.x, EffectDistance.y);

                start = end;
                end = verts.Count;
                ApplyShadowZeroAlloc(verts, EffectColor, start, verts.Count, -EffectDistance.x, -EffectDistance.y);

                vh.Clear();
                vh.AddUIVertexTriangleStream(verts);
            }
        }
    }
}
