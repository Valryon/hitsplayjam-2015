using UnityEngine;
using System.Collections;

public class TitleScreenScript : MonoBehaviour 
{
  void Start()
  {
    SoundsScript.Play ("credits", this.transform.position);
  }

	public void StartSelection()
  {
    Application.LoadLevel ("Selection");
  }
}
