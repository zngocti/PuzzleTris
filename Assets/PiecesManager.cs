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
    protected Vector3Int[] _savedPiece;

    protected List<T> _myPool;

    protected Dictionary<Vector3Int, T> _piecesInBoard;

    protected List<T> _nextPieces;

    [System.Serializable]
    public struct PieceInBag
    {
        public T _pieceType;
        [Min(1)]
        public int _quantityInASpawnBag;
    }

    [SerializeField]
    protected PieceInBag[] _possiblePieces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    protected void GenerateBagOfPieces()
    {
        _nextPieces.Clear();

        for (int i = 0; i < _possiblePieces.Length; i++)
        {
            for (int c = 0; c < _possiblePieces[i]._quantityInASpawnBag; c++)
            {
                _nextPieces.Add(_possiblePieces[i]._pieceType);
            }
        }

        Utilities.Shuffle<T>(_nextPieces);
    }

    protected T GetAvailablePiece()
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

    protected abstract bool UpdateCurrentPiece(Vector3Int[] newCurrentPiece);
    protected abstract bool CanMoveFromTo(Board board, Vector3Int[] fromPos, Vector3Int[] toPos);
    protected abstract bool MoveFromTo(Board board, Vector3Int[] fromPos, Vector3Int[] toPos);
    protected abstract bool MoveCurrentPieceTo(Board board, Vector3Int[] toPos);
    protected abstract bool MovePieceToDirection(Board board, Direction direction);
    protected abstract bool CanRotatePiece(Board board, Direction direction);
    protected abstract bool RotatePiece(Board board, Direction direction);
    protected abstract void CheckForMatch(Board board);
}
