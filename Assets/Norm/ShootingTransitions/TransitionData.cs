using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transition", menuName = "ScriptableObjects/Transition", order = 1)]
public class TransitionData : ScriptableObject
{
    public string[] boolNames;
    public bool[] boolValues;
    public string[] intNames;
    public int[] intValues;
    public bool[] notEqual;
    public string[] floatNames;
    public float[] floatValuesLow;
    public float[] floatValuesHigh;
    public string destination;
    public string separateArmDestination;
}
