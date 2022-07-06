using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats instance;
    public int waves = 0;
    public int score = 0;

    private void Start()
    {
        instance = this; 
    }
}
