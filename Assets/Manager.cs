using UnityEngine;

public class Manager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
