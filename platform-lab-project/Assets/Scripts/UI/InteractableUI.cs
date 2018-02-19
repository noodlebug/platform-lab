using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//	interactable object ui elements

public class InteractableUI
{
    UIManager ui;

    //  interactable in range
    private List<Interactable> inRange = new List<Interactable>();
    private int selected;
    //  all currently interactable (in range + linked to in range)
    private List<Interactable> interactables = new List<Interactable>();


    public InteractableUI(UIManager _ui)
    {
        ui = _ui;
    }

    //  invoke interaction
    public void Interact()
    {
        if (interactables.Count == 0)
        {
            return;
        }
        interactables[selected].Interact();
    }

    //  deselect current, increment selected, select current
    public void SelectNext()
    {
        Deselect(interactables[selected]);

        selected = selected >= interactables.Count - 1 ? 0 : selected + 1; 
        
        Select(interactables[selected]);       
    }
    private void Select(Interactable interObject)
    {
        ui.highlight.SetColor(interObject.gameObject, Color.black);        
    }
    private void Deselect(Interactable interObject)
    {
        ui.highlight.SetColor(interObject.gameObject, Color.grey);      
    }

    // order interactables by x position
    private void SortList()
    {
        List<Interactable> sorted = new List<Interactable>(interactables).OrderBy(i => i.XPos()).ToList();
        interactables.Clear();
        interactables.AddRange(sorted);
    }

    //  interactable in range
    public void EnterRange(Interactable interObject)
    {
        if (!interactables.Contains(interObject))
        {
            EnterInteract(interObject.linked);
            SortList();
        }
        
        if (inRange.Count == 0)
        {
            selected = 0;
            Select(interactables[0]);
        }

        inRange.Add(interObject);        
    }
    private void EnterInteract(List<Interactable> interObjects)
    {
        foreach (Interactable interactableObj in interObjects)
        {
            if (interactables.Contains(interactableObj))
            {
                return;
            }
            ui.highlight.AddHighlight(interactableObj.gameObject);
            Deselect(interactableObj);
            interactables.Add(interactableObj);        
        }
        
    }

    //  interactable out of range
    public void ExitRange(Interactable interObject)
    {
        inRange.Remove(interObject);
        foreach (Interactable linked in interObject.linked)
        {
            if (inRange.Contains(linked))
            {
                return;
            }
        }

        ExitInteract(interObject.linked);
    }
    private void ExitInteract(List<Interactable> interObjects)
    {
        foreach (Interactable interactableObj in interObjects)
        {
            if (!interactables.Contains(interactableObj))
            {
                return;
            }
            ui.highlight.RemoveHighlight(interactableObj.gameObject);                
            interactables.Remove(interactableObj);
        }      
    }
}
