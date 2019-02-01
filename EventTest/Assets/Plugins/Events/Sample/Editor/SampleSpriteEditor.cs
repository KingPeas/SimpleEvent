/*
 SampleSpriteEditor.cs

 Author:
    Anton Grigoryev <anton.grigorjev@gmail.com>

 Copyright (c) 2011 Anton Grigoryev
*/

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SampleSprite))]
public class SampleSpriteEditor : Editor
{
  public override void OnInspectorGUI()
  {
    Target.Size = EditorGUILayout.Vector2Field("Size", Target.Size);
    Target.Zero = EditorGUILayout.Vector2Field("Zero Point", Target.Zero);
    Target.TextureCoords = EditorGUILayout.RectField("Texture Coordinates", Target.TextureCoords);
    Target.PixelCorrect = EditorGUILayout.Toggle("Pixel Correct", Target.PixelCorrect);

    if (GUI.changed)
    {
      Target.UpdateMesh();
      EditorUtility.SetDirty(target);
    }
  }

  private SampleSprite Target
  {
    get { return target as SampleSprite; }
  }

  //[MenuItem("Sprites/Create/Sample")]
  private static void CreateSprite()
  {
    var gameObject = new GameObject("New Sample Sprite");
    gameObject.AddComponent<SampleSprite>();
    Selection.activeObject = gameObject;
  }
}