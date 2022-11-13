using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    protected bool _isPivot = false;
    protected bool _isCurrentPiece = false;
    protected bool _isInUse = false;

    public bool IsPivot { get => _isPivot; }
    public bool IsCurrentPiece { get => _isCurrentPiece; }
    public bool IsInUse { get => _isInUse; }
}
