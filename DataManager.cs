using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string dataFilePath;

    private void Awake()
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, "itemData.json");
    }

    public void SaveData(List<ItemData> itemList)
    {
        string json = JsonUtility.ToJson(new Serialization<ItemData>(itemList), true);
        File.WriteAllText(dataFilePath, json);
        Debug.Log("Data saved to: " + dataFilePath);
    }

    public List<ItemData> LoadData()
    {
        if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            List<ItemData> itemList = JsonUtility.FromJson<Serialization<ItemData>>(json).ToList();
            Debug.Log("Data loaded from: " + dataFilePath);
            return itemList;
        }
        return new List<ItemData>();
    }
}

[System.Serializable]
public class Serialization<T>
{
    public List<T> target;

    public Serialization(List<T> target)
    {
        this.target = target;
    }

    public List<T> ToList()
    {
        return target;
    }
}
