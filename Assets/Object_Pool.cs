using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pool : MonoBehaviour
{
    [SerializeField] public GameObject[] FallBox;
    #region Singleton

    private static Object_Pool _instance;

    public static Object_Pool Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Object_Pool>();
            return _instance;
        }
    }
    #endregion
}
