using UnityEngine;

public class Paddock : Selectable
{
    [SerializeField] private GameObject _evolutionsChanger;

    private AnimationEventsListener _dinosaurAnimationEventsListener;
    private MoneyObject _moneyObject;
    private Animator _dinosaurAnimator;

    private void Awake()
    {
        _moneyObject = GetComponent<MoneyObject>();
    }

    private void Start()
    {
        _evolutionsChanger.SetActive(IsSelected);

        _dinosaurAnimationEventsListener = GetComponentInChildren<AnimationEventsListener>();

        _dinosaurAnimator = _dinosaurAnimationEventsListener.GetComponent<Animator>();
    }

    public override void Select()
    {
        base.Select();

        _evolutionsChanger.SetActive(IsSelected);

        if (_dinosaurAnimationEventsListener.IsAnimationEnded && _moneyObject.CurrentMoneyInteger != 0)
        {
            _dinosaurAnimator.SetTrigger("Fun");

            PlaySound(Sounds[2], 0.5f);
        }
    }

    public override void Unselect()
    {
        base.Unselect();

        _evolutionsChanger.SetActive(IsSelected);
    }
}
