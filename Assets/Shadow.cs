using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Shadow<T> : MonoBehaviour where T : Piece
{
    [SerializeField]
    protected Tile _shadowTile;

    protected Vector3Int[] _shadowPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveShadow(Board board)
    {
        if (_shadowPosition == null)
        {
            return;
        }

        for (int i = 0; i < _shadowPosition.Length; i++)
        {
            board.RemoveTileIfInPosition(_shadowPosition[i], _shadowTile);
        }
    }

    public abstract void SetShadow(Board board, Vector3Int[] piece, Dictionary<Vector3Int, T> piecesInBoard);
}
