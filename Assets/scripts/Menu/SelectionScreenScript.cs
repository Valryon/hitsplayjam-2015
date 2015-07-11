using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionScreenScript : MonoBehaviour 
{
  public SelectionScreenItemScript prefab;
  public List<PlayerDefinition> defs;

  private SelectionScreenItemScript[] selectionPanels;
  private SelectionScreenItemScript currentSelection;

  void Awake()
  {
    selectionPanels = FindObjectsOfType<SelectionScreenItemScript> ();

    Select (selectionPanels [0]);
  }

  private void Select(SelectionScreenItemScript s)
  {
    if (currentSelection != null) 
    {
      currentSelection.Deselect();
    }
    currentSelection = s;
    currentSelection.Select ();
  }

	public void StartGame()
  {
    Application.LoadLevel ("Main");
  }
}
