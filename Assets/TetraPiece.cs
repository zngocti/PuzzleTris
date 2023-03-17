using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPiece : Piece
{
    private int _currentPosition = 0;

    [System.Serializable]
    public struct Offsets
    {
        public Vector3Int[] _theOffsetClock;
        public Vector3Int[] _theOffsetCounterClock;
    }

    [SerializeField]
    private Offsets[] _positions;
}
