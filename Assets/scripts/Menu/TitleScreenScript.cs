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
    music.Stop();
    animator.Play("start");
  }

  void Start()
  {
    var c = SoundsScript.Play("title", Vector3.zero);

    StartCoroutine(Timer.Start(c.length / 2f, () =>
    {
      music.Play();
    }));
  }

  void Update()
  {
    if (button.interactable)
    {
      if (Input.anyKey)
      {
        StartSelection();
      }
    }
  }

  public void StartSelection()
  {
    SoundsScript.Play("play", Vector3.zero);

    button.interactable = false;

    StartCoroutine(Timer.Start(0.35f, () =>
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene("Selection");
    }));
  }
}
