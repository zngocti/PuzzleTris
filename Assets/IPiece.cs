using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPiece<T>
{
    bool IsPivot { get; }
    bool IsCurrentPiece { get; }
    T PieceType { get; }

    void SetPiece(T newType, bool isPivot = false);
}
