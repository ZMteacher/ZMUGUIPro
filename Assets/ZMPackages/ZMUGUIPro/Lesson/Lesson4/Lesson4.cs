using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.UGUIPro;

public class Lesson4 : MonoBehaviour
{
    public ButtonPro button;
    void Start()
    {
        //1.ButtonPro使用教程

        //2.动态添加事件
        //普通事件添加
        //button.AddButtonClick(OnButtonClick);
        //button.onClick.AddListener(OnButtonClick);

        //长按事件添加
        button.AddButtonLongListener(OnLongPressButtonClick,1);

        //双击事件添加
        //button.AddButtonDoubleClickListener(OnDoubleButtonClick, 0.3f);

        //3.按钮事件触发原理和音效配置 ButtonProBase 

        //4.添加一个拓展功能
        //步骤 第一步：创建Editor去绘制对应的字段。 第二步：创建功能拓展类，去实现对应功能的逻辑。
        //前提：功能拓展类必须要支持序列化
    }

    public void OnButtonClick()
    {
        Debug.Log("普通点击事件触发");
    }
    public void OnLongPressButtonClick()
    {
        Debug.Log("长安按钮事件触发");
    }
    public void OnDoubleButtonClick()
    {
        Debug.Log("双击按钮事件触发");
    }
}
