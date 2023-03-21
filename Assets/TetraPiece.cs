using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPiece : Piece
{
    private int _currentPosition = 0;

    [System.Serializable]
    public struct PossiblePositions
    {
        public Vector3Int[] _clockwiseAttempts;
        public Vector3Int[] _counterclockwiseAttempts;
    }

    [SerializeField]
    private PossiblePositions[] _positionsToTry = new PossiblePositions[4];
}
