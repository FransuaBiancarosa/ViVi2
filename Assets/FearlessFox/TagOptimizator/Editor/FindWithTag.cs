
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindWithTag : EditorWindow
{
  private static EditorWindow editorWindow;

  private static int selectedTag = 0;

  private string[] listTags;

  private List<GameObject> resultsFind = new List<GameObject>();

  private int indexTagActive = 0;
  private bool isFind = false;
  private bool allSelected = false;

  [MenuItem("Window/FearlessFox/Find With Tag")]
  public static void FindGameObjecWithTag()
  {
    editorWindow = EditorWindow.GetWindow(typeof(FindWithTag));
    editorWindow.title = "FindWithTag";
    editorWindow.minSize = new Vector2(400, 400);
    editorWindow.maxSize = new Vector2(400, 400);
  }

  private void OnGUI()
  {
    GUILayout.Space(5f);
    GUIStyle style = new GUIStyle();
    style.richText = true;
    style.alignment = TextAnchor.MiddleCenter;
    EditorGUILayout.LabelField("<size=15>Select Tag</size>", style);
    listTags = UnityEditorInternal.InternalEditorUtility.tags;
    GUILayout.Space(5f);
    selectedTag = EditorGUILayout.Popup(selectedTag, listTags);
    if (GUILayout.Button("Find"))
    {
      resultsFind.Clear();
      resultsFind.AddRange(GameObject.FindGameObjectsWithTag(listTags[selectedTag]));

      if (resultsFind.Count > 0)
      {
        Selection.objects = resultsFind.ToArray();
        indexTagActive = 0;
        if (resultsFind.Count > 1)
          allSelected = true;
      }
      isFind = true;
    }
    GUILayout.Space(10f);

    if (resultsFind.Count >= 0 && isFind == true)
    {
      EditorGUILayout.LabelField("Found: " + resultsFind.Count);
      GUILayout.Space(10f);
      if (resultsFind.Count > 1)
      {
        if (GUILayout.Button("Next"))
        {
          if (resultsFind.Count > 0)
          {
            if (indexTagActive < resultsFind.Count - 1)
              indexTagActive++;
            else
              indexTagActive = 0;
            Selection.activeObject = resultsFind[indexTagActive];
            allSelected = false;
          }
        }
        if (GUILayout.Button("Previus"))
        {
          if (indexTagActive > 0)
            indexTagActive--;
          else
            indexTagActive = resultsFind.Count - 1;
          Selection.activeObject = resultsFind[indexTagActive];
          allSelected = false;
        }
      }
      GUILayout.Space(10f);
      if (resultsFind.Count > 0)
      {
        if (allSelected == true)
          EditorGUILayout.LabelField("Selected: <size=20><color=blue><b>All Selected</b></color></size>", style);
        else
          EditorGUILayout.LabelField(string.Format("Selected: <size=20><color=blue><b>{0}</b></color></size>",
           resultsFind[indexTagActive].name), style);
      }
      GUILayout.Space(20f);
      if (GUILayout.Button("Reset"))
      {
        resultsFind.Clear();
        Selection.activeObject = null;
        allSelected = false;
        isFind = false;
      }
    }

    if (EditorWindow.focusedWindow == editorWindow)
      OnViewWindows();
  }

  private void OnViewWindows()
  {

  }
}
