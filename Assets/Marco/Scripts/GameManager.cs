using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    float points = 0;
    float gameTime = 0;

    bool blackScreenEffectDone = false;
    bool blueScreenEffectDone = false;

    bool isGameRunning = true;

    public bool lookForSudo = false;
    public bool virusCanHappen = false;

    [SerializeField]
    TextMeshProUGUI textTimer;

    [SerializeField]
    TextMeshProUGUI textPoints;

    [SerializeField]
    TextMeshProUGUI textLevel;

    [SerializeField]
    RootSizeManager rootSizeManager;

    [SerializeField]
    MonitorScreenManager monitorScreenMan;

    [SerializeField]
    public eventManager eventMan;

    [SerializeField]
    Image blackPanel;

    [SerializeField]
    GameObject blueScreen1;

    [SerializeField] 
    GameObject blueScreen2;

    [SerializeField]
    public Dictionary<int, float> levelToTimeBetweenCommands = new Dictionary<int, float>();

    [SerializeField]
    NotificationInstructionController _notifController;

    [SerializeField]
    ExecuteDiscardTrayManager _traymanager;

    public int level = 1;

    public GameManager()
    {
        levelToTimeBetweenCommands.Add(1, 30);
        levelToTimeBetweenCommands.Add(2, 30);
        levelToTimeBetweenCommands.Add(3, 30);
        levelToTimeBetweenCommands.Add(4, 5);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        //textLevel.text = "1";
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
            increasePoints( (level/4) * Time.deltaTime);
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

        if(deathCounter >= 3 && !blackScreenEffectDone)
        {
            blackScreenEffectDone = true;
            Debug.Log("Black effect");
            StartCoroutine(blackScreenEffect());
        }
        if(deathCounter == rootSizeManager.maxSlots && !blueScreenEffectDone)
        {
            isGameRunning = false;
            blueScreenEffectDone = true;
            Debug.Log("Game finished");
            StartCoroutine(blueScreenEffect());
            monitorScreenMan.gameOver(points);
        }
    }

    public void UpdateLevel(int newLevel)
    {
        //textLevel.text = newLevel.ToString();
        if (newLevel > 1)
        {
            lookForSudo = true;
        }

        if(newLevel > 2)
        {
            virusCanHappen = true;
        }

        if(newLevel == 2)
        {
            eventMan.addNewEvents();
        }
        if (newLevel == 3)
        {
            eventMan.addNewEvents();
        }
        if (newLevel == 4)
        {
            eventMan.addNewEvents();
        }

        level = newLevel;

        _notifController.WriteInfo(newLevel);
        if(newLevel == 2 || newLevel == 3) {
            _traymanager.newInfo = true;
            _traymanager.newInfoIcon.SetActive(true);
        }
    }

    IEnumerator blackScreenEffect()
    {

        blackPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        blackPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        blackPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        blackPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        blackPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        blackPanel.gameObject.SetActive(false);
        yield return null;
    }

    IEnumerator blueScreenEffect()
    {
        blueScreen1.SetActive(true);
        blueScreen2.SetActive(true);
        yield return new WaitForSeconds(3f);
        blueScreen1.SetActive(false);
        blueScreen2.SetActive(false);
    }
}
