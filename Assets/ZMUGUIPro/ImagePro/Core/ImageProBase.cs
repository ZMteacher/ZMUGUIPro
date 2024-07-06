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
using UnityEngine.UI;

namespace ZM.UGUIPro
{
    [System.Serializable]
    public class ImageProBase: Image
    {
		[SerializeField]
		private LocalizationImageExtend m_LocalizationImage = new LocalizationImageExtend();
		public LocalizationImageExtend LocalizationImageExtend { get { return m_LocalizationImage; } }
		[SerializeField]
		private ImageMaskExtend m_ImageMaskExtend = new ImageMaskExtend();
		public new Sprite sprite { get { return base.sprite; } set { base.sprite = value; SpriteReassign(); } }
		protected override void Awake()
		{
			base.Awake();
			m_LocalizationImage.Initializa(this);
			m_ImageMaskExtend.Initializa(this);
		}
		protected override void Start()
		{
			base.Start();
			m_LocalizationImage.UpdateImage();
		}
		private void Update()
		{
			m_ImageMaskExtend.Update();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
		public void ModifyFillPercent(float value)
		{
			m_ImageMaskExtend.m_FillPercent = value;
		}
#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
			m_ImageMaskExtend.EditorInitializa(this);
		}
#endif
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (!m_ImageMaskExtend.m_IsUseMaskImage)
			{
				base.OnPopulateMesh(vh);
			}
			else
			{
				m_ImageMaskExtend.OnPopulateMesh(vh);
			}
		}

		public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (!m_ImageMaskExtend.m_IsUseMaskImage)
			{
				return base.IsRaycastLocationValid(screenPoint, eventCamera);
			}
			else
			{
				Vector2 local;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local);
				return m_ImageMaskExtend.Contains(local, m_ImageMaskExtend.outterVertices, m_ImageMaskExtend.innerVertices);
			}
		}
		public virtual void SpriteReassign() { }
	}
}
