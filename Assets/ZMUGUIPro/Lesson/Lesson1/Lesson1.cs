using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.UGUIPro;

public class Lesson1 : MonoBehaviour
{

    async void Start()
    {
        //1.多语言Excel表配置生成方式
        //生成流程:菜单栏-ZMFrame-生成多语言配置

        //2.多语言Excel读取、解析、输出路径配置 ExcelToConfig 

        //3.多语言功能使用演示
        //初始化多语言系统，自动加载本地对应语言配置文件
        await LocalizationManager.Instance.InitLanguageConfig();

        //4.切换语言
        //await LocalizationManager.Instance.SwitchLanguage(LanguageType.English);

        //5.多语言图片加载方式  ImageProBase LocalizationImageExtend

        //6.新增多语言配置

        //7.
        //Text 多语言 TextMeshPro
        //Text：动态字体 会动态去查找设备中支持的字体。中文 
        //TextmeshPro 静态字体 中文 去制作字体 增大包体 。
        //总结：3种以上，老师建议使用TextPro  
        //     2种，使用哪一套都行，除非有严格的包体限制，如果有限制推荐使用TextPro

    }
    private async void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            await LocalizationManager.Instance.SwitchLanguage(LanguageType.English);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            await LocalizationManager.Instance.SwitchLanguage(LanguageType.Chinese);
        }
    }
}
