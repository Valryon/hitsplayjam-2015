using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionScreenScript : MonoBehaviour 
{
  public Button button;

  void Awake()
  {
  }

  void Update()
  {
    if (button.interactable) {
      if(Input.anyKey)
      {
        StartGame();
      }
    }
  }

	public void StartGame()
  {
    button.interactable = false;
    Application.LoadLevel ("Main");
  }
}
