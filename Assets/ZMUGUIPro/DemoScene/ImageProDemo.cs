using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProDemo : MonoBehaviour
{
    public GameObject imageMaskObj;
    public GameObject UnityMaskObj;

    public void OnImageProMaskButtonClick()
    {
        imageMaskObj.SetActive(true);
        UnityMaskObj.SetActive(false);
    }

    public void OnUnityMaskButtonClick()
    {
        imageMaskObj.SetActive(false);
        UnityMaskObj.SetActive(true);
    }
}
