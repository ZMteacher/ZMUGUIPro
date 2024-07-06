using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.UGUIPro;

public class ZMUGUIPro : MonoBehaviour
{

    async void Start()
    {
        //执行可等待异步本地多语言配置加载
        await LocalizationManager.Instance.InitLanguageConfig();
    }

    public async void OnChineseButtonClick()
    {
       await LocalizationManager.Instance.SwitchLanguage(LanguageType.Chinese);
    }

    public async void OnEnglishButtonClick()
    {
        await LocalizationManager.Instance.SwitchLanguage(LanguageType.English);
    }
}
