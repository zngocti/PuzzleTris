using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraManager : PiecesManager<TetraPiece>
{
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

            TetraPiece value;

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
