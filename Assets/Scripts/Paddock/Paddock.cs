using UnityEngine;

public class Paddock : MonoBehaviour, ISelectable
{
	[SerializeField] private GameObject _evolutionsChanger;

	private AnimationEventsListener _dinosaurAnimationEventsListener;
	private MoneyObject _moneyObject;
	private Animator _dinosaurAnimator;

	public bool IsSelected { get; internal set; }

	private void Awake()
	{
		_moneyObject = GetComponent<MoneyObject>();
	}

	private void Start()
	{
		// _evolutionsChanger.SetActive(IsSelected);

		_dinosaurAnimationEventsListener = GetComponentInChildren<AnimationEventsListener>();

		_dinosaurAnimator = _dinosaurAnimationEventsListener.GetComponent<Animator>();
	}

	public void Select()
	{
		if (_dinosaurAnimationEventsListener.IsAnimationEnded && _moneyObject.CurrentMoneyInteger != 0)
		{
			_dinosaurAnimator.SetTrigger("Fun");

			// PlaySound(Sounds[2], 0.5f);
		}
	}

	public void Deselect()
	{

	}
}