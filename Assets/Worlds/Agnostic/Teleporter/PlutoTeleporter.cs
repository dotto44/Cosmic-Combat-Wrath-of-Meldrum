using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoTeleporter : Teleporter
{
    [SerializeField] int teleporterNumber;

    protected override bool getTeleporterUnlocked()
    {
        return DataService.Instance.saveData.getPlutoTeleporterUnlocked(teleporterNumber);
    }

    protected override void setTeleporterUnlocked()
    {
        DataService.Instance.saveData.setPlutoTeleporterUnlocked(teleporterNumber);
    }
}
