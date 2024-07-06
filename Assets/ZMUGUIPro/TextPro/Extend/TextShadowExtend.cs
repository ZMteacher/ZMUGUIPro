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
    public class TextShadowExtend
    {
        [SerializeField]
        private bool m_UseShadow;
        [SerializeField]
        private Color m_ShadowColorTopLeft = Color.white;
        [SerializeField]
        private Color m_ShadowColorTopRight = Color.white;
        [SerializeField]
        private Color m_ShadowColorBottomLeft = Color.white;
        [SerializeField]
        private Color m_ShadowColorBottomRight = Color.white;
        [SerializeField]
        private Vector2 m_EffectDistance = new Vector2(1f, -1f);

        private const float kMaxEffectDistance = 600f;
        private Vector2 m_VertexColorOffset = Vector2.zero;

        public bool UseShadow
        {
            get
            {
                return m_UseShadow;
            }
            set
            {
                m_UseShadow = value;
            }
        }

        public Color VertexTopLeft
        {
            get
            {
                return m_ShadowColorTopLeft;
            }
            set
            {
                m_ShadowColorTopLeft = value;
            }
        }

        public Color VertexTopRight
        {
            get
            {
                return m_ShadowColorTopRight;
            }
            set
            {
                m_ShadowColorTopRight = value;
            }
        }

        public Color VertexBottomLeft
        {
            get
            {
                return m_ShadowColorBottomLeft;
            }
            set
            {
                m_ShadowColorBottomLeft = value;
            }
        }

        public Color VertexBottomRight
        {
            get
            {
                return m_ShadowColorBottomRight;
            }
            set
            {
                m_ShadowColorBottomRight = value;
            }
        }

        public Vector2 effectDistance
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

        protected void ApplyShadow(List<UIVertex> verts, Vector2 min, Vector2 max, Color32 color, float x, float y)
        {
            UIVertex vt;
            int start = 0, end = verts.Count;
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
                vt.color = RemapColor(min, max, color, v);
                verts[i] = vt;
            }
        }

        private Color RemapColor(Vector2 min, Vector2 max, Color color, Vector2 pos)
        {
            float x01 = max.x == min.x ? 0f : Mathf.Clamp01((pos.x - min.x) / (max.x - min.x));
            float y01 = max.y == min.y ? 0f : Mathf.Clamp01((pos.y - min.y) / (max.y - min.y));
            x01 -= m_VertexColorOffset.x * (m_VertexColorOffset.x > 0f ? x01 : (1f - x01));
            y01 -= m_VertexColorOffset.y * (m_VertexColorOffset.y > 0f ? y01 : (1f - y01));
            Color newColor = Color.Lerp(
                Color.Lerp(VertexBottomLeft, VertexBottomRight, x01),
                Color.Lerp(VertexTopLeft, VertexTopRight, x01),
                y01
            );
            //使用全新颜色 不继承原有的
            return newColor;
            //return color * newColor;
        }

        public void PopulateMesh(VertexHelper vh, RectTransform rectTransform, Color color)
        {
            if (UseShadow)
            {
                Vector2 min = rectTransform.pivot;
                min.Scale(-rectTransform.rect.size);
                Vector2 max = rectTransform.rect.size + min;
                List<UIVertex> output = new List<UIVertex>();
                vh.GetUIVertexStream(output);
                ApplyShadow(output, min, max, color, effectDistance.x, effectDistance.y);
                vh.Clear();
                vh.AddUIVertexTriangleStream(output);
            }
        }
    }
}
