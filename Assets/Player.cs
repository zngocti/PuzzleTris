using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Board))]
[RequireComponent(typeof(PiecesManager))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private string _name;

    public Board _board;
    public PiecesManager _piecesManager;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _piecesManager = GetComponent<PiecesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
