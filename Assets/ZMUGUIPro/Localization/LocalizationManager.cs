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
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace ZM.UGUIPro
{
    public enum LanguageType //语言类型
    {
        None = 0,
        English = 1,            //英语
        Chinese=2,
        Thai,//泰语
    }

    public enum LanguageFontType //语言字体类型
    {
        English=1,
        Chinese=2,
        Thai=3,//泰语
    }
    public class LocalizationManager
    {
        private static LocalizationManager _instance;
        public static LocalizationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LocalizationManager();
                }
                return _instance;
            }
        }
        /// <summary>
        /// 本地多语言数据配置文件
        /// </summary>
        private LocalizationDataConfig m_LocalizationDataconfig = new LocalizationDataConfig();

        private LanguageType m_LanguageType;
        /// <summary>
        /// 当前语言类型
        /// </summary>
        public LanguageType LanguageType { get { return m_LanguageType; } set { m_LanguageType = value; } }
        /// <summary>
        /// 已加载的本地多语言数据列表
        /// </summary>
        private Dictionary<LanguageType, List<LocalizationData>> m_LocalizationDataListDic = new Dictionary<LanguageType, List<LocalizationData>>();
        /// <summary>
        /// 已加载的字体字典
        /// </summary>
        private Dictionary<LanguageType, Font> fontDic = new Dictionary<LanguageType, Font>();
        /// <summary>
        /// 本地多语言数据列表
        /// </summary>
        private List<LocalizationData> m_LocalizationDataList { get { return m_LocalizationDataListDic.ContainsKey(m_LanguageType) ? m_LocalizationDataListDic[m_LanguageType] : null; } }
        /// <summary>
        /// 语言改变文本监听列表
        /// </summary>
        private List<System.Action> m_LocalizationTextList = new List<System.Action>();
        /// <summary>
        /// 语言改变字体监听列表
        /// </summary>
        private List<System.Action> m_LocalizationFontList = new List<System.Action>();
        /// <summary>
        /// 本地多语言标记
        /// </summary>
        private string mLastUseLanguageType = "lastlanguageType";
       

        /// <summary>
        /// 初始化多语言类型
        /// </summary>
        public async Task<int> InitLanguageConfig()
        {
            InitLanguageType();
            return await LoadLanguageConfig();
        }
        /// <summary>
        /// 初始化语言配置
        /// </summary>
        private  void InitLanguageType()
        {
            int int_language = PlayerPrefs.GetInt("lastlanguageType");
            LanguageType= int_language == 0 ? LanguageType = LanguageType.English: LanguageType = (LanguageType)int_language;
            //SystemLanguage systemLanguage = Application.systemLanguage; //系统语言  
            PlayerPrefs.SetInt(mLastUseLanguageType, (int)LanguageType);
        }
        /// <summary>
        /// 加载语言配置文件
        /// </summary>
        /// <returns></returns>
        private async Task<int> LoadLanguageConfig()
        {

            if (m_LocalizationDataList != null)
                return 1;
 
            List<LocalizationData> dataList = await m_LocalizationDataconfig.LoadConfig(m_LanguageType);
            if (dataList != null)
            {
                m_LocalizationDataListDic.Add(m_LanguageType, dataList);
            }
            return 0;
        }
        /// <summary>
        /// Editor模式预加载配置
        /// </summary>
        public void PreLoadConfigEidotr()
        {

            if (m_LocalizationDataList != null)
                return;

            InitLanguageType();

            List<LocalizationData> dataList = m_LocalizationDataconfig.LoadConfigEditor(m_LanguageType);
            if (dataList != null)
            {
                m_LocalizationDataListDic.Add(m_LanguageType, dataList);
            }
        }
        #region 多语言获取
        /// <summary>
        /// 获取多语言数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public LocalizationData GetLocalizationData(string key)
        {

            LocalizationData tempdata = new LocalizationData { Key = "noConfig", value = "noConfig" };
            if (string.IsNullOrEmpty(key)) return tempdata;

            if (m_LocalizationDataList == null)
            {
#if UNITY_EDITOR
                PreLoadConfigEidotr();
#else
                PreLoadConfig();
#endif
            }
            List<LocalizationData> dataList = m_LocalizationDataList;
            if (dataList==null)
            {
                Debug.LogError(" dataList is Null ...");
                return tempdata;
            }
            for (int i = 0; i < dataList.Count; i++)
            {
                LocalizationData data = dataList[i];
                if (string.Equals(data.Key, key))
                {
                    return data;
                }
            }
            Debug.LogError(" key:" + key + " Can't find data!");
            return tempdata;
        }
        public string GetLocalizationDataValue(string key)
        {
            return GetLocalizationData(key).value;
        }
        public string GetLocalizationText(string key, bool needCorrect = true)
        {
            if (m_LocalizationDataList == null)
            {
#if UNITY_EDITOR
                PreLoadConfigEidotr();
#else
                PreLoadConfig();
#endif
            }
            if (m_LocalizationDataList == null)
            {
                return "";
            }
            for (int i = 0; i < m_LocalizationDataList.Count; i++)
            {
                LocalizationData data = m_LocalizationDataList[i];
                if (string.Equals(data.Key, key))
                {
                    return data.value;
                }
            }
            Debug.LogError(" key:" + key + " Can't find!");
            return "";
        }
        public int GetLocalizationImageIndex()
        {
            return (int)m_LanguageType;

        }
        public string GetLanguageTypeName()
        {
            return m_LanguageType.ToString();
        }
        #endregion
        public async Task<string> SwitchLanguage(LanguageType language)
        {
            m_LanguageType = language;
            PlayerPrefs.SetInt(mLastUseLanguageType, (int)language);
            //等待对应语言配置加载完成
            await LoadLanguageConfig();

            for (int i = 0; i < m_LocalizationTextList.Count; i++)
            {
                m_LocalizationTextList[i]?.Invoke();
            }
            for (int i = 0; i < m_LocalizationFontList.Count; i++)
            {
                m_LocalizationFontList[i]?.Invoke();
            }
            return "";
        }

        /// <summary>
        /// 修改字体
        /// </summary>
        /// <param name="text"></param>
        /// <param name="setType"></param>
        public void ChangeFont(Text text, LanguageType setType = LanguageType.None)
        {
            if (text != null)
            {
                var currentType = setType == LanguageType.None ? LanguageType : setType;

                string fontName = "defaultFont";
                switch (currentType)
                {
                    case LanguageType.English:        
                        fontName = "Siddhanta Unity";
                        break;
                }
                Font font = null;
                if (!fontDic.ContainsKey(LanguageType))
                {
                    font = Resources.Load<Font>("Font/" + fontName);
                    fontDic.Add(LanguageType, font);
                }
                else
                {
                    font = fontDic[LanguageType];
                }

                if (font != null)
                    text.font = font;
            }
        }

        #region 事件监听
        public void AddLanguageChangeListener(System.Action localizationTextCall)
        {
            if (!m_LocalizationTextList.Contains(localizationTextCall))
                m_LocalizationTextList.Add(localizationTextCall);
        }

        public void RemoveLanguageChangeListener(System.Action localizationTextCall)
        {
            if (m_LocalizationTextList.Contains(localizationTextCall))
                m_LocalizationTextList.Remove(localizationTextCall);
        }
        public void AddFontChangeListener(System.Action localizationTextCall)
        {
            if (!m_LocalizationFontList.Contains(localizationTextCall))
                m_LocalizationFontList.Add(localizationTextCall);
        }

        public void RemoveFontChangeListener(System.Action localizationTextCall)
        {
            if (m_LocalizationFontList.Contains(localizationTextCall))
                m_LocalizationFontList.Remove(localizationTextCall);
        }
        #endregion
    }
}
