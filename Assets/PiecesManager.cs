using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Right,
    Down,
    Left
}

public abstract class PiecesManager<T> : MonoBehaviour where T : Piece
{
    [SerializeField]
    [Min(10)]
    protected int _poolSize = 300;

    protected Vector3Int[] _currentPiece;
    protected Vector3Int[] _heldPiece;

    protected Vector3Int[][] _previewPieces;

    protected List<T> _myPool = new List<T>();

    protected Dictionary<Vector3Int, T> _piecesInBoard = new Dictionary<Vector3Int, T>();

    protected List<T> _nextPieces = new List<T>();

    private int _currentBagPiece = 0;

    [System.Serializable]
    public struct PieceInBag
    {
        public T _pieceType;
        [Min(1)]
        public int _amountInBag;
    }

    [SerializeField]
    protected PieceInBag[] _possiblePieces;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GeneratePool();
        GenerateBagOfPieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPreviewPositions(Board board)
    {
        _previewPieces = new Vector3Int[board.PreviewPositions.Length][];
    }

    protected void GeneratePool()
    {
        if (_myPool.Count > 0)
        {
            return;
        }

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject go = new GameObject();
            _myPool.Add(go.AddComponent<T>());
        }
    }

    //usar una vez
    protected void GenerateBagOfPieces()
    {
        _nextPieces.Clear();

        for (int i = 0; i < _possiblePieces.Length; i++)
        {
            for (int c = 0; c < _possiblePieces[i]._amountInBag; c++)
            {
                _nextPieces.Add(_possiblePieces[i]._pieceType);
            }
        }

        Utilities.Shuffle<T>(_nextPieces);

        _currentBagPiece = 0;
    }

    protected T GetEmptyPiece()
    {
        for (int i = 0; i < _myPool.Count; i++)
        {
            if (!_myPool[i].IsInUse)
            {
                return _myPool[i];
            }
        }

        return null;
    }

    public void HoldPiece(Board board)
    {
        Vector3Int[] temp;

        if (_piecesInBoard.ContainsKey(board.HeldPiecePosition))
        {
            Vector3Int[] oldCurrent = new Vector3Int[_currentPiece.Length];

            for (int i = 0; i < oldCurrent.Length; i++)
            {
                oldCurrent[i] = _currentPiece[i];
            }

            temp = MovePieceToPivotPosition(board, _heldPiece, board.TemporalPosition);
            UpdateCurrentPiece(temp);

            temp = MovePieceToPivotPosition(board, oldCurrent, board.HeldPiecePosition);
            SetPieceNoCurrent(temp);
            UpdateHeldPiece(temp);

            temp = MovePieceToPivotPosition(board, _currentPiece, board.StartPosition);
            UpdateCurrentPiece(temp);

            return;
        }

        temp = MovePieceToPivotPosition(board, _currentPiece, board.HeldPiecePosition);
        SetPieceNoCurrent(temp);
        UpdateHeldPiece(temp);

        //hacer que la current sea la siguiente de las preview
        SetStartPieceAndUpdate(board);
    }

    //esto regresa las posiciones finales
    protected Vector3Int[] MovePieceToPivotPosition(Board board, Vector3Int[] fromPos , Vector3Int pivotPosition)
    {
        int pivot = -1;

        for (int i = 0; i < fromPos.Length; i++)
        {
            if (_piecesInBoard.GetValueOrDefault(fromPos[i]).IsPivot)
            {
                pivot = i;
                break;
            }
        }

        if (pivot < 0)
        {
            Debug.Log("No pivot");
            //si no hay pivote
            return null;
            //tal vez hacer que el pivote sea igualmente el 0, asumiendo que hay al menos una pieza?
        }
        //generar posiciones para ir a la posicion deseada
        Vector3Int[] temporalPosition = new Vector3Int[fromPos.Length];

        for (int i = 0; i < fromPos.Length; i++)
        {
            //agarro las coordenadas del pivote, les resto la de la parte de la pieza para ver el offset y a eso le sumo la nueva position para saber a donde va a ir
            temporalPosition[i].x = fromPos[i].x - fromPos[pivot].x + pivotPosition.x;
            temporalPosition[i].y = fromPos[i].y - fromPos[pivot].y + pivotPosition.y;
        }

        MoveFromTo(board, fromPos, temporalPosition, false);

        return temporalPosition;
    }

    protected void SetStartPieceAndUpdate(Board board)
    {
        if (_previewPieces[0] == null)
        {
            SetFirstPreview(board);
        }

        Vector3Int[] temp =  MovePieceToPivotPosition(board, _previewPieces[0], board.StartPosition);
        UpdateCurrentPiece(temp);


        for (int i = 1; i < _previewPieces.Length; i++)
        {
            temp = MovePieceToPivotPosition(board, _previewPieces[i], board.PreviewPositions[i - 1]);
            UpdatePreviewPiece(temp, i - 1);
        }

        //conseguir nueva pieza para la quinta preview
        UpdatePreviewPiece(GeneratePieceInBoard(board, board.PreviewPositions[board.PreviewPositions.Length - 1]), _previewPieces.Length - 1);
    }

    //una vez
    protected void SetFirstPreview(Board board)
    {
        for (int i = 0; i < _previewPieces.Length; i++)
        {
            UpdatePreviewPiece(GeneratePieceInBoard(board, board.PreviewPositions[i]), i);
        }
    }

    protected Vector3Int[] GeneratePieceInBoard(Board board, Vector3Int pivotPosition)
    {
        T temporalPiece = GetPieceFromBag();

        if (temporalPiece == null)
        {
            return null;
        }

        //consigo la forma de la pieza usandoi coordenadas desde el origen con 0,0 en el pivote
        Vector3Int[] offset = temporalPiece.GetOriginCoordinates();;

        T auxPiece;

        for (int i = 0; i < offset.Length; i++)
        {
            offset[i].x += pivotPosition.x;
            offset[i].y += pivotPosition.y;

            if (i < offset.Length - 1 && offset.Length > 1)
            {
                auxPiece = GetEmptyPiece();

                if (auxPiece == null)
                {
                    return null;
                }

                auxPiece.SetPiece(temporalPiece);

                if (offset[i].x == pivotPosition.x && offset[i].y == pivotPosition.y)
                {
                    auxPiece.SetAsPivot();
                }
                else
                {
                    auxPiece.SetAsPivot(false);
                }

                auxPiece.SetInUse();
                _piecesInBoard.Add(new Vector3Int(offset[i].x, offset[i].y), auxPiece);

                board.SetTile(new Vector3Int(offset[i].x, offset[i].y), auxPiece.Tile);
            }
            else
            {
                if (offset[i].x == pivotPosition.x && offset[i].y == pivotPosition.y)
                {
                    temporalPiece.SetAsPivot();
                }
                else
                {
                    temporalPiece.SetAsPivot(false);
                }

                temporalPiece.SetInUse();
                _piecesInBoard.Add(new Vector3Int(offset[i].x, offset[i].y), temporalPiece);

                board.SetTile(new Vector3Int(offset[i].x, offset[i].y), temporalPiece.Tile);
            }
        }

        return offset;
    }

    protected T GetPieceFromBag()
    {
        T temporalPiece = GetEmptyPiece();

        if (temporalPiece == null)
        {
            return null;
        }

        temporalPiece.SetPiece(_nextPieces[_currentBagPiece]);

        _currentBagPiece++;

        if (_currentBagPiece >= _nextPieces.Count)
        {
            _currentBagPiece = 0;
            _nextPieces.Shuffle();
        }

        return temporalPiece;
    }

    protected bool UpdateCurrentPiece(Vector3Int[] newCurrentPiece)
    {
        if (_currentPiece == null)
        {
            _currentPiece = new Vector3Int[newCurrentPiece.Length];
        }
        else if (_currentPiece.Length != newCurrentPiece.Length)
        {
            _currentPiece = new Vector3Int[newCurrentPiece.Length];
        }

        for (int i = 0; i < _currentPiece.Length; i++)
        {
            _currentPiece[i] = newCurrentPiece[i];
            _piecesInBoard.GetValueOrDefault(_currentPiece[i]).SetAsCurrentPiece();
        }

        return true;
    }

    protected void SetPieceNoCurrent(Vector3Int[] piece)
    {
        for (int i = 0; i < piece.Length; i++)
        {
            _piecesInBoard.GetValueOrDefault(piece[i]).SetAsCurrentPiece(false);
        }
    }

    protected bool UpdateHeldPiece(Vector3Int[] newHeldPiece)
    {
        if (_heldPiece == null)
        {
            _heldPiece = new Vector3Int[newHeldPiece.Length];
        }
        else if (_heldPiece.Length != newHeldPiece.Length)
        {
            _heldPiece = new Vector3Int[newHeldPiece.Length];
        }

        for (int i = 0; i < _heldPiece.Length; i++)
        {
            _heldPiece[i] = newHeldPiece[i];
        }

        return true;
    }

    protected bool UpdatePreviewPiece(Vector3Int[] newPreviewPiece, int spaceToUpdate)
    {
        if (_previewPieces.Length <= spaceToUpdate || spaceToUpdate < 0)
        {
            return false;
        }

        if (_previewPieces[spaceToUpdate] == null)
        {
            _previewPieces[spaceToUpdate] = new Vector3Int[newPreviewPiece.Length];
        }
        else if (_previewPieces[spaceToUpdate].Length != newPreviewPiece.Length)
        {
            _previewPieces[spaceToUpdate] = new Vector3Int[newPreviewPiece.Length];
        }

        for (int i = 0; i < _previewPieces[spaceToUpdate].Length; i++)
        {
            _previewPieces[spaceToUpdate][i] = newPreviewPiece[i];
        }

        return true;
    }

    protected abstract bool CanMoveFromTo(Board board, Vector3Int[] fromPos, Vector3Int[] toPos, bool insideBoard);
    protected abstract bool MoveFromTo(Board board, Vector3Int[] fromPos, Vector3Int[] toPos, bool insideBoard);
    protected abstract bool MoveUpdateCurrentPieceTo(Board board, Vector3Int[] toPos, bool insideBoard );
    public abstract bool MovePieceToDirection(Board board, Direction direction);
    public abstract bool RotatePiece(Board board, Direction direction);
    protected abstract void CheckForMatch(Board board);
}
