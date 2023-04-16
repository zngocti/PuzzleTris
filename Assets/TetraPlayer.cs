using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPlayer : Player
{
    [SerializeField]
    TetraManager _tetraManager;

    bool _gameStarted = false;

    float _gameSpeed = 1.5f;
    float _timerSpeed = 0;

    float _keySpeedMax = 0.20f;
    float _keySpeed = 0.20f;
    float _keySpedRemove = 0.08f;
    float _keySpeedMin = 0.08f;
    float _keySpeedCurrent = 0;

    KeyCode _lastKey = KeyCode.None;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I ) && !_gameStarted)
        {
            _gameStarted = true;
            _tetraManager.StartGame(_board);
            _tetraManager.UpdateShadow(_board);
        }

        if (_gameStarted)
        {
            if (_timerSpeed < _gameSpeed)
            {
                _timerSpeed += Time.deltaTime;
            }
            else
            {
                _timerSpeed = 0;
                if (!_tetraManager.MovePieceToDirection(_board, Direction.Down))
                {
                    _tetraManager.NextPiece(_board);
                    _tetraManager.UpdateShadow(_board);
                }
            }

            //input
            bool temp;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (MoveWithKeySpeed(KeyCode.RightArrow, Direction.Right, out temp))
                {
                    if (temp)
                    {
                        _tetraManager.UpdateShadow(_board);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (MoveWithKeySpeed(KeyCode.LeftArrow, Direction.Left, out temp))
                {
                    if (temp)
                    {
                        _tetraManager.UpdateShadow(_board);
                    }
                }               
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                //MoveWithKeySpeedDown();
                if (MoveWithKeySpeed(KeyCode.DownArrow, Direction.Down, out temp))
                {
                    if (!temp)
                    {
                        _tetraManager.NextPiece(_board);
                        _tetraManager.UpdateShadow(_board);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _tetraManager.RotatePiece(_board, out temp, Direction.Left);
                if (temp)
                {
                    _tetraManager.UpdateShadow(_board);
                }
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                _tetraManager.RotatePiece(_board, out temp, Direction.Right);
                if (temp)
                {
                    _tetraManager.UpdateShadow(_board);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //MoveWithKeySpeedSpace();

                while (_tetraManager.MovePieceToDirection(_board, Direction.Down))
                {

                }
                _tetraManager.NextPiece(_board);
                _tetraManager.UpdateShadow(_board);
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _tetraManager.HoldPiece(_board);
                _tetraManager.UpdateShadow(_board);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _tetraManager.DebugPiece();
            }

            if (Input.GetKeyUp(_lastKey))
            {
                _lastKey = KeyCode.None;
            }
        }
    }

    private void Awake()
    {
        _piecesManager = _tetraManager;
    }

    //el bool que devuelve el metodo es para saber si se cumplio el tiempo para tomar la key
    //si el metodo devuelve true entocnes el bool que hace out pieceMoved es el del MovePieceToDirection
    private bool MoveWithKeySpeed(KeyCode myKey, Direction myDirection, out bool pieceMoved)
    {
        if (_lastKey == myKey)
        {
            if (_keySpeedCurrent >= _keySpeed)
            {
                pieceMoved = _tetraManager.MovePieceToDirection(_board, myDirection);
                /*
                if (_tetraManager.MovePieceToDirection(_board, myDirection))
                {
                    _tetraManager.UpdateShadow(_board);
                }*/
                _keySpeedCurrent = 0;
                if (_keySpeed > _keySpeedMin)
                {
                    _keySpeed -= _keySpedRemove;
                }

                return true;
            }
            else
            {
                _keySpeedCurrent += Time.deltaTime;
                pieceMoved = false;
                return false;
            }
        }
        else
        {
            _keySpeedCurrent = 0;
            _keySpeed = _keySpeedMax;
            _lastKey = myKey;
            pieceMoved = _tetraManager.MovePieceToDirection(_board, myDirection);
            /*
            if (_tetraManager.MovePieceToDirection(_board, myDirection))
            {
                _tetraManager.UpdateShadow(_board);
            }*/

            return true;
        }
    }

    /*
    private void MoveWithKeySpeedDown()
    {
        if (_lastKey == KeyCode.DownArrow)
        {
            if (_keySpeedCurrent >= _keySpeed)
            {
                _keySpeedCurrent = 0;
                if (_keySpeed > _keySpeedMin)
                {
                    _keySpeed -= _keySpedRemove;
                }
                if (!_tetraManager.MovePieceToDirection(_board, Direction.Down))
                {
                    _tetraManager.NextPiece(_board);
                }
            }
            else
            {
                _keySpeedCurrent += Time.deltaTime;
            }
        }
        else
        {
            _keySpeedCurrent = 0;
            _keySpeed = _keySpeedMax;
            _lastKey = KeyCode.DownArrow;
            if (!_tetraManager.MovePieceToDirection(_board, Direction.Down))
            {
                _tetraManager.NextPiece(_board);
            }
        }
    }*/

    /*
    private void MoveWithKeySpeedSpace()
    {
        if (_lastKey == KeyCode.Space)
        {
            if (_keySpeedCurrent >= _keySpeed)
            {
                _keySpeedCurrent = 0;
                if (_keySpeed > _keySpeedMin)
                {
                    _keySpeed -= _keySpedRemove;
                }

                while (_tetraManager.MovePieceToDirection(_board, Direction.Down))
                {

                }
                _tetraManager.NextPiece(_board);
            }
            else
            {
                _keySpeedCurrent += Time.deltaTime;
            }
        }
        else
        {
            _keySpeedCurrent = 0;
            _keySpeed = _keySpeedMax;
            _lastKey = KeyCode.Space;
            while (_tetraManager.MovePieceToDirection(_board, Direction.Down))
            {

            }
            _tetraManager.NextPiece(_board);
        }
    }*/
}