using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TetraManager : PiecesManager<TetraPiece>
{
    protected override bool UpdateCurrentPiece(Vector3Int[] newCurrentPiece)
    {
        if (_currentPiece.Length != newCurrentPiece.Length)
        {
            return false;
        }

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            _currentPiece[i] = newCurrentPiece[i];
        }

        return true;
    }

    protected override bool CanMoveFromTo(Board board, Vector3Int[] fromPos, Vector3Int[] toPos)
    {
        if (fromPos.Length != toPos.Length)
        {
            return false;
        }

        TetraPiece value;

        for (int i = 0; i < toPos.Length; i++)
        {
            if (toPos[i].x < 0 || toPos[i].x > board.Width || toPos[i].y < 0)
            {
                return false;
            }

            if (board.IsOccupied(toPos[i]) && _piecesInBoard.TryGetValue(toPos[i], out value))
            {
                if (!value.IsCurrentPiece)
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected override bool MoveFromTo(Board board, Vector3Int[] fromPos, Vector3Int[] toPos)
    {
        if (!CanMoveFromTo(board, fromPos, toPos))
        {
            return false;
        }

        TetraPiece[] tempPieces = new TetraPiece[fromPos.Length];

        for (int i = 0; i < fromPos.Length; i++)
        {
            tempPieces[i] = _piecesInBoard[fromPos[i]];
            _piecesInBoard.Remove(fromPos[i]);
        }

        for (int i = 0; i < toPos.Length; i++)
        {
            _piecesInBoard.Add(toPos[i], tempPieces[i]);
        }

        board.MoveTilesFromTo(fromPos, toPos);

        return true;
    }

    protected override bool MoveCurrentPieceTo(Board board, Vector3Int[] toPos)
    {
        if (!MoveFromTo(board, _currentPiece, toPos))
        {
            return false;
        }

        UpdateCurrentPiece(toPos);

        return true;
    }

    /*
    protected override bool CanMoveTo(Board board, Direction direction)
    {
        //eliminar metodo
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
    }*/

    protected override bool MovePieceToDirection(Board board, Direction direction)
    {
        Vector3Int[] newPos = new Vector3Int[_currentPiece.Length];

        switch (direction)
        {
            case Direction.Right:
                for (int i = 0; i < _currentPiece.Length; i++)
                {
                    newPos[i].x = _currentPiece[i].x + 1;
                    newPos[i].y = _currentPiece[i].y;
                }
                break;
            case Direction.Down:
                for (int i = 0; i < _currentPiece.Length; i++)
                {
                    newPos[i].x = _currentPiece[i].x;
                    newPos[i].y = _currentPiece[i].y - 1;
                }
                break;
            case Direction.Left:
                for (int i = 0; i < _currentPiece.Length; i++)
                {
                    newPos[i].x = _currentPiece[i].x - 1;
                    newPos[i].y = _currentPiece[i].y;
                }
                break;
            default:
                break;
        }

        return MoveCurrentPieceTo(board, newPos);
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

            if (i == _currentPiece.Length - 1)
            {
                //no hay pivote, es una pieza cuadrada por lo que no hay que rotar
                return false;
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

            if (pos.x < 0 || pos.x > board.Width || pos.y < 0)
            {
                return false;
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

    protected override bool RotatePiece(Board board, Direction direction = Direction.Right)
    {
        if (direction != Direction.Right && direction != Direction.Left)
        {
            return false;
        }

        Vector3Int myPivot = new Vector3Int();

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            if (_piecesInBoard[_currentPiece[i]].IsPivot)
            {
                myPivot = _currentPiece[i];
                break;
            }

            if (i == _currentPiece.Length - 1)
            {
                //no hay pivote, es una pieza cuadrada por lo que no hay que hacer nada para que se considere rotada
                return true;
            }
        }

        Vector3Int[] posToMove = new Vector3Int[_currentPiece.Length];

        //calculo de rotacion normal
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

        //si la rotacion es posible se efectua
        if (MoveCurrentPieceTo(board, posToMove))
        {
            return true;
        }

        //si no es posible reviso si es una pieza larga
        bool isLong = IsPieceLong(_currentPiece);

        //hacer calculos de rotacion para piezas largas y cortas
        //considerar que las piezas largas pueden quedar hasta con dos tiles de problema seguidos

        return true;
    }

    private bool IsPieceLong(Vector3Int[] pieceDir)
    {
        //cantidad de partes que seguidas hacen que la pieza sea considerada larga, en este caso 4 o mas
        int piecesToBeLong = 4;

        int[] arrX = new int[pieceDir.Length];
        int[] arrY = new int[pieceDir.Length];

        for (int i = 0; i < pieceDir.Length; i++)
        {
            arrX[i] = pieceDir[i].x;
            arrY[i] = pieceDir[i].y;
        }

        Array.Sort(arrY);
        Array.Sort(arrX);

        int numX = 1;
        int numY = 1;

        int lastX = 0;
        int lastY = 0;

        for (int i = 1; i < pieceDir.Length; i++)
        {
            if (pieceDir[i].x == pieceDir[i - 1].x)
            {
                numX++;
            }
            else
            {
                if (numX > lastX)
                {
                    lastX = numX;
                }

                numX = 1;
            }

            if (pieceDir[i].y == pieceDir[i - 1].y)
            {
                numY++;
            }
            else
            {
                if (numY > lastY)
                {
                    lastY = numY;
                }

                numY = 1;
            }
        }

        if (numX >= piecesToBeLong || lastX >= piecesToBeLong || numY >= piecesToBeLong || lastY >= piecesToBeLong)
        {
            return true;
        }

        return false;
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
