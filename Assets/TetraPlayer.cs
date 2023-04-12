using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPlayer : Player
{
    [SerializeField]
    TetraManager _tetraManager;

    bool _gameStarted = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I ) && !_gameStarted)
        {
            _gameStarted = true;
            _tetraManager.StartGame(_board);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _tetraManager.MovePieceToDirection(_board, Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _tetraManager.MovePieceToDirection(_board, Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _tetraManager.MovePieceToDirection(_board, Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _tetraManager.RotatePiece(_board, Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _tetraManager.RotatePiece(_board, Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _tetraManager.NextPiece(_board);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _tetraManager.HoldPiece(_board);
        }
    }

    private void Awake()
    {
        _piecesManager = _tetraManager;
    }
}
