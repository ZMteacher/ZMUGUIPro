using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonProDemo : MonoBehaviour
{
    public static ButtonProDemo Instance;//测试代码，单例不要这么写
    public GameObject doubleObj;
    public GameObject longPressObj;
    public AudioSource audioSound;
    private void Awake()
    {
        Instance = this;
    }
    public void OnLongPressButtonClick()
    {
        Debug.Log("触发长按事件");
        longPressObj.SetActive(true);
    }


    public void OnDoubleButtonClick()
    {
        Debug.Log("触发双击事件");
        doubleObj.SetActive(true);
    }

    public void ResetObjState()
    {
        doubleObj.SetActive(false);
        longPressObj.SetActive(false);
    }

    public void PlaySound()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("ButtonClick");
        audioSound.PlayOneShot(audioClip);
    }
}
