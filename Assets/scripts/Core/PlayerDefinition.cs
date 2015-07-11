using UnityEngine;
using System.Collections;

public class PlayerDefinition : ScriptableObject 
{
  public Sprite avatar;

  public SimpleAnimation defaultAnimation;
  public SimpleAnimation[] animations;

  public string shiityName;
  public bool isGoalKeeper = false;

  [Range(1,5)]
  public int speed = 1;

  [Range(1,5)]
  public int shootForce = 1;
}
