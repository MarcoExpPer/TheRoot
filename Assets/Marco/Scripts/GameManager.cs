using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float points = 0;
    float gameTime = 0;

    bool isGameRunning = true;

    [SerializeField]
    TextMeshProUGUI textTimer;

    [SerializeField]
    TextMeshProUGUI textPoints;

    [SerializeField]
    RootSizeManager rootSizeManager;

    [SerializeField]
    MonitorScreenManager monitorScreenMan;

    [SerializeField]
    public Dictionary<int, float> levelToTimeBetweenCommands = new Dictionary<int, float>();


    public int level = 1;

    public GameManager()
    {
        levelToTimeBetweenCommands.Add(1, 20);
        levelToTimeBetweenCommands.Add(2, 15);
        levelToTimeBetweenCommands.Add(3, 15);
        levelToTimeBetweenCommands.Add(4, 13);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        

        textTimer.text = "00:00";
        textPoints.text = "00 00";

        rootSizeManager.OnRootChanged += hasGameEnded;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            gameTime += Time.deltaTime;
            increasePoints(level/2 * Time.deltaTime);
        }
    }

    private void OnGUI()
    {

        float minutes = Mathf.Floor(gameTime / 60);
        string min = minutes.ToString("00");


        float seconds = (gameTime % 60);
        string secs = seconds.ToString("00");
       
        textTimer.text = min + ":" + secs;
    }

    public void increasePoints(float newPoints)
    {
        points += newPoints;
        string pointsString = points.ToString("00 00");
        textPoints.text = pointsString;
    }


    public void hasGameEnded()
    {
        int deathCounter = 0;
        for(int i = 0; i < rootSizeManager.maxSlots; i++)
        {
            if(rootSizeManager.slots[i].slotData.state == ESlotState.BLOCKED ||
                rootSizeManager.slots[i].slotData.state == ESlotState.VIRUS)
            {
                deathCounter += 1;
            }
        }

        if(deathCounter == rootSizeManager.maxSlots)
        {
            isGameRunning = false;
            Debug.Log("Game finished");
            monitorScreenMan.gameOver(points);
        }
    }
}
