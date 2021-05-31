using Assets.Scripts.ShapesData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<Shape> ShapeList;

    private List<bool[,]> shapesData => ShapesData.ShapesList;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < ShapeList.Count; i++)
        {
            var shape = ShapeList[i];
            var shapeIndex = Random.Range(0, shapesData.Count);
            shape.CreateShapeFromShapeData(shapesData[shapeIndex], i);
        }
    }

    public Shape GetSelectedShape(int id)
    {
        var shape = ShapeList.FirstOrDefault(s => s.ShapeId == id);

        return shape;
    }

    public int GetSquaresCount(int id)
    {
        var shape = ShapeList.FirstOrDefault(s => s.ShapeId == id);
        if (shape == null)
            return 0;

        return shape.GetSquareCount();
    }

    private void RequestNewShapes()
    {
        for (int i = 0; i < ShapeList.Count; i++)
        {
            var shape = ShapeList[i];
            var shapeIndex = Random.Range(0, shapesData.Count);
            shape.ReqeuestNewShape(shapesData[shapeIndex], i);
        }
    }

    private void OnEnable()
    {
        GameEvents.RequestNewShapes += RequestNewShapes;
    }

    private void OnDisable()
    {
        GameEvents.RequestNewShapes -= RequestNewShapes;
    }
}
