using UnityEngine;
using System.Collections;

[System.Serializable]
public class SoundsScriptEntry
{
  public string name;
  public AudioClip[] clips;
}

public class SoundsScript : MonoBehaviour 
{
  public SoundsScriptEntry[] list;
	
  private static SoundsScript instance;

  void Awake()
  {
    instance = this;
  }

  public static AudioClip Play(string name, Vector3 position)
  {
    if (instance == null)
      return null;

    foreach (var e in instance.list) 
    {
      if(e.name.ToLower() == name.ToLower())
      {
        var c = e.clips[Random.Range(0, e.clips.Length)];
        PlayClip(c, position);

        return c;
      }
    }

    return null;
  }

  private static void PlayClip(AudioClip c, Vector3 position)
  {
    AudioSource.PlayClipAtPoint (c, position);
  }
}
