using UnityEngine;

public abstract class Placeable : MonoBehaviour, ISelectable
{
	private readonly SoundPlayer soundMachine = new SoundPlayer();

	public Animator animator;
	public Vector3 Position { get => transform.position; }
	public BoundsInt Area { get; protected set; }

	private void Start()
	{
		AudioClip clip = Resources.Load<AudioClip>("/Audio/UISounds/UIClick.wav");
		
		soundMachine.sounds.Add("OnClick", clip);
		animator = GetComponent<Animator>();
	}

	public void Select()
	{
		soundMachine.PlaySound("OnClick");
		animator.SetBool("Selected", true);
	}

	public void Deselect()
	{
		animator.SetBool("Selected", false);
	}
}