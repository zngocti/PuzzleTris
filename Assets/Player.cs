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
[RequireComponent(typeof(PiecesManager))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private string _name;

    private Board _board;
    private PiecesManager _piecesManager;

    public PieceType _pieceType;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _piecesManager = GetComponent<PiecesManager>();
    }

    private void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
