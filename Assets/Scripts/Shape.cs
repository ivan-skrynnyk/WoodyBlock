using Assets.Scripts.ShapesData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject sprite;
    public float shapeSelectedScale = 1.2f;
    public Vector2 offset = new Vector2(0f, 100f);

    private float squareOffset = 1.5f;
    private float squareGap = 5f;

    private Vector3 _shapeStartScale;
    private RectTransform _rectTransform;
    private bool _isShapeDraggable;
    private Canvas _canvas;
    private Vector3 _startPosition;

    private List<GameObject> currentShapeSquares = new List<GameObject>();

    private bool[,] _shapeData;
    public int ShapeId { get; set; }
    public bool IsShapeDraggable => _isShapeDraggable;
    public bool[,] ShapeData => _shapeData;

    public void Awake()
    {
        _shapeStartScale = GetComponent<RectTransform>().localScale;
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _startPosition = GetComponent<RectTransform>().localPosition;
        _isShapeDraggable = true;
    }

    void Start()
    {
    }

    public int GetSquareCount()
    {
        int squareCount = 0;
        for (int i = 0; i < _shapeData.GetLength(0); i++)
        {
            for(int j = 0; j < _shapeData.GetLength(1); j++)
            {
                if (_shapeData[i, j])
                    squareCount++;
            }
        }

        return squareCount;
    }

    public void ReqeuestNewShape(bool[,] shapeData, int id)
    {
        _rectTransform.localPosition = _startPosition;
        currentShapeSquares.Clear();
        CreateShapeFromShapeData(shapeData, id);
    }

    public void CreateShapeFromShapeData(bool[,] shapeData, int id)
    {
        _shapeData = shapeData;
        ShapeId = id;
        _isShapeDraggable = true;

        for (int i = 0; i < shapeData.GetLength(0); i++)
        {
            for (int j = 0; j < shapeData.GetLength(1); j++)
            {
                if (shapeData[i, j])
                {
                    var blockObject = Instantiate(sprite);
                    blockObject.transform.SetParent(gameObject.transform);
                    blockObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                    var rectTransform = blockObject.GetComponent<RectTransform>();

                    //TODO provide better position logic. Need fix centering of final shape

                    var xOffset = (rectTransform.transform.localScale.x * rectTransform.rect.width + squareOffset) * j + (j * squareGap);
                    var yOffset = (rectTransform.transform.localScale.y * rectTransform.rect.height + squareOffset) * i + (i * squareGap);

                    rectTransform.anchoredPosition = new Vector2(gameObject.transform.position.x + xOffset, gameObject.transform.position.y - yOffset);
                    rectTransform.localPosition = new Vector2(gameObject.transform.position.x + xOffset, gameObject.transform.position.y - yOffset);

                    currentShapeSquares.Add(blockObject);
                }
            }
        }
    }

    public bool IsOnStartPosition()
    {
        return _rectTransform.localPosition == _startPosition;
    }

    public bool IsAllSquareIsActive()
    {
        foreach(var square in currentShapeSquares)
        {
            if (!square.gameObject.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    public void DestroyShapeSquares()
    {
        _isShapeDraggable = false;
        foreach (var square in currentShapeSquares)
        {
            if (square == null)
                return;

            square.GetComponent<BoxCollider2D>().enabled = false;
            square.SetActive(false);

            Destroy(square.gameObject);
        }
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        GetComponent<RectTransform>().localScale = new Vector3(shapeSelectedScale, shapeSelectedScale, shapeSelectedScale);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchorMin = new Vector2(0f, 0f);
        _rectTransform.anchorMax = new Vector2(0f, 0f);

        _rectTransform.pivot = new Vector2(0f, 0f);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out Vector2 position);

        _rectTransform.localPosition = position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvents.CheckIfShapeCanBePlaced(_shapeData, ShapeId);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void MoveShapeToStartPosition()
    {
        _rectTransform.localScale = _shapeStartScale;
        _rectTransform.localPosition = new Vector3(_startPosition.x -50f, _startPosition.y);
    }

    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
    }

    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
    }
}
