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
    public class TextSpacingExtend
    {
        [SerializeField]
        private bool m_UseTextSpacing;
        public bool UseTextSpacing
        {
            get
            {
                return m_UseTextSpacing;
            }
            set
            {
                m_UseTextSpacing = value;
            }
        }

        [SerializeField]
        [Range(-10, 100)]
        private float m_TextSpacing = 1f;
        public float _textSpacing
        {
            get
            {
                return m_TextSpacing;
            }
            set
            {
                m_TextSpacing = value;
            }
        }


        public void PopulateMesh(VertexHelper toFill)
        {
            if (UseTextSpacing)
            {
                if (toFill.currentVertCount == 0)
                {
                    return;
                }
                List<UIVertex> vertexs = new List<UIVertex>();
                toFill.GetUIVertexStream(vertexs);
                int indexCount = toFill.currentIndexCount;
                UIVertex vt;
                for (int i = 6; i < indexCount; i++)
                {
                    //第一个字不用改变位置
                    vt = vertexs[i];
                    vt.position += new Vector3(m_TextSpacing * (i / 6), 0, 0);
                    vertexs[i] = vt;
                    //以下注意点与索引的对应关系
                    if (i % 6 <= 2)
                    {
                        toFill.SetUIVertex(vt, (i / 6) * 4 + i % 6);
                    }
                    if (i % 6 == 4)
                    {
                        toFill.SetUIVertex(vt, (i / 6) * 4 + i % 6 - 1);
                    }
                }
            }
        }
    }
}
