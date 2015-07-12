using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionScreenItemScript : MonoBehaviour 
{
  public PlayerDefinition defition;
  public Button button;

  void Awake()
  {
    SpriteState s = button.spriteState;

    s.highlightedSprite = defition.avatarMenu;
    s.disabledSprite = defition.avatarMenuNb;
    s.pressedSprite = defition.avatarMenu;
    button.image.sprite = defition.avatarMenuNb;

    button.spriteState = s;
  }

 
}
