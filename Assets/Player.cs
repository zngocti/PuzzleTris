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
    private PiecesManager _piecesManager;

    public PieceType _pieceType;

    private void Awake()
    {
        _board = GetComponent<Board>();
    }

    private void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
