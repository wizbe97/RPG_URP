using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointsDDOL : MonoBehaviour
{
    public static SpawnpointsDDOL Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
