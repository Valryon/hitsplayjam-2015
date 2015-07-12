using UnityEngine;
using System.Collections;

public enum ROLE
{
  Defense,
  Attack,
  Keeper  
};

public class PlayerDefinition : ScriptableObject 
{
  [Header("Stats")]
  
  public ROLE role = ROLE.Defense;
  
  [Range(1,5)]
  public int speed = 1;
  
  [Range(1,5)]
  public int shootForce = 1;
  
  [Range(1,5)]
  public int lobForce = 1;

  [Header("Menu")]
  public Sprite avatar;
  public Sprite avatarBg;
  public Sprite avatarBgNb;
  public string shiityName;
  public string biography;
}
