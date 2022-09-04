using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private int _width = 10;
    [SerializeField]
    private int _height = 20;

    public GameObject _tilePrefab;

    public GameObject[,] backgroundTiles;

    private void Awake()
    {
        _height += 10;

        //columns rows
        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                backgroundTiles[i,c] = Instantiate(_tilePrefab);
            }
        }
    }
}
