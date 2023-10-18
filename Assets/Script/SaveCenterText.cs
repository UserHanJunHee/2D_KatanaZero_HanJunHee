using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCenterText : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
