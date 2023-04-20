using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int _numberOfPlayers = 1;

    [SerializeField]
    Player[] _myPlayers;

    [SerializeField]
    bool _hasTimer = false;
    [SerializeField]
    [Min(10)]
    float _sprintTimerInSec = 120;

    float _currentGameTimer = 0;

    [SerializeField]
    [Min(0)]
    int _linesToWin = 40;

    int[] _counterKO;
    int[] _linesMade;
    int[] _score;

    [SerializeField]
    [Min(1)]
    float _timeTargetingPlayer = 5;

    [SerializeField]
    bool _increasingDifficulty = false;
    [SerializeField]
    [Min(1)]
    int _linesToNextLevel = 10;

    private void Awake()
    {
        if (_numberOfPlayers >= _myPlayers.Length)
        {
            _numberOfPlayers = _myPlayers.Length;
        }
        else
        {
            Player[] tempPlayers = new Player[_numberOfPlayers];

            for (int i = 0; i < _myPlayers.Length; i++)
            {
                if (i < _numberOfPlayers)
                {
                    tempPlayers[i] = _myPlayers[i];
                    tempPlayers[i].SetPlayerID(i);
                }
                else
                {
                    _myPlayers[i].TurnOffPlayer();
                }
            }
        }

        _counterKO = new int[_numberOfPlayers];
        _linesMade = new int[_numberOfPlayers];
        _score = new int[_numberOfPlayers];
    }

    // Start is called before the first frame update
    void Start()
    {
        //reajustar camara o posiciones de los tableros/jugadores
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
