// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class SimpleAnimationMenus
{
  [MenuItem("Assets/Create/PXN/Simple Sprite Animation")]
  public static void CreateSpriteFrameAnimation()
  {
    var anim = ScriptableObjectUtility.CreateAsset<SimpleAnimation>(Selection.activeObject.name);

    AutoFillAnimation(anim);
  }

  public static void AutoFillAnimation(SimpleAnimation anim, bool reverse = false, bool clean = true)
  {
    if (anim.allowAutoUpdate)
    {
      string path = AssetDatabase.GetAssetPath(anim.GetInstanceID());

      // Get the directory
      string dir = Path.GetDirectoryName(path);

      if(anim.name.ToLower().Contains("boucle") || anim.name.ToLower().Contains("static"))
      {
        anim.loop = true;
      }

      if (Directory.Exists(dir))
      {
        // List all sprites from the dir
        List<Sprite> sprites = new List<Sprite>();

        if(clean == false)
        {
          sprites.AddRange(anim.frames);
        }

        foreach (string file in Directory.GetFiles(dir))
        {
          foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(file))
          {
            if (asset is Sprite)
            {
              // Add them to the anim
              sprites.Add(asset as Sprite);
            }
          }
        }

        if(reverse == false) 
        {
          anim.frames = sprites.ToArray();
        }
        else 
        {
          sprites.Reverse();
          anim.frames = sprites.ToArray();
        }

        EditorUtility.SetDirty(anim);

        AssetDatabase.SaveAssets();
        sprites = null;
      }
    }
  }
}
#endif