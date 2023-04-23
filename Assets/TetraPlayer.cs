using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetraPlayer : Player
{
    [SerializeField]
    TetraManager _tetraManager;

    bool _gameStarted = false;

    [SerializeField]
    [Min(0.01f)]
    float _gameSpeed = 1.5f;
    float _timerSpeed = 0;

    [SerializeField]
    [Min(0.01f)]
    float _keySpeedMax = 0.20f;
    float _keySpeed = 0.20f;
    [SerializeField]
    [Min(0)]
    float _keySpedRemove = 0.08f;
    [SerializeField]
    [Min(0.01f)]
    float _keySpeedMin = 0.08f;
    float _keySpeedCurrent = 0;

    [SerializeField]
    float _matchTimer = 0.3f;
    float _matchTimerCurrent = -1;

    bool _canHold = true;

    Direction _currentDirection = Direction.None;
    Direction _lastDirection = Direction.None;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerOn)
        {
            //bajar pieza segun velocidad del juego
            ApplyFallToPiece();

            ProcessMovement();

            //revisar si se esta efectuando un match
            ProcessMatch();
        }
    }

    PlayerControls _controls;

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }

    private void Awake()
    {
        _controls = new PlayerControls();

        _controls.Gameplay.Right.started += context => StartDirection(Direction.Right);
        _controls.Gameplay.Left.started += context => StartDirection(Direction.Left);
        _controls.Gameplay.SoftDrop.started += context => StartDirection(Direction.Down);

        _controls.Gameplay.Right.canceled += context => StopDirection(Direction.Right);
        _controls.Gameplay.Left.canceled += context => StopDirection(Direction.Left);
        _controls.Gameplay.SoftDrop.canceled += context => StopDirection(Direction.Down);

        _controls.Gameplay.RotateClockwise.started += context => DoRotation(Direction.Right);
        _controls.Gameplay.RotateCounterClockwise.started += context => DoRotation(Direction.Left);
        
        _controls.Gameplay.HardDrop.started += context => DoHardDrop();
        _controls.Gameplay.Hold.started += context => HoldPiece();

        _piecesManager = _tetraManager;
    }

    private void ProcessMovement()
    {
        if (Input.GetKey(KeyCode.I) && !_gameStarted)
        {
            _gameStarted = true;
            _tetraManager.StartGame(_board);
            _tetraManager.UpdateShadow(_board);
            return;
        }
        else if (!_gameStarted || _matchTimerCurrent >= 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _tetraManager.DebugPiece();
        }

        bool temp;

        if (_currentDirection == Direction.Right)
        {
            if (MoveWithKeySpeed(Direction.Right, out temp))
            {
                if (temp)
                {
                    _tetraManager.UpdateShadow(_board);
                }
            }
        }
        else if (_currentDirection == Direction.Left)
        {
            if (MoveWithKeySpeed(Direction.Left, out temp))
            {
                if (temp)
                {
                    _tetraManager.UpdateShadow(_board);
                }
            }
        }
        else if (_currentDirection == Direction.Down)
        {
            if (MoveWithKeySpeed(Direction.Down, out temp))
            {
                if (!temp)
                {
                    PieceLanded();
                }
            }
        }
    }

    private void DoRotation(Direction direction)
    {
        bool temp;

        _tetraManager.RotatePiece(_board, out temp, direction);

        if (temp)
        {
            _tetraManager.UpdateShadow(_board);
        }
    }

    private void DoHardDrop()
    {
        while (_tetraManager.MovePieceToDirection(_board, Direction.Down))
        {

        }

        PieceLanded();
    }

    private void HoldPiece()
    {
        if (_canHold)
        {
            _canHold = false;
            _tetraManager.HoldPiece(_board);
            _tetraManager.UpdateShadow(_board);
        }
    }

    private void StartMovingToDirection(Direction myDirection)
    {
        if (_tetraManager.MovePieceToDirection(_board, myDirection))
        {
            if (myDirection != Direction.Down)
            {
                _tetraManager.UpdateShadow(_board);
            }

            return;
        }

        if (myDirection == Direction.Down)
        {
            PieceLanded();
        }
    }

    //el bool que devuelve el metodo es para saber si se cumplio el tiempo para tomar la key
    //si el metodo devuelve true entonces el bool que hace out pieceMoved es el del MovePieceToDirection
    private bool MoveWithKeySpeed(Direction myDirection, out bool pieceMoved)
    {
        if (_keySpeedCurrent >= _keySpeed)
        {
            pieceMoved = _tetraManager.MovePieceToDirection(_board, myDirection);

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

    private void StartDirection(Direction direction)
    {
        if (direction == _currentDirection)
        {
            return;
        }

        StartMovingToDirection(direction);

        _currentDirection = direction;
        _keySpeedCurrent = 0;
        _keySpeed = _keySpeedMax;
    }

    private void StopDirection(Direction direction)
    {
        if (direction != _currentDirection)
        {
            return;
        }

        _currentDirection = Direction.None;
        _keySpeedCurrent = 0;
        _keySpeed = _keySpeedMax;
    }

    private void ApplyFallToPiece()
    {
        if (!_gameStarted || _matchTimerCurrent >= 0)
        {
            return;
        }

        if (_timerSpeed < _gameSpeed)
        {
            _timerSpeed += Time.deltaTime;
        }
        else
        {
            _timerSpeed = 0;
            if (!_tetraManager.MovePieceToDirection(_board, Direction.Down))
            {
                PieceLanded();
            }
        }
    }

    private void ProcessMatch()
    {
        if (_matchTimerCurrent < 0)
        {
            return;
        }

        _matchTimerCurrent += Time.deltaTime;

        if (_matchTimerCurrent >= _matchTimer)
        {
            _matchTimerCurrent = -1;
            _tetraManager.DestroyMarkedPieces(_board);
            _tetraManager.MoveAllPiecesDown(_board);
            _tetraManager.NextPiece(_board);
            _tetraManager.UpdateShadow(_board);
        }
    }

    private void PieceLanded()
    {
        _tetraManager.StopUsingCurrentPiece();
        _canHold = true;

        if (_tetraManager.CheckForMatch(_board))
        {
            //ganar puntos
            _matchTimerCurrent = 0;
            return;
        }
        
        if (_tetraManager.CheckLostCondition(_board))
        {
            _gameStarted = false;
            Debug.Log("Perdiste");
            return;
        }

        if (!_tetraManager.NextPiece(_board))
        {
            _gameStarted = false;
            Debug.Log("Perdiste");
            return;
        }

        _tetraManager.UpdateShadow(_board);
    }
}