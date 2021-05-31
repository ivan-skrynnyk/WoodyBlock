using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    private int i_coordinate;
    private int j_coordinate;
    public Image activeImage;

    private bool isActive;
    private bool ishovered;

    public void SetCoordinate(int i, int j)
    {
        i_coordinate = i;
        j_coordinate = j;
    }

    public bool IsHovered => ishovered;
    public bool IsActive => isActive;

    public bool CanUseSquare()
    {
        return ishovered;
    }

    public void ActivateSquare()
    {
        activeImage.gameObject.SetActive(true);
        isActive = true;
    }

    public void DeactivateSquare()
    {
        activeImage.gameObject.SetActive(false);
        isActive = false;
    }

    void Start()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ishovered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ishovered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ishovered = true;
    }
}
