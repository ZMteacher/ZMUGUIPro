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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using System;

 namespace ZM.UGUIPro {	
	 [Serializable]
	public class ImageMaskExtend
	{
	    public Image m_Image;
	
	    private RectTransform rectTransform;
	    [SerializeField]
	    public bool m_IsUseMaskImage = false;
	
	    //private Sprite z_Sprite;
	    //public Sprite sprite { get { return z_Sprite; } set { if (SetPropertyUtilityExtend.SetClass(ref z_Sprite, value)) m_Image.SetAllDirty(); } }
	    public Sprite overrideSprite { get { return m_Image.overrideSprite; } set { m_Image.overrideSprite = value;   m_Image. SetAllDirty(); } }
	
	    public void Initializa(Image image)
	    {
	        m_Image = image;
	        rectTransform = m_Image.rectTransform;
	        innerVertices = new List<Vector3>();
	        outterVertices = new List<Vector3>();
	    }
#if UNITY_EDITOR
	    public void EditorInitializa(Image image)
	    {
	        m_Image = image;
	        rectTransform = m_Image.rectTransform;
	    }
#endif
	    // Update is called once per frame
	    public void Update()
	    {
	        this.m_TrisCont = (float)Mathf.Clamp(this.m_TrisCont, 0, m_Image.rectTransform.rect.width / 2);
	    }
	
	    [SerializeField]
	    [Tooltip("圆形或扇形填充比例")]
	    [Range(0, 1)]
	    public float m_FillPercent = 1f;
	    [SerializeField]
	    [Tooltip("是否填充圆形")]
	    public bool m_Fill = true;
	    [Tooltip("圆环宽度")]
	    public float m_TrisCont = 5;
	    [SerializeField]
	    [Tooltip("圆形")]
	    [Range(3, 100)]
	    public int m_Segements = 20;
	
	    public List<Vector3> innerVertices;
	    public List<Vector3> outterVertices;
	
	    public void OnPopulateMesh(VertexHelper vh)
	    {
	        vh.Clear();
	
	        innerVertices.Clear();
	        outterVertices.Clear();
	
	        float degreeDelta = (float)(2 * Mathf.PI / m_Segements);
	        int curSegements = (int)(m_Segements * m_FillPercent);
	
	        float tw = rectTransform.rect.width;
	        float th = rectTransform.rect.height;
	        float outerRadius = rectTransform.pivot.x * tw;
	        float innerRadius = rectTransform.pivot.x * tw - m_TrisCont;
	
	        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
	
	        float uvCenterX = (uv.x + uv.z) * 0.5f;
	        float uvCenterY = (uv.y + uv.w) * 0.5f;
	        float uvScaleX = (uv.z - uv.x) / tw;
	        float uvScaleY = (uv.w - uv.y) / th;
	
	        float curDegree = 0;
	        UIVertex uiVertex;
	        int verticeCount;
	        int triangleCount;
	        Vector2 curVertice;
	        //Debuger.Log("m_Fill:"+ m_Fill);
	        if (m_Fill) //圆形
	        {
	            curVertice = Vector2.zero;
	            verticeCount = curSegements + 1;
	            uiVertex = new UIVertex();
	            uiVertex.color = m_Image.color;
	            uiVertex.position = curVertice;
	            uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
	            vh.AddVert(uiVertex);
	
	            for (int i = 1; i < verticeCount; i++)
	            {
	                float cosA = Mathf.Cos(curDegree);
	                float sinA = Mathf.Sin(curDegree);
	                curVertice = new Vector2(cosA * outerRadius, sinA * outerRadius);
	                curDegree += degreeDelta;
	
	                uiVertex = new UIVertex();
	                uiVertex.color = m_Image.color;
	                uiVertex.position = curVertice;
	                uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
	                vh.AddVert(uiVertex);
	
	                outterVertices.Add(curVertice);
	            }
	
	            triangleCount = curSegements * 3;
	            for (int i = 0, vIdx = 1; i < triangleCount - 3; i += 3, vIdx++)
	            {
	                vh.AddTriangle(vIdx, 0, vIdx + 1);
	            }
	            if (m_FillPercent == 1)
	            {
	                //首尾顶点相连
	                vh.AddTriangle(verticeCount - 1, 0, 1);
	            }
	        }
	        else//圆环
	        {
	            verticeCount = curSegements * 2;
	            for (int i = 0; i < verticeCount; i += 2)
	            {
	                float cosA = Mathf.Cos(curDegree);
	                float sinA = Mathf.Sin(curDegree);
	                curDegree += degreeDelta;
	
	                curVertice = new Vector3(cosA * innerRadius, sinA * innerRadius);
	                uiVertex = new UIVertex();
	                uiVertex.color = m_Image.color;
	                uiVertex.position = curVertice;
	                uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
	                vh.AddVert(uiVertex);
	                innerVertices.Add(curVertice);
	
	                curVertice = new Vector3(cosA * outerRadius, sinA * outerRadius);
	                uiVertex = new UIVertex();
	                uiVertex.color = m_Image.color;
	                uiVertex.position = curVertice;
	                uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
	                vh.AddVert(uiVertex);
	                outterVertices.Add(curVertice);
	            }
	
	            triangleCount = curSegements * 3 * 2;
	            for (int i = 0, vIdx = 0; i < triangleCount - 6; i += 6, vIdx += 2)
	            {
	                vh.AddTriangle(vIdx + 1, vIdx, vIdx + 3);
	                vh.AddTriangle(vIdx, vIdx + 2, vIdx + 3);
	            }
	            if (m_FillPercent == 1)
	            {
	                //首尾顶点相连
	                vh.AddTriangle(verticeCount - 1, verticeCount - 2, 1);
	                vh.AddTriangle(verticeCount - 2, 0, 1);
	            }
	        }
	
	    }
	
	 
	    public bool Contains(Vector2 p, List<Vector3> outterVertices, List<Vector3> innerVertices)
	    {
	        var crossNumber = 0;
	        RayCrossing(p, innerVertices, ref crossNumber);//检测内环
	        RayCrossing(p, outterVertices, ref crossNumber);//检测外环
	        return (crossNumber & 1) == 1;
	    }
	
	    /// <summary>
	    /// 使用RayCrossing算法判断点击点是否在封闭多边形里
	    /// </summary>
	    /// <param name="p"></param>
	    /// <param name="vertices"></param>
	    /// <param name="crossNumber"></param>
	    private void RayCrossing(Vector2 p, List<Vector3> vertices, ref int crossNumber)
	    {
	        for (int i = 0, count = vertices.Count; i < count; i++)
	        {
	            var v1 = vertices[i];
	            var v2 = vertices[(i + 1) % count];
	
	            //点击点水平线必须与两顶点线段相交
	            if (((v1.y <= p.y) && (v2.y > p.y))
	                || ((v1.y > p.y) && (v2.y <= p.y)))
	            {
	                //只考虑点击点右侧方向，点击点水平线与线段相交，且交点x > 点击点x，则crossNumber+1
	                if (p.x < v1.x + (p.y - v1.y) / (v2.y - v1.y) * (v2.x - v1.x))
	                {
	                    crossNumber += 1;
	                }
	            }
	        }
	    }
	
	
	}
}
