using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TetraType
{
    J,
    L,
    S,
    Z,
    T,
    O,
    I
}

public class TetraPiece : Piece, IHasPieceType<TetraType>
{
    private TetraType _myType = TetraType.J;

    public TetraType PieceType { get => _myType; }

    public void SetPiece(TetraType newType, bool isPivot = false)
    {
        _isPivot = isPivot;
        _myType = newType;
    }
}
