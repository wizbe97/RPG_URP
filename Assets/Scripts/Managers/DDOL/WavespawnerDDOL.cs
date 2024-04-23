using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavespawnerDDOL : MonoBehaviour
{
    public static WavespawnerDDOL Instance;
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
