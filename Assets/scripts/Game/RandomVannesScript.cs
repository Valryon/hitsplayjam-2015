using UnityEngine;
using System.Collections;

public class RandomVannesScript : MonoBehaviour 
{
  public AudioSource source;

  private float cooldown;
  private GameScript gameScript;

	void Awake()
  {
    cooldown = Random.Range (5f, 10f);

    gameScript = FindObjectOfType<GameScript> ();
  }

  void Update()
  {
    if (gameScript.Paused)
      return;

    cooldown -= Time.deltaTime;

    if(cooldown < 0)
    {
      cooldown = Random.Range (10f, 20f);

      var clip = SoundsScript.Get("random");
      source.clip = clip;
      source.loop = false;
      source.Play();
    }
  }
}
