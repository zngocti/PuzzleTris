using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraManager : PiecesManager<TetraPiece>
{
    protected override bool CanMoveTo(Board board, Direction direction)
    {
        for (int i = 0; i < _currentPiece.Length; i++)
        {
            var pos = _currentPiece[i];

            switch (direction)
            {
                case Direction.Right:
                    if (pos.x + 1 >= board.Width)
                    {
                        return false;
                    }
                    break;
                case Direction.Down:
                    if (pos.y - 1 < 0)
                    {
                        return false;
                    }
                    break;
                case Direction.Left:
                    if (pos.x - 1 < 0)
                    {
                        return false;
                    }
                    break;
                default:
                    break;
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

    protected override void MovePieceTo(Board board, Direction direction)
    {
        if (CanMoveTo(board, direction))
        {
            Vector3Int[] posToMove = new Vector3Int[_currentPiece.Length];

            switch (direction)
            {
                case Direction.Right:
                    //mover current piece?
                    break;
                case Direction.Down:
                    break;
                case Direction.Left:
                    break;
                default:
                    break;
            }
            //mover pieza en direcion en el board
            //board.MoveTilesFromTo()
            //mover la pieza en piecesinboard aca
            //y hacer return
        }

        if (direction == Direction.Down)
        {
            //detener pieza y cambiar a la siguiente
        }
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
