using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : Selectable
{
    [SerializeField] private Animator _selectedAnimator;

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !GridBuildingSystem.Current.TempPlaceableObject)
        {
            Select();
        }
    }

    public override void Select()
    {
        base.Select();

        _selectedAnimator.SetBool("FadeInOut", IsSelected);
    }

    public override void Unselect()
    {
        base.Unselect();

        _selectedAnimator.SetBool("FadeInOut", IsSelected);
    }
}
