using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsDDOL : MonoBehaviour
{
    public static PickupsDDOL Instance;
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
