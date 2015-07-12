using UnityEngine;
using System.Collections;

public class TitleScreenScript : MonoBehaviour 
{
  public SimpleAnimatorUI animator;

  void Awake()
  {
    animator.Play("start");
  }

  void Start()
  {
  }

	public void StartSelection()
  {
    Application.LoadLevel ("Selection");
  }
}
