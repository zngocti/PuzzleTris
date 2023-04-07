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
    [SerializeField]
    private PiecesManager<Piece> _piecesManager;

    public PieceType _pieceType;

    private void Awake()
    {
        _board = GetComponent<Board>();
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
