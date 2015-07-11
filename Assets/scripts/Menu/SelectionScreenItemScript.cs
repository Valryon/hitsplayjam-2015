using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionScreenItemScript : MonoBehaviour 
{
  public PlayerDefinition defition;
  public Image image;
  public Image fond;
  public Image fondnb;
  public Text text;

  void Awake()
  {
    SetDefinition (defition);

    Deselect ();
  }

  public void SetDefinition(PlayerDefinition d)
  {
    this.defition = d;

    image.sprite = defition.avatar;
    fond.sprite = defition.avatarBg;
    fondnb.sprite = defition.avatarBgNb;

    text.text = defition.shiityName;
  }

  public void Select()
  {
    fond.enabled = false;
    transform.localScale = Vector3.one;
  }

  public void Deselect()
  {
    fond.enabled = true;
    transform.localScale = Vector3.one * 0.65f;
  }
}
