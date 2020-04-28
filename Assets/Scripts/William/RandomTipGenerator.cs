using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RandomTipGenerator : MonoBehaviour
{
    public Text toolTipText;

    public string[] toolTips;

    private int num;

    public void Start()
    {
        num = Random.Range(0, toolTips.Length);

        placeText();
    }

    public void placeText()
    {
        toolTipText.text = toolTips[num];
    }
}
