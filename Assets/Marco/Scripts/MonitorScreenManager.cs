using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorScreenManager : MonoBehaviour
{

    [SerializeField]
    RectTransform gameCanvas;

    [SerializeField]
    RectTransform rootCanvas;

    [SerializeField]
    GameObject gameOverPanel;
    [SerializeField]
    TextMeshPro endPointsText;

    bool isGameCanvasInFrame = true;
    bool moving = false;
  
    float maxRightLength;

    [SerializeField]
    float timeToMove = 2;
    float currentTIme;

    // Start is called before the first frame update
    void Start()
    {
        maxRightLength = (float) (gameCanvas.rect.width * 1.5);

        Vector2 position = rootCanvas.anchoredPosition;
        position.x += maxRightLength;
        rootCanvas.anchoredPosition = position;

        gameOverPanel.SetActive(false);

    }

    private void OnGUI()
    {
        if (moving)
        {
            currentTIme += Time.deltaTime;

            if (isGameCanvasInFrame)
            {
                Vector2 position = gameCanvas.anchoredPosition;
                position.x = Mathf.Lerp(0, -maxRightLength, currentTIme / timeToMove);
                gameCanvas.anchoredPosition = position;

                position = rootCanvas.anchoredPosition;
                position.x = Mathf.Lerp(maxRightLength, 0, currentTIme / timeToMove);
                rootCanvas.anchoredPosition = position;
            }
            else
            {
                Vector2 position = rootCanvas.anchoredPosition;
                position.x = Mathf.Lerp(0, maxRightLength, currentTIme / timeToMove);
                rootCanvas.anchoredPosition = position;

                position = gameCanvas.anchoredPosition;
                position.x = Mathf.Lerp(-maxRightLength, 0, currentTIme / timeToMove);
                gameCanvas.anchoredPosition = position;
            }

            if (currentTIme >= timeToMove)
            {
                moving = false;
                isGameCanvasInFrame = !isGameCanvasInFrame;
            }

        }
    }


    public void swapScreen()
    {
        if (!moving)
        {
            currentTIme = 0;
            moving = true;
        }
    }

    public void gameOver(float points)
    {
        if (isGameCanvasInFrame)
        {
            swapScreen();
        }

        gameOverPanel.SetActive(true);

        endPointsText.text = "Points: " + (int) points;
    }
}
