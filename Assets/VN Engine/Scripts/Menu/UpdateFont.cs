using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateFont : MonoBehaviour
{

    public Text text;


    void Start()
    {
        Font fontToUse = Resources.Load("Fonts/" + text.text, typeof(Font)) as Font;
        text.font = fontToUse;
    }
}