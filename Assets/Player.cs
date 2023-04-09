using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    Tetromino,
    Pyramid,
    Hex,
    Pipe,
    Column,
    Ice,
    Uno
}

[RequireComponent(typeof(Board))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private Board _board;
    ///https://forum.unity.com/threads/put-instance-of-generic-class-in-inspector.1405954/
    [SerializeField]
    public PiecesManager set;
    [SerializeField]
    private PiecesManager<Piece> _piecesManager;

    public PieceType _pieceType;

    private void Awake()
    {
        if (_board == null)
        {
            _board = GetComponent<Board>();
        }
    }

    private void Start()
    {
        _piecesManager.SetPreviewPositions(_board);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
