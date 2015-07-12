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

  private static System.Random betterRandom;

  static SoundsScript()
  {
    betterRandom = new System.Random (System.DateTime.Now.Millisecond);
  } 

  void Awake()
  {
    instance = this;
  }

  public static AudioClip Play(string name, Vector3 position)
  {
    AudioClip c = Get (name);

    if (c != null) 
    {
      PlayClip (c, position);
    }

    return c;
  }

  public static AudioClip Get (string name)
  {
    if (instance == null)
      return null;
    
    foreach (var e in instance.list) 
    {
      if(e.name.ToLower() == name.ToLower())
      {
        var c = e.clips[betterRandom.Next(0, e.clips.Length)];
        
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
