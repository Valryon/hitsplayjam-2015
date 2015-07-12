using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionScreenItemScript : Selectable 
{
  public PlayerDefinition defition;
  public Button button;
  public GameObject popup;
  public Text popupName, popupBio;

  void Awake()
  {
    SpriteState s = button.spriteState;

    s.highlightedSprite = null;
    s.disabledSprite = defition.avatarMenuNb;
    s.pressedSprite = defition.avatarMenu;
    button.image.sprite = defition.avatarMenuNb;

    button.spriteState = s;
  }

  public override void OnPointerEnter (UnityEngine.EventSystems.PointerEventData eventData)
  {
    base.OnPointerEnter (eventData);

    button.image.sprite = defition.avatarMenu;

    popupName.text = defition.shiityName;
    popupBio.text = defition.biography;
  }

  public override void OnPointerExit (UnityEngine.EventSystems.PointerEventData eventData)
  {
    base.OnPointerDown (eventData);

    button.image.sprite = defition.avatarMenuNb;
  }
}
