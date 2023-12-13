using UnityEngine;

public class Selectable : SoundObject
{
    public bool IsSelected;

    public virtual void Select()
    {
        SelectablesManager.Current.UnselectAll();

        IsSelected = true;

        SelectablesManager.Current.SetIsSomethingSelected(IsSelected);
    }

    public virtual void Unselect()
    {
        IsSelected = false;

        SelectablesManager.Current.SetIsSomethingSelected(IsSelected);
    }

    public void PlayPlacementSound()
    {
        PlaySound(Sounds[1]);
    }
}
