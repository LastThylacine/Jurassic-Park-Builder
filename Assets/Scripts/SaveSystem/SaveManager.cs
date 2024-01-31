using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Current;

    public SaveData saveData;
    [SerializeField] private string placeablesPath = "Placeables";

    private void Awake()
    {
        Current = this;

        SaveSystem.Initialize();
    }

    private void Start()
    {
        saveData = SaveSystem.Load();

        LoadGame();
    }

    private void LoadGame()
    {
        LoadPlaceableObjects();
    }

    private void LoadPlaceableObjects()
    {
        int maxId = 0;

        foreach (var placeableObjectData in saveData.PlaceableObjectDatas.Values)
        {
            PlaceableObjectItem placeableObjectItem = Resources.Load<PlaceableObjectItem>(placeablesPath + "/" + placeableObjectData.ItemName);

            GameObject obj = Instantiate(placeableObjectItem.Prefab, Vector3.zero, Quaternion.identity);

            PlaceableObject placeableObject = obj.GetComponent<PlaceableObject>();

            placeableObject.Initialize(placeableObjectItem, placeableObjectData);

            placeableObject.Placed = true;

            if (Int32.Parse(placeableObjectData.ID) > maxId)
            {
                maxId = Int32.Parse(placeableObjectData.ID);
            }
        }

        SaveData.IdCount = maxId;
        Debug.Log(maxId);
    }

    private void OnDisable()
    {
        SaveSystem.Save(saveData);
    }
}