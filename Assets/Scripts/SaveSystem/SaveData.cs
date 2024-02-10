using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class SaveData
{
    public static int IdCount;

    public Dictionary<string, PlaceableObjectData> PlaceableObjectDatas = new Dictionary<string, PlaceableObjectData>();

    public static string GenerateId()
    {
        IdCount++;
        return IdCount.ToString();
    }

    public void AddData(Data data)
    {
        if (data is PlaceableObjectData placeableObjectData)
        {
            if (PlaceableObjectDatas.ContainsKey(placeableObjectData.ID))
            {
                PlaceableObjectDatas[placeableObjectData.ID] = placeableObjectData;
            }
            else
            {
                PlaceableObjectDatas.Add(placeableObjectData.ID, placeableObjectData);
            }
        }
    }

    public void RemoveData(Data data)
    {
        if (data is PlaceableObjectData placeableObjectData)
        {
            if (PlaceableObjectDatas.ContainsKey(placeableObjectData.ID))
            {
                PlaceableObjectDatas.Remove(placeableObjectData.ID);
            }
        }
    }

    [OnDeserialized]
    internal void OnDeserealizedMethod(StreamingContext streamingContext)
    {
        PlaceableObjectDatas ??= new Dictionary<string, PlaceableObjectData>();
    }
}
