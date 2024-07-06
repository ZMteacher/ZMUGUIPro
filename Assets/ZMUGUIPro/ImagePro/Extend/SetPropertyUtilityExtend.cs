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

 namespace ZM.UGUIPro {	
	internal static class SetPropertyUtilityExtend
	{
	    public static bool SetColor(ref Color currentValue, Color newValue)
	    {
	        if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
	            return false;
	
	        currentValue = newValue;
	        return true;
	    }
	
	    public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
	    {
	        if (currentValue.Equals(newValue))
	            return false;
	
	        currentValue = newValue;
	        return true;
	    }
	
	    public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
	    {
	        if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
	            return false;
	
	        currentValue = newValue;
	        return true;
	    }
	}
}
