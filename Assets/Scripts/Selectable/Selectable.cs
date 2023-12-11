using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Building _building;
    private Paddock _paddock;
    private SelectablesManager _selectablesManager;

    public bool IsSelected;

    public virtual void Start()
    {
        //if (GetComponent<Building>())
        //    _building = GetComponent<Building>();

        //if (GetComponent<Paddock>())
        //    _paddock = GetComponent<Paddock>();

        _selectablesManager = FindObjectOfType<SelectablesManager>();

        Unselect();
    }

    //private void Update()
    //{
    //    if (_paddock != null)
    //        IsSelected = _paddock.IsSelected;

    //    if (_building != null)
    //        IsSelected = _building.IsSelected;
    //}

    public virtual void Select()
    {
        _selectablesManager.UnselectAll();

        IsSelected = true;

        _selectablesManager.SetIsSomethingSelected(IsSelected);
    }

    public virtual void Unselect()
    {
        IsSelected = false;

        _selectablesManager.SetIsSomethingSelected(IsSelected);
    }
}
