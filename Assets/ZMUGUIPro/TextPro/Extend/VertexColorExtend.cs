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
    public class VertexColorExtend
    {
        public enum ColorFilterType
        {
            Additive,  //基础色加上新颜色
            OverLap, //颜色叠加
        }

        [SerializeField]
        private bool m_UseVertexColor = false;
        [SerializeField]
        private ColorFilterType m_VertexColorFilter = ColorFilterType.Additive;
        [SerializeField]
        private Color m_VertexTopLeft = Color.white;
        [SerializeField]
        private Color m_VertexTopRight = Color.white;
        [SerializeField]
        private Color m_VertexBottomLeft = Color.white;
        [SerializeField]
        private Color m_VertexBottomRight = Color.white;
        [SerializeField]
        private Vector2 m_VertexColorOffset = Vector2.zero;

        public bool UseVertexColor
        {
            get
            {
                return m_UseVertexColor;
            }

            set
            {
                m_UseVertexColor = value;
            }
        }


        public ColorFilterType VertexColorFilter
        {
            get
            {
                return m_VertexColorFilter;
            }

            set
            {
                m_VertexColorFilter = value;
            }
        }

        public Color VertexTopLeft
        {
            get
            {
                return m_VertexTopLeft;
            }

            set
            {
                m_VertexTopLeft = value;
            }
        }

        public Color VertexTopRight
        {
            get
            {
                return m_VertexTopRight;
            }

            set
            {
                m_VertexTopRight = value;
            }
        }

        public Color VertexBottomLeft
        {
            get
            {
                return m_VertexBottomLeft;
            }

            set
            {
                m_VertexBottomLeft = value;
            }
        }

        public Color VertexBottomRight
        {
            get
            {
                return m_VertexBottomRight;
            }

            set
            {
                m_VertexBottomRight = value;
            }
        }

        public Vector2 VertexColorOffset
        {
            get
            {
                return m_VertexColorOffset;
            }

            set
            {
                m_VertexColorOffset = value;
            }
        }

        public void PopulateMesh(VertexHelper toFill, RectTransform rectTransform, Color color)
        {
            if (UseVertexColor)
            {
                Vector2 min = rectTransform.pivot;
                min.Scale(-rectTransform.rect.size);
                Vector2 max = rectTransform.rect.size + min;
                int len = toFill.currentVertCount;
                for (int i = 0; i < len; i++)
                {
                    UIVertex v = new UIVertex();
                    toFill.PopulateUIVertex(ref v, i);
                    v.color = RemapColor(min, max, color, v.position);
                    toFill.SetUIVertex(v, i);
                }
            }
        }


        private Color RemapColor(Vector2 min, Vector2 max, Color color, Vector2 pos)
        {
            float x01 = max.x == min.x ? 0f : Mathf.Clamp01((pos.x - min.x) / (max.x - min.x));
            float y01 = max.y == min.y ? 0f : Mathf.Clamp01((pos.y - min.y) / (max.y - min.y));
            x01 -= VertexColorOffset.x * (VertexColorOffset.x > 0f ? x01 : (1f - x01));
            y01 -= VertexColorOffset.y * (VertexColorOffset.y > 0f ? y01 : (1f - y01));
            Color newColor = Color.Lerp(
                Color.Lerp(VertexBottomLeft, VertexBottomRight, x01),
                Color.Lerp(VertexTopLeft, VertexTopRight, x01),
                y01
            );
            switch (VertexColorFilter)
            {
                default:
                case ColorFilterType.Additive:
                    return color + newColor;
                case ColorFilterType.OverLap:
                    float a = Mathf.Max(newColor.a, color.a);
                    newColor = Color.Lerp(color, newColor, newColor.a);
                    newColor.a = a;
                    return newColor;
            }
        }

    }
}
