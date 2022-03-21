using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoJellybeanMound : JellybeanMound
{
    [SerializeField] int moundNumber;

    protected override int getStatusOfMound()
    {
        return DataService.Instance.saveData.getPlutoBeanPileStatus(moundNumber);
    }

    protected override void setStatusOfMound(int health)
    {
        DataService.Instance.saveData.setPlutoBeanPileStatus(moundNumber, health);
    }
}
