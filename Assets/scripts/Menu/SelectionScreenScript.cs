using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionScreenScript : MonoBehaviour
{
  public Button button;
  
  public void StartGame()
  {
    button.interactable = false;
    UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlay");
  }
}
