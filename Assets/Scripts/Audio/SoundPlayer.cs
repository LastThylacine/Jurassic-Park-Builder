using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer
{
	private readonly AudioSource _audioSource;
	public readonly Dictionary<string, AudioClip> sounds;

	public SoundPlayer()
	{
		_audioSource = new AudioSource();
		sounds = new Dictionary<string, AudioClip>();
	}

	public void PlaySound(string sound, float volume = 1f)
    {
        _audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/UISounds/UIClick"), volume);
    }
}