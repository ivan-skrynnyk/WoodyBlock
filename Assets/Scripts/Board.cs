using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int rows = 0;
    public int columns = 0;
    public GameObject gridSquare;
    public float squareScale = 0.5f;
    public float squareOffset = 0.0f;
    public float squareGap = 0.1f;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);

    private GameObject[,] _gridSquares;
    private int _shapesLeft;
    private Vector2 _offset = new Vector2(0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        _gridSquares = new GameObject[rows, columns];
        _shapesLeft = shapeStorage.ShapeList.Count;
        SpawnGridSquare();
    }

    private void Update()
    {
    }

    private void SpawnGridSquare()
    {
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                _gridSquares[i, j] = Instantiate(gridSquare) as GameObject;
                _gridSquares[i, j].transform.SetParent(gameObject.transform);
                _gridSquares[i, j].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[i, j].GetComponent<GridSquare>().SetCoordinate(i, j);

                index++;
            }
        }

        CorrectPosition();
    }

    private void CorrectPosition()
    {
        int columnNumber = 0;
        int rowNumber = 0;
        bool moveToNewRow = false;
        Vector2 square_gap = new Vector2(0.0f, 0.0f);
        var squareRect = _gridSquares[0, 0].GetComponent<RectTransform>();

        _offset.x = squareRect.transform.localScale.x * squareRect.rect.width + squareOffset;
        _offset.y = squareRect.transform.localScale.y * squareRect.rect.height + squareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if (columnNumber + 1 > columns)
            {
                columnNumber = 0;
                square_gap.x = 0;
                rowNumber++;
                moveToNewRow = false;
            }

            var xOffset = _offset.x * columnNumber + (square_gap.x * squareGap);
            var yOffset = _offset.y * rowNumber + (square_gap.y * squareGap);

            if (columnNumber > 0)
            {
                square_gap.x++;
                xOffset += squareGap;
            }

            if (rowNumber > 0 && !moveToNewRow)
            {
                moveToNewRow = true;
                square_gap.y++;
                yOffset += squareGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + xOffset, startPosition.y - yOffset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + xOffset, startPosition.y - yOffset, 0f);

            columnNumber++;
        }
    }

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    public void CheckIfShapeCanBePlaced(bool[,] shapeData, int id)
    {
        int x = 0;
        int y = 0;
        bool isHovered = false;

        int squaresCount = shapeStorage.GetSquaresCount(id);
        if (squaresCount == GetHoveredSquares())
        {
            for (int i = 0; i < _gridSquares.GetLength(0); i++)
            {
                for (int j = 0; j < _gridSquares.GetLength(1); j++)
                {
                    var gridSquare = _gridSquares[i, j].GetComponent<GridSquare>();
                    if (gridSquare == null)
                        break;

                    if (gridSquare.IsHovered)
                    {
                        isHovered = true;
                        x = i;
                        y = j;
                        break;
                    }
                }
                if (isHovered) break;
            }
        }

        if (!isHovered)
        {
            GameEvents.MoveShapeToStartPosition();
            return;
        }

        CorrectHoveredPositions(ref y, shapeData);

        var canShapeBePlaced = CanShapeBePlaced(x, y, shapeData);

        if (canShapeBePlaced)
        {
            PlaceShapeOnBoard(x, y, shapeData);
            CheckForFullRow(x, shapeData);
            CheckForFullColumn(y, shapeData);
            GameEvents.AddScore(squaresCount);

            shapeStorage.GetSelectedShape(id).DestroyShapeSquares();

            if (--_shapesLeft == 0)
            {
                GameEvents.RequestNewShapes();
                _shapesLeft = shapeStorage.ShapeList.Count;
            }

            var availableShapes = shapeStorage.ShapeList.Where(s => s.IsShapeDraggable);
            bool newMoveIsAvailbale = false;
            foreach (var shape in availableShapes)
            {
                if (CheckIfMovesAreAvaivable(shape.ShapeData))
                {
                    newMoveIsAvailbale = true;
                    break;
                }
            }

            if (!newMoveIsAvailbale)
            {
                GameEvents.GameOver();
            }

        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }

    }

    private int GetHoveredSquares()
    {
        int count = 0;
        for (int i = 0; i < _gridSquares.GetLength(0); i++)
        {
            for (int j = 0; j < _gridSquares.GetLength(1); j++)
            {
                var gridSquare = _gridSquares[i, j].GetComponent<GridSquare>();
                if (gridSquare == null)
                    break;

                if (gridSquare.IsHovered)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool CanShapeBePlaced(int x, int y, bool[,] shapeData)
    {
        bool canShapeBePlaced = true;

        int shapeDataRowPosition = 0;
        int shapeDataColumnPosition = 0;

        for (int i = x; i < x + shapeData.GetLength(0); i++)
        {
            for (int j = y; j < y + shapeData.GetLength(1); j++)
            {
                if(j == 10)
                {

                }
                if(i >= _gridSquares.GetLength(0) || j >= _gridSquares.GetLength(1))
                {
                    Debug.Log($"i = {i}, j = {j}");
                }
                var gridSquare = _gridSquares[i, j].GetComponent<GridSquare>();
                if (gridSquare == null)
                    break;

                if (gridSquare.IsActive && shapeData[shapeDataRowPosition, shapeDataColumnPosition])
                {
                    canShapeBePlaced = false;
                    break;
                }
                shapeDataColumnPosition++;
            }
            shapeDataColumnPosition = 0;
            shapeDataRowPosition++;
            if (!canShapeBePlaced) break;
        }

        return canShapeBePlaced;
    }

    private void PlaceShapeOnBoard(int x, int y, bool[,] shapeData)
    {
        int shapeDataRowPosition = 0;
        int shapeDataColumnPosition = 0;

        for (int i = x; i < x + shapeData.GetLength(0); i++)
        {
            shapeDataColumnPosition = 0;
            for (int j = y; j < y + shapeData.GetLength(1); j++)
            {
                var gridSquare = _gridSquares[i, j].GetComponent<GridSquare>();
                if (gridSquare == null)
                    break;

                if (shapeData[shapeDataRowPosition, shapeDataColumnPosition])
                {
                    gridSquare.ActivateSquare();
                }
                shapeDataColumnPosition++;
            }
            shapeDataColumnPosition = 0;
            shapeDataRowPosition++;
        }
    }

    private void CheckForFullRow(int x, bool[,] shapeData)
    {
        List<int> rowIndexes = new List<int>();
        int newXPosition = x + shapeData.GetLength(0) - 1;
        while (newXPosition >= x)
        {
            if (CheckForFullRowInternal(newXPosition, _gridSquares))
            {
                rowIndexes.Add(newXPosition);
            }

            newXPosition--;
        }

        for (int i = 0; i < rowIndexes.Count; i++)
        {
            for (int j = 0; j < _gridSquares.GetLength(1); j++)
            {
                var gridSquare = _gridSquares[rowIndexes[i], j].GetComponent<GridSquare>();
                if (gridSquare == null)
                    break;

                gridSquare.DeactivateSquare();
            }
        }

        GameEvents.AddScore(rowIndexes.Count * 10);
    }

    private bool CheckForFullRowInternal(int x, GameObject[,] boardData)
    {
        for (int i = 0; i < boardData.GetLength(1); i++)
        {
            if (!boardData[x, i].GetComponent<GridSquare>().IsActive)
                return false;
        }

        return true;
    }

    private void CheckForFullColumn(int y, bool[,] shapeData)
    {
        List<int> columnIndexes = new List<int>();
        int newYPosition = y + shapeData.GetLength(1) - 1;
        while (newYPosition >= y)
        {
            if (CheckForFullColumnInternal(newYPosition, _gridSquares))
            {
                columnIndexes.Add(newYPosition);
            }

            newYPosition--;
        }

        for (int i = 0; i < columnIndexes.Count; i++)
        {
            for (int j = 0; j < _gridSquares.GetLength(0); j++)
            {
                var gridSquare = _gridSquares[j, columnIndexes[i]].GetComponent<GridSquare>();
                if (gridSquare == null)
                    break;

                gridSquare.DeactivateSquare();
            }
        }

        GameEvents.AddScore(columnIndexes.Count * 10);
    }

    private bool CheckForFullColumnInternal(int y, GameObject[,] boardData)
    {
        for (int i = 0; i < boardData.GetLength(0); i++)
        {
            if (!boardData[i, y].GetComponent<GridSquare>().IsActive)
                return false;
        }

        return true;
    }

    private void CorrectHoveredPositions(ref int y, bool[,] shapeData)
    {
        if (shapeData[0, 0])
            return;

        for (int i = 0; i < shapeData.GetLength(1); i++)
        {
            if (!shapeData[0, i])
                y--;
        }
    }

    private bool CheckIfMovesAreAvaivable(bool[,] shapeData)
    {
        for (int i = 0; i < _gridSquares.GetLength(0) - shapeData.GetLength(1); i++)
        {
            for (int j = 0; j < _gridSquares.GetLength(1) - shapeData.GetLength(0); j++)
            {
                if (CanShapeBePlaced(i, j, shapeData))
                    return true;
            }
        }

        return false;
    }
}
