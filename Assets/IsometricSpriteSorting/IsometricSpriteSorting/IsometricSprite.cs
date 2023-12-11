using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ! Important !  You must first use the Editor Sprite Sorter in order for this one to work.
/// Use this component attached to a GameObject for it to sort itself in the game world.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class IsometricSprite : MonoBehaviour
{
    /// <summary>
    /// This precision value will determine how often changes in the sprite layers occur when moving through the Y axis. A higher value means more precision.
    /// The precision is negative because we will sort from downwards to upwards as farther apart in our worldspace
    /// </summary>
    public static int PrecisionValue
    {
        get
        {
            return -10; 
        }
    }

    private SpriteRenderer spriteRenderer;

    void Start ()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); //Retrieve this object sprite renderer
	}
		
	void Update ()
    {
        spriteRenderer.sortingOrder = (int)(spriteRenderer.gameObject.transform.position.y * PrecisionValue); //Will set the sprites sorting layer order based on its Y axis
    }
}
