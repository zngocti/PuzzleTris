using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasPieceType<T> where T : System.Enum
{
    T PieceType { get; }

    public void SetPiece(T newType, bool isPivot = false);
}
