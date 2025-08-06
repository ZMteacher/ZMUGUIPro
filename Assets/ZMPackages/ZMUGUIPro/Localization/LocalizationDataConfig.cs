/*----------------------------------------------------------------
* Title: ZM.UGUIPro
*
* Description: TextPro ImagePro ButtonPro TextMesh Pro
* 
* Support Function: 高性能描边、本地多语言文本、图片、按钮双击模式、长按模式、文本顶点颜色渐变、双色渐变、三色渐变
* 
* Usage: 右键-TextPro-ImagePro-ButtonPro-TextMeshPro
* 
* Author: 铸梦 https://www.yxtown.com/user/38633b977fadc0db8e56483c8ee365a2cafbe96b
*
* Date: 2023.4.13
*
* Modify: 
--------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZM.UGUIPro
{
    [System.Serializable]
    public class LocalizationData
    {
        //[JsonConverter(typeof(StringEnumConverter))]
        //public LanguageType languageType;
        public string Key;
        public string value;
    }
    public class LocalizationDataConfig
    {
        /// <summary>
        /// 多语言配置文件路径
        /// </summary>
        public const string CONFIG_PATH = "ExcelData/";
        /// <summary>
        /// 是否异步加载中
        /// </summary>
        private bool IsConfigLoading = false;
        
        
        /// <summary>
        /// 加载对应语言配置
        /// </summary>
        /// <param name="languageType"></param>
        /// <returns></returns>
        public async Task<List<LocalizationData>> LoadConfig(LanguageType languageType)
        {
            if (IsConfigLoading)
            {
                return null;
            }
            
            IsConfigLoading = true;

            //获取语言字符名称
            string languageName = languageType.ToString();
            //计算多语言配置文件加载路径
            string configPath = $"{CONFIG_PATH}{languageName}/{languageName}";
            //默认Resources加载，需要你根据自己的项目资源加载方式去修改
            TextAsset textAsset = Resources.Load<TextAsset>(configPath);
            //验证配置文件是否加载失败
            if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
            {
                IsConfigLoading = false;
                return null;
            }
            //开始异步加载配置文件
            List<LocalizationData> localizationDatalist = null;
            string jsonString = textAsset.text;
            //反序列化放到子线程中进行，放置配置表过大，导致主线程卡顿
            await Task.Run(() => { localizationDatalist = JsonConvert.DeserializeObject<List<LocalizationData>>(jsonString); });
            
            IsConfigLoading = false;
            return localizationDatalist;
        }

        /// <summary>
        /// 加载对应语言配置，通过Editor模式
        /// </summary>
        /// <param name="languageType"></param>
        /// <returns></returns>
        public List<LocalizationData> LoadConfigFormEditor(LanguageType languageType)
        {
            if (IsConfigLoading)
            {
                return null;
            }
            IsConfigLoading = true;
            //获取语言字符名称
            string languageName = languageType.ToString();
            //计算多语言配置文件加载路径
            string configPath = $"{CONFIG_PATH}{languageName}/{languageName}";
            //默认Resources加载，需要你根据自己的项目资源加载方式去修改
            TextAsset textAsset = Resources.Load<TextAsset>(configPath);
            
            //验证配置文件是否加载失败
            if (textAsset == null || string.IsNullOrEmpty(textAsset.text))
            {
                IsConfigLoading = false;
                return null;
            }
            
            string json = textAsset.text;
            List<LocalizationData> localizationDatalist = JsonConvert.DeserializeObject<List<LocalizationData>>(json);
            IsConfigLoading = false;
            return localizationDatalist;
        }
    }
}
