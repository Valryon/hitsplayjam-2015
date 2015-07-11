using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionScreenItemScript : MonoBehaviour 
{
  public PlayerDefinition defition;
  public Image image;
  public Text text;

  void Awake()
  {
    if (defition != null) {
      image.sprite = defition.avatar;
      text.text = defition.shiityName;
    }
  }
}
