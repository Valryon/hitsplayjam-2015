using UnityEngine;
using System.Collections;

public class PlayerDefinition : ScriptableObject 
{
  public Sprite avatar;

  public SimpleAnimation defaultAnimation;
  public SimpleAnimation[] animations;

  public string shiityName;
  public bool isGoalKeeper = false;
  public float speed = 5f;
  public float shootForce = 1f;
}
