using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPiece : Piece
{
    [System.Serializable]
    public struct PossiblePositions
    {
        public Vector3Int[] _clockwiseAttempts;
        public Vector3Int[] _counterclockwiseAttempts;
    }

    [SerializeField]
    private PossiblePositions[] _positionsToTry = new PossiblePositions[4];

    //offsets posibles a probar para la posicion actual de la pieza
    public Vector3Int[] PositionsToTryInDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                if (_currentPosition + 1 < _positionsToTry.Length)
                {
                    return _positionsToTry[_currentPosition + 1]._clockwiseAttempts;
                }

                return _positionsToTry[0]._clockwiseAttempts;

            case Direction.Left:
                if (_currentPosition - 1 >= 0)
                {
                    return _positionsToTry[_currentPosition - 1]._counterclockwiseAttempts;
                }

                return _positionsToTry[_positionsToTry.Length - 1]._counterclockwiseAttempts;

            default:
                return null;
        }
    }

    public void RotateCurrentPosition(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                if (_currentPosition + 1 < _positionsToTry.Length)
                {
                    _currentPosition++;
                    return;
                }

                _currentPosition = 0;
                break;
            case Direction.Left:
                if (_currentPosition - 1 >= 0)
                {
                    _currentPosition--;
                    return;
                }

                _currentPosition = _positionsToTry.Length - 1;
                break;
            default:
                break;
        }
    }
}
