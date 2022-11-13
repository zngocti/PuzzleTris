using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraManager : PiecesManager
{
    protected override void GeneratePool()
    {
        if (_myPool.Count > 0)
        {
            return;
        }

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject go = new GameObject();
            _myPool.Add(go.AddComponent<TetraPiece>());
        }
    }

    protected override void GenerateRandomPieces()
    {
        var posiblePieces = (TetraType[])System.Enum.GetValues(typeof(TetraType));

        Utilities.Shuffle<TetraType>(posiblePieces);

        for (int i = 0; i < posiblePieces.Length; i++)
        {
            /*
            TetraPiece myPiece = (TetraPiece)GetAvailablePiece();
            
            if (myPiece == null)
            {
                Debug.LogError("No available piece found");
                return;
            }

            myPiece.SetPiece(posiblePieces[i], true);*/
            _nextPieces.Add(posiblePieces[i]);
        }
    }

    protected override bool CanMoveDown(Board board)
    {
        for (int i = 0; i < _currentPiece.Length; i++)
        {
            var pos = _currentPiece[i];
            pos.y--;

            if (pos.y < 1)
            {
                return false;
            }

            Piece value;

            if (board.IsOccupied(pos) && _piecesInBoard.TryGetValue(pos, out value))
            {
                if (!value.IsCurrentPiece)
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected override void MovePieceDown(Board board)
    {
        if (CanMoveDown(board))
        {
            //mover pieza abajo
        }

        //detener pieza y cambiar a la siguiente
    }

    protected override void MovePieceSideways(Board board, bool moveToRight)
    {
        throw new System.NotImplementedException();
    }

    protected override void RotatePiece(Board board)
    {
        throw new System.NotImplementedException();
    }

    protected override void CheckForMatch(Board board)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
