using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EditorMenus  
{
  [MenuItem("SFF/Create.../Player definition")]
  [MenuItem("Assets/Create/SFF/Player definition")]
  public static void CreatePlayerDef()
  {
    ScriptableObjectUtility.CreateAsset<PlayerDefinition>();
  }
}
