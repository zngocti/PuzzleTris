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

public class TetraPiece : MonoBehaviour, IPiece<TetraType>
{
    private bool _isPivot = false;
    private bool _isCurrentPiece = false;
    private TetraType _myType = TetraType.J;

    public bool IsPivot { get => _isPivot; }
    public bool IsCurrentPiece { get => _isCurrentPiece; }

    public TetraType PieceType { get => _myType; }

    public void SetPiece(TetraType newType, bool isPivot = false)
    {
        _isPivot = isPivot;
        _myType = newType;
    }
}
