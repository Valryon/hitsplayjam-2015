using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionScreenScript : MonoBehaviour 
{
  public SelectionScreenItemScript[] selectionPanels;
  public SelectionScreenItemScript currentSelection;

  void Awake()
  {
    selectionPanels = FindObjectsOfType<SelectionScreenScript> ();

    Select (selectionPanels [0]);
  }

  private void Select(SelectionScreenScript s)
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
