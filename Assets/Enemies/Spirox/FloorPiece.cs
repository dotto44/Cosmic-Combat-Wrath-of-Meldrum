using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPiece : MonoBehaviour
{
    [SerializeField] FloorPieces floorPieces;


    void drop4()
    {
        floorPieces.dropFloorPiece4();
    }

    void drop3()
    {
        floorPieces.dropFloorPiece3();
    }

    void drop2()
    {
        floorPieces.dropFloorPiece2();
    }

    void drop5()
    {
        floorPieces.dropFloorPiece5();
    }

    void drop6()
    {
        floorPieces.dropFloorPiece6();
    }
}
