using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraManager : PiecesManager<TetraPiece>
{
    protected override void GeneratePool()
    {
        if (_myPool.Count > 0)
        {
            return;
        }

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject go = new GameObject();
            _myPool.Add(go.AddComponent<TetraPiece>());
        }
    }

    protected override void GenerateRandomPieces()
    {
        throw new System.NotImplementedException();
    }

    protected override void MovePieceDown(Board board)
    {
        throw new System.NotImplementedException();
    }

    protected override void MovePieceSideways(Board board, bool moveToRight)
    {
        throw new System.NotImplementedException();
    }

    protected override void RotatePiece(Board board)
    {
        throw new System.NotImplementedException();
    }

    protected override void CheckForMatch(Board board)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
