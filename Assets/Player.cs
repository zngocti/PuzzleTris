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
public abstract class Player : MonoBehaviour
{
    [SerializeField]
    protected string _name;

    [SerializeField]
    protected Board _board;

    protected TetraManager _piecesManager;

    public PieceType _pieceType;

    private void Awake()
    {
        if (_board == null)
        {
            _board = GetComponent<Board>();
        }
    }

    protected virtual void Start()
    {
        _piecesManager.SetPreviewPositions(_board);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
