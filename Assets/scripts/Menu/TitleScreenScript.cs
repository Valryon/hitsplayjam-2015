using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleScreenScript : MonoBehaviour 
{
  public AudioSource music;
  public SimpleAnimatorUI animator;
  public Button button;

  void Awake()
  {
    music.enabled = false;
    animator.Play("start");
  }

  void Start()
  {
    SoundsScript.Play ("title", Vector3.zero);

    StartCoroutine (Timer.Start (1f, () => {
      music.enabled = true;
    }));
  }

	public void StartSelection()
  {
    SoundsScript.Play ("play", Vector3.zero);

    button.interactable = false;

    StartCoroutine (Timer.Start (0.35f, () => {
      Application.LoadLevel ("Selection");
    }));
  }
}
