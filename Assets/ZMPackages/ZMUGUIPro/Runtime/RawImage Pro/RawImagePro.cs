using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using ZM.UGUIPro;


public class RawImagePro : RawImage
{
    [SerializeField]
    private RawImageProMaskExtend m_RawImageProMaskExtend = new RawImageProMaskExtend();

    protected override void Awake()
    {
        base.Awake();
        m_RawImageProMaskExtend.Initializa(this);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (m_RawImageProMaskExtend.IsUseMask)
        {
            m_RawImageProMaskExtend.OnPopulateMesh(vh);
            return;
        } 
        base.OnPopulateMesh(vh);
    }
    
}