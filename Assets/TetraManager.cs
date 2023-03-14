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
                    for (int i = 0; i < posToMove.Length; i++)
                    {
                        posToMove[i].x++;
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < posToMove.Length; i++)
                    {
                        posToMove[i].y--;
                    }
                    break;
                case Direction.Left:
                    for (int i = 0; i < posToMove.Length; i++)
                    {
                        posToMove[i].x--;
                    }
                    break;
                default:
                    break;
            }

            TetraPiece[] tempPieces = new TetraPiece[_currentPiece.Length];

            for (int i = 0; i < _currentPiece.Length; i++)
            {
                tempPieces[i] = _piecesInBoard[_currentPiece[i]];
                _piecesInBoard.Remove(_currentPiece[i]);
            }

            for (int i = 0; i < posToMove.Length; i++)
            {
                _piecesInBoard.Add(posToMove[i], tempPieces[i]);
            }

            board.MoveTilesFromTo(_currentPiece, posToMove);

            return;
        }

        if (direction == Direction.Down)
        {
            //cambiar a la siguiente pieza
        }
    }

    protected override bool CanRotatePiece(Board board, Direction direction = Direction.Right)
    {
        Vector3Int myPivot = new Vector3Int();

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            if (_piecesInBoard[_currentPiece[i]].IsPivot)
            {
                myPivot = _currentPiece[i];
                break;
            }
        }

        Vector3Int pos = new Vector3Int();
        TetraPiece value;

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            switch (direction)
            {
                case Direction.Right:
                    pos.x = _currentPiece[i].y - myPivot.y + myPivot.x;
                    pos.y = -(_currentPiece[i].x - myPivot.x) + myPivot.y;
                    break;
                case Direction.Left:
                    pos.x = -(_currentPiece[i].y - myPivot.y) + myPivot.x;
                    pos.y = _currentPiece[i].x - myPivot.x + myPivot.y;
                    break;
                default:
                    break;
            }

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

    protected override void RotatePiece(Board board, Direction direction = Direction.Right)
    {
        if (!CanRotatePiece(board, direction))
        {
            return;
        }

        Vector3Int myPivot = new Vector3Int();

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            if (_piecesInBoard[_currentPiece[i]].IsPivot)
            {
                myPivot = _currentPiece[i];
                break;
            }
        }

        Vector3Int[] posToMove = new Vector3Int[_currentPiece.Length];

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            switch (direction)
            {
                case Direction.Right:
                    posToMove[i].x = _currentPiece[i].y - myPivot.y + myPivot.x;
                    posToMove[i].y = -(_currentPiece[i].x - myPivot.x) + myPivot.y;
                    break;
                case Direction.Left:
                    posToMove[i].x = -(_currentPiece[i].y - myPivot.y) + myPivot.x;
                    posToMove[i].y = _currentPiece[i].x - myPivot.x + myPivot.y;
                    break;
                default:
                    break;
            }
        }

        TetraPiece[] tempPieces = new TetraPiece[_currentPiece.Length];

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            tempPieces[i] = _piecesInBoard[_currentPiece[i]];
            _piecesInBoard.Remove(_currentPiece[i]);
        }

        for (int i = 0; i < posToMove.Length; i++)
        {
            _piecesInBoard.Add(posToMove[i], tempPieces[i]);
        }

        board.MoveTilesFromTo(_currentPiece, posToMove);
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
