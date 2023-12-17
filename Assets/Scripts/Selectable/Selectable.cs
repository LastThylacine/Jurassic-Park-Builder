using UnityEngine;

public class Selectable : SoundObject
{
    public bool IsSelected;

    [SerializeField] private FadeInOut _objectsFadeInOut;

    public virtual void Select()
    {
        SelectablesManager.Current.UnselectAll();

        IsSelected = true;

        _objectsFadeInOut.SetFade(IsSelected);

        SelectablesManager.Current.SetIsSomethingSelected(IsSelected);
    }

    public virtual void Unselect()
    {
        IsSelected = false;

        _objectsFadeInOut.SetFade(IsSelected);

        SelectablesManager.Current.SetIsSomethingSelected(IsSelected);
    }

    public void PlayPlacementSound()
    {
        PlaySound(Sounds[1]);
    }
}
