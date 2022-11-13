using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasPieceType<T>
{
    T PieceType { get; }

    void SetPiece(T newType, bool isPivot = false);
}
