using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    protected override bool CanRotateAndMovePiece(Board board, Direction movementDirection = Direction.Right, Direction rotationDirection = Direction.Right)
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

        bool isLong = false;

        int piecesInLong = 4;

        int[] arrX = new int[_currentPiece.Length];
        int[] arrY = new int[_currentPiece.Length];

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            arrX[i] = _currentPiece[i].x;
            arrY[i] = _currentPiece[i].y;
        }

        Array.Sort(arrY);
        Array.Sort(arrX);

        int numX = 1;
        int numY = 1;

        int lastX = 0;
        int lastY = 0;

        for (int i = 1; i < _currentPiece.Length; i++)
        {
            if (_currentPiece[i].x == _currentPiece[i - 1].x)
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

            if (_currentPiece[i].y == _currentPiece[i - 1].y)
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

        if (numX >= piecesInLong || lastX >= piecesInLong || numY >= piecesInLong || lastY >= piecesInLong)
        {
            isLong = true;
        }

        //hasta aca solo revise si es una pieza larga de 4 partes, depende si lo es o no debe girar diferente
        //es mejor cambiar la forma en la que hago si puede rotar o moverse
        //hacer un metodo de puede ocupar tal espacio, usando arrays

        Vector3Int[] posToMove = new Vector3Int[_currentPiece.Length];

        bool redo = false;

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            switch (rotationDirection)
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

            switch (movementDirection)
            {
                case Direction.Right:
                    posToMove[i].x++;
                    break;
                case Direction.Left:
                    posToMove[i].x--;
                    break;
                default:
                    break;
            }

            if (posToMove[i].x < 0 || posToMove[i].x > board.Width || posToMove[i].y > 0)
            {
                redo = true;
            }
        }

        if (redo)
        {
            //esto es parte del codigo de arriba, refactorizarlo

            for (int i = 0; i < _currentPiece.Length; i++)
            {
                switch (movementDirection)
                {
                    case Direction.Right:
                        posToMove[i].x++;
                        break;
                    case Direction.Left:
                        posToMove[i].x--;
                        break;
                    default:
                        break;
                }
            }
        }

        for (int i = 0; i < _currentPiece.Length; i++)
        {

        }

        return true;
    }

    protected override void RotateAndMovePiece(Board board, Direction movementDirection, Direction rotationDirection)
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
