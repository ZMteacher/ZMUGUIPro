using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineDemo : MonoBehaviour
{
    public GameObject unityOutlineRootObj;
    public GameObject textProOutlineRootObj;
    public GameObject textMeshProOutlineRootObj;


    public void OnUnityOutlineButtonClick()
    {
        unityOutlineRootObj.SetActive(true);
        textProOutlineRootObj.SetActive(false);
        textMeshProOutlineRootObj.SetActive(false);
    }
    public void OnTextProOutlineButtonClick()
    {
        unityOutlineRootObj.SetActive(false);
        textProOutlineRootObj.SetActive(true);
        textMeshProOutlineRootObj.SetActive(false);
    }
    public void OnTextMeshProOutlineButtonClick()
    {
        unityOutlineRootObj.SetActive(false);
        textProOutlineRootObj.SetActive(false);
        textMeshProOutlineRootObj.SetActive(true);
    }

}
