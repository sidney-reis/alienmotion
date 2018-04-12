using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalMessage : MonoBehaviour
{
  public static int finalTextStep = 0;
  public static Transform finalTextField;

  public static void setFinalText()
  {
    finalTextField = GameObject.Find("EndgameCanvas").transform.Find("EndgameMessage/Scroll View/Viewport/Content/Text").transform;
    finalTextField.GetComponent<Text>().text = PlayerInfo.finalTexts[0];
  }

  public void okButton()
  {
    if (finalTextStep + 1 == PlayerInfo.finalTexts.Length)
    {
      Application.Quit();
    }
    else
    {
      finalTextStep++;
      finalTextField.GetComponent<Text>().text = PlayerInfo.finalTexts[finalTextStep];
    }
  }
}