using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game Objects/Placeable Object Item", order = 0)]
public class PlaceableObjectItem : ScriptableObject
{
    public GameObject Prefab;
}
