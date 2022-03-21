using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShootingTransition
{
    public string[] boolNames;
    public bool[] boolValues;
    public string[] intNames;
    public int[] intValues;
    public string[] floatNames;
    public float[]  floatValuesLow;
    public float[] floatValuesHigh;
    public string destination;
}
