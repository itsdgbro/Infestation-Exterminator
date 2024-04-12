using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IDataPersistence
{


    public void LoadData(GameData data)
    {
        this.transform.position = data.cubePos;
    }

    public void SaveData(GameData data)
    {
        data.cubePos= transform.position;
    }
}
