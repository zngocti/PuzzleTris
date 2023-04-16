using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TetraManager : PiecesManager<TetraPiece>
{
    [SerializeField]
    private TetraShadow _shadowPiece;

    protected override bool CanMoveFromTo(Board board, Vector3Int[] fromPos, Vector3Int[] toPos, bool insideBoard)
    {
        if (fromPos.Length != toPos.Length)
        {
            return false;
        }

        TetraPiece value;

        for (int i = 0; i < toPos.Length; i++)
        {
            if (insideBoard)
            {
                if (toPos[i].x < 0 || toPos[i].x > board.Width - 1 || toPos[i].y < 0)
                {
                    return false;
                }
            }

            if (_piecesInBoard.TryGetValue(toPos[i], out value))
            {
                if (!value.IsCurrentPiece)
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected override bool MoveFromTo(Board board, Vector3Int[] fromPos, Vector3Int[] toPos, bool insideBoard)
    {
        if (!CanMoveFromTo(board, fromPos, toPos, insideBoard))
        {
            Debug.Log("No puede mover");
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

    protected override bool MoveUpdateCurrentPieceTo(Board board, Vector3Int[] toPos, bool insideBoard)
    {
        if (!MoveFromTo(board, _currentPiece, toPos, insideBoard))
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

    public override bool MovePieceToDirection(Board board, Direction direction)
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

        return MoveUpdateCurrentPieceTo(board, newPos, true);
    }

    public override Vector3Int[] RotatePiece(Board board, out bool pieceMoved, Direction direction = Direction.Right, bool insideBoard = true, Vector3Int[] _piecePosition = null)
    {
        pieceMoved = false;

        if (direction != Direction.Right && direction != Direction.Left)
        {
            return null;
        }

        bool isCurrent = false;

        if (_piecePosition == null)
        {
            _piecePosition = _currentPiece;
            isCurrent = true;
        }

        Vector3Int myPivot = new Vector3Int();

        for (int i = 0; i < _piecePosition.Length; i++)
        {
            if (_piecesInBoard[_piecePosition[i]].IsPivot)
            {
                myPivot = _piecePosition[i];
                break;
            }

            if (i == _piecePosition.Length - 1)
            {
                //no hay pivote, es una pieza cuadrada por lo que no hay que hacer nada para que se considere rotada
                //tengo que cambiar un poco esto, las piezas cuadradas tienen que tener pivote para ser ubicadas
                return _piecePosition;
            }
        }

        Vector3Int[] posToMove = new Vector3Int[_piecePosition.Length];

        //calculo de rotacion normal
        for (int i = 0; i < _piecePosition.Length; i++)
        {
            switch (direction)
            {
                case Direction.Right:
                    posToMove[i].x = _piecePosition[i].y - myPivot.y + myPivot.x;
                    posToMove[i].y = -(_piecePosition[i].x - myPivot.x) + myPivot.y;
                    break;
                case Direction.Left:
                    posToMove[i].x = -(_piecePosition[i].y - myPivot.y) + myPivot.x;
                    posToMove[i].y = _piecePosition[i].x - myPivot.x + myPivot.y;
                    break;
                default:
                    break;
            }
        }

        //intento rotarla con la rotacion normal
        if (isCurrent)
        {
            if (MoveUpdateCurrentPieceTo(board, posToMove, insideBoard))
            {
                for (int i = 0; i < posToMove.Length; i++)
                {
                    _piecesInBoard[posToMove[i]].RotateCurrentPosition(direction);
                }
                pieceMoved = true;
                return posToMove;
            }
        }
        else
        {
            if (MoveFromTo(board, _piecePosition, posToMove, insideBoard))
            {
                for (int i = 0; i < posToMove.Length; i++)
                {
                    _piecesInBoard[posToMove[i]].RotateCurrentPosition(direction);
                }
                pieceMoved = true;
                return posToMove;
            }
        }

        //obtengo el array con los offset desde el pivote
        Vector3Int[] offsetPositons = _piecesInBoard[myPivot].PositionsToTryInDirection(direction);

        if (offsetPositons == null)
        {
            return _piecePosition;
        }

        if (offsetPositons.Length == 0)
        {
            return _piecePosition;
        }

        //intento rotarlo con los diferentes offsets aplicados
        for (int i = 0; i < offsetPositons.Length; i++)
        {
            //aplico un offset
            for (int c = 0; c < posToMove.Length; c++)
            {
                posToMove[c].x += offsetPositons[i].x;
                posToMove[c].y += offsetPositons[i].y;
            }

            //intento hacer la rotacion
            if (isCurrent)
            {
                if (MoveUpdateCurrentPieceTo(board, posToMove, insideBoard))
                {
                    for (int c = 0; c < posToMove.Length; c++)
                    {
                        _piecesInBoard[posToMove[c]].RotateCurrentPosition(direction);
                    }
                    pieceMoved = true;
                    return posToMove;
                }
            }
            else
            {
                if (MoveFromTo(board, _piecePosition, posToMove, insideBoard))
                {
                    for (int c = 0; c < posToMove.Length; c++)
                    {
                        _piecesInBoard[posToMove[c]].RotateCurrentPosition(direction);
                    }
                    pieceMoved = true;
                    return posToMove;
                }
            }

            //remuevo el offset
            for (int c = 0; c < posToMove.Length; c++)
            {
                posToMove[c].x -= offsetPositons[i].x;
                posToMove[c].y -= offsetPositons[i].y;
            }
        }

        return _piecePosition;
    }

    protected override void CheckForMatch(Board board)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame(Board board)
    {
        SetStartPieceAndUpdate(board);
    }

    public void NextPiece(Board board)
    {
        SetPieceNoCurrent(_currentPiece);
        SetStartPieceAndUpdate(board);
    }

    public void UpdateShadow(Board board)
    {
        _shadowPiece.SetShadow(board, _currentPiece, _piecesInBoard);
    }

    public void DebugPiece()
    {
        TetraPiece outTemp;

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            if (_piecesInBoard.TryGetValue(_currentPiece[i], out outTemp))
            {
                Debug.Log("piece id: " + outTemp.GetPieceId + " position: " + outTemp.CurrentPosition);
            }
        }
    }
}
