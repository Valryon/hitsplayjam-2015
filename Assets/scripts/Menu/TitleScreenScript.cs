using UnityEngine;
using System.Collections;

public class TitleScreenScript : MonoBehaviour 
{
  void Start()
  {
  }

	public void StartSelection()
  {
    Application.LoadLevel ("Selection");
  }
}
