using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Current;

    public SaveData SaveData;
    [SerializeField] private string placeablesPath = "Placeables";
    [SerializeField] private PlaceableObject _defaulPlaceableObject;

    private void Awake()
    {
        Current = this;

        SaveSystem.Initialize();
    }

    private void Start()
    {
        SaveData = SaveSystem.Load();

        LoadGame();
    }

    private void LoadGame()
    {
        LoadPlaceableObjects();
    }

    private void LoadPlaceableObjects()
    {
        int maxId = 0;

        foreach (var placeableObjectData in SaveData.PlaceableObjectDatas.Values)
        {
            if (placeableObjectData.ID == "1")
            {
                PlaceableObjectItem defaultPlaceableObjectItem = Resources.Load<PlaceableObjectItem>(placeablesPath + "/TriceratopsItem");

                _defaulPlaceableObject.Initialize(defaultPlaceableObjectItem, placeableObjectData);
                _defaulPlaceableObject.PlaceWithoutSave();

                maxId = 1;

                continue;
            }

            PlaceableObjectItem placeableObjectItem = Resources.Load<PlaceableObjectItem>(placeablesPath + "/" + placeableObjectData.ItemName);

            GameObject obj = Instantiate(placeableObjectItem.Prefab, Vector3.zero, Quaternion.identity);

            PlaceableObject placeableObject = obj.GetComponent<PlaceableObject>();

            placeableObject.Initialize(placeableObjectItem, placeableObjectData);

            placeableObject.PlaceWithoutSave();

            if (Int32.Parse(placeableObjectData.ID) > maxId)
            {
                maxId = Int32.Parse(placeableObjectData.ID);
            }
        }

        SaveData.IdCount = maxId;
    }

    public void SaveGame()
    {
        SaveSystem.Save(SaveData);
    }
}