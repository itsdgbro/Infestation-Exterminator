using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        Debug.Log("DWEA");
    }

    public void SaveData(GameData data)
    {
        throw new System.NotImplementedException();
    }
}
