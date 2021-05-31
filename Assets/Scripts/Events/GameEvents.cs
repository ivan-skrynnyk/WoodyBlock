using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<bool[,], int> CheckIfShapeCanBePlaced;

    public static Action MoveShapeToStartPosition;

    public static Action RequestNewShapes;

    public static Action<int> AddScore;

    public static Action GameOver;
}
