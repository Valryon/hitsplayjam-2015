using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public static class EditorMenus  
{
  [MenuItem("SFF/Create.../Player definition")]
  [MenuItem("Assets/Create/SFF/Player definition")]
  public static void CreatePlayerDef()
  {
    ScriptableObjectUtility.CreateAsset<PlayerDefinition>();
  }

  [MenuItem("Assets/SFF/Process textures")]
  public static void ProcessTextures()
  {
    string path = AssetDatabase.GetAssetPath (Selection.activeObject);
    string dir = path;
    
    GenerateOrUpdateAnimForDir (dir);
  }

  public static void GenerateOrUpdateAnimForDir(string dir)
  {
    if (Directory.Exists(dir))
    {
      // Extract filename
      string filename = Path.GetFileName(dir) + ".asset";
      
      // Existing?
      SimpleAnimation anim = (SimpleAnimation)AssetDatabase.LoadAssetAtPath(dir + "/" + filename, typeof(SimpleAnimation));
      
      // No, simply create
      if (anim == null)
      {
        // Make sure there is at least one image file
        bool spriteFound = false;
        foreach (var file in Directory.GetFiles(dir))
        {
          if (Path.GetExtension(file).ToLower().EndsWith("png"))
          {
            spriteFound = true;
            break;
          }
        }
        
        if (spriteFound)
        {
          anim = ScriptableObjectUtility.CreateAsset<SimpleAnimation>(dir, filename);
          SimpleAnimationMenus.AutoFillAnimation(anim);
        }
      }
      else
      {
        SimpleAnimationMenus.AutoFillAnimation(anim);
      }
      
      // Scan all subdirectories (ignore current one)
      foreach (string subdir in Directory.GetDirectories(dir))
      {
        GenerateOrUpdateAnimForDir(subdir);
      }
    }
  }
}
