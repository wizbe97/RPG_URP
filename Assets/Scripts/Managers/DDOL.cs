using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    private static readonly HashSet<GameObject> instances = new HashSet<GameObject>();

    private void Awake()
    {
        if (!instances.Contains(gameObject))
        {
            DontDestroyOnLoad(gameObject);
            instances.Add(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
