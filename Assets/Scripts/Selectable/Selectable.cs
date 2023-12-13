using UnityEngine;

public class Selectable : SoundObject
{
    public bool IsSelected;

    [SerializeField] private ObjectsFadeInOut _objectsFadeInOut;

    public virtual void Select()
    {
        SelectablesManager.Current.UnselectAll();

        IsSelected = true;

        _objectsFadeInOut.FadeInOut(IsSelected);

        SelectablesManager.Current.SetIsSomethingSelected(IsSelected);
    }

    public virtual void Unselect()
    {
        IsSelected = false;

        _objectsFadeInOut.FadeInOut(IsSelected);

        SelectablesManager.Current.SetIsSomethingSelected(IsSelected);
    }

    public void PlayPlacementSound()
    {
        PlaySound(Sounds[1]);
    }
}
