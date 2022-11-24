using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PiecesManager<T> : MonoBehaviour where T : Piece
{
    [SerializeField]
    [Min(10)]
    protected int _poolSize = 300;

    protected Vector3Int[] _currentPiece;
    protected Vector3Int[] _savedPiece;

    protected List<Piece> _myPool;

    protected Dictionary<Vector3Int, Piece> _piecesInBoard;

    protected List<System.Enum> _nextPieces;

    [System.Serializable]
    public struct PieceInBag
    {
        public T _pieceType;
        [Min(1)]
        public int _quantityInASpawnBag;
    }

    [SerializeField]
    PieceInBag[] myArray;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract void GeneratePool();
    protected abstract void GenerateRandomPieces();
    protected Piece GetAvailablePiece()
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

    protected abstract bool CanMoveDown(Board board);
    protected abstract void MovePieceDown(Board board);
    protected abstract void MovePieceSideways(Board board, bool moveToRight);
    protected abstract void RotatePiece(Board board);
    
    protected abstract void CheckForMatch(Board board);
}
