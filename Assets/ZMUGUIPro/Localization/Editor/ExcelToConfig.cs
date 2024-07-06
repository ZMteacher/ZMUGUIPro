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
using Excel;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using ZM.UGUIPro;

public class ExcelToConfig
{
    /// <summary>
    /// 解析表名 （在实际开发中可能某列会存在不解析的情况）
    /// </summary>
    private static List<string> mParseKeyList = new List<string>() { "Chinese", "English","Thai" };
    /// <summary>
    /// Excel文件读取路径
    /// </summary>
    public static string ExcelLaodPath= "Assets/ZMUGUIPro/Localization/ExcelData/";
    /// <summary>
    /// 文件输出路径
    /// </summary>
    public static string OutPutPath = "ZMUGUIPro/Localization/ExcelData/";
    [MenuItem("ZMFrame/生成Excel多语言配置")]
    static void GeneratorLoaclization()
    {
        DataSet resultSet = LaodExcel(ExcelLaodPath+"翻译文档.xlsx");
        GenerateLocalizationCfg(resultSet);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="excelFile">Excel file.</param> 
    public static DataSet LaodExcel(string excelFile)
    {
        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        return mExcelReader.AsDataSet();
    }
    /// <summary>
    /// 生成多语言配置
    /// </summary>
    public static void GenerateLocalizationCfg(DataSet resultSet)
    {

        //判断Excel文件中是否存在数据表
        if (resultSet.Tables.Count < 1)
            return;
        //默认读取第一个数据表
        DataTable mSheet = resultSet.Tables[0];
        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表以保存全部数据
        Dictionary<string, List<LocalizationData>> localizationDataListDic = new Dictionary<string, List<LocalizationData>>();
        for (int i = 1; i < colCount; i++)
        {
            List<LocalizationData> dataList = new List<LocalizationData>();

            string cloName = mSheet.Rows[0][i].ToString();
            int startIndex = cloName.LastIndexOf(@"（");
            if (startIndex > 0)
                cloName = cloName.Substring(0, startIndex - 1);

            if (cloName != "" && mParseKeyList.Contains(cloName))
            {
                localizationDataListDic.Add(cloName, dataList);
            }
        }

        //读取数据
        for (int i = 1; i < rowCount; i++)
        {
            for (int j = 1; j < colCount; j++)
            {
                //创建实例
                LocalizationData data = new LocalizationData();
              
                data.Key = mSheet.Rows[i][colCount - 1].ToString();
                data.value = mSheet.Rows[i][j].ToString();
                Debug.Log("key:" + data.Key);
                if (!string.IsNullOrEmpty(data.value) && mSheet.Rows[0][j].ToString() != "" && !string.IsNullOrEmpty(data.Key))
                {
                    string cloName = mSheet.Rows[0][j].ToString();
                    int startIndex = cloName.LastIndexOf(@"（");
                    if (startIndex > 0)
                        cloName = cloName.Substring(0, startIndex - 1);
                    if (localizationDataListDic.ContainsKey(cloName))
                        localizationDataListDic[cloName].Add(data);  //添加至列表

                }
            }
        }
        foreach (var item in localizationDataListDic)
        {
            //生成Json字符串
            string json = JsonConvert.SerializeObject(item.Value, Newtonsoft.Json.Formatting.Indented);
            int startIndex = item.Key.LastIndexOf(@"（");
            string itemKey = item.Key;
            if (startIndex > 0)
            {
                itemKey = item.Key.Substring(0, startIndex - 1);
            }
            //创建单独语言文件路径
            var savePath = OutPutPath + itemKey + "/";
            if (!Directory.Exists(Application.dataPath + "/" + savePath))
                Directory.CreateDirectory(Application.dataPath + "/" + savePath);
            json = json.Replace(@"\\n", @"\n");
            //写入文件
            using (FileStream fileStream = new FileStream(Application.dataPath + "/" + savePath + itemKey + "" + ".txt", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter textWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    textWriter.Write(json);
                }
            }
            Debug.Log("写入完成 Path：" + Application.dataPath + "/" + savePath + itemKey + ".txt");
        }
        AssetDatabase.Refresh();
    }
}
