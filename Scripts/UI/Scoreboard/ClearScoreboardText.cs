using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearScoreboardText : MonoBehaviour
{
    public bool ItsLocal;
    public GameObject ButtonLocal;
    public GameObject ButtonOnline;

    public void ClearText()
    {
        var MyText = this.gameObject.GetComponent<Text>();
        if (MyText != null)
        {
            MyText.text = "";
        }
        if(ItsLocal == true)
        {
            ItsLocal = false;
            ButtonLocal.SetActive(false);
            ButtonOnline.SetActive(true);
        }
        else
        {
            ItsLocal = true;
            ButtonLocal.SetActive(true);
            ButtonOnline.SetActive(false);
        }
    }
    
}
