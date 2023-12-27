using UnityEngine;

public class Building : ISelectable
{
    [SerializeField] private Animator _selectedAnimator;

	public bool IsSelected { get; internal set; }

	public void Select()
    {
        _selectedAnimator.SetBool("FadeInOut", true);
    }

    public void Deselect()
    {

        _selectedAnimator.SetBool("FadeInOut", false);
    }
}
