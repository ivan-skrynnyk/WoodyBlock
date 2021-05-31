using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPopUp;
    // Start is called before the first frame update
    void Start()
    {
        gameOverPopUp.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;

    }

    private void OnGameOver()
    {
        gameOverPopUp.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
