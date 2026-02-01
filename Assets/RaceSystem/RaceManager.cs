using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI currentLapTimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;
    [SerializeField] private TextMeshProUGUI overallRaceTimeText;
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private TextMeshProUGUI missedText;

    [Header("Race Settings")]
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private int lastCheckpointIndex = -1;
    [SerializeField] bool isCircuit = false;
    [SerializeField] private int totalLaps = 1;
    
    private int currentLap = 1;


    private bool raceStarted = false;
    private bool raceFinished = false;

    private bool missed = false;

    [Header("Lap Timer")]
    private float currentLapTime = 0f;
    private float bestLapTime = Mathf.Infinity;
    private float overralRaceTime = 0f;

    #region Unity Function
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (raceStarted)
        {
            UpdateTimers();
        }
        UpdateUI();
    }

    #endregion

    #region Checkpoint Management
    
    public void CheckpointReached(int checkpointIndex)
    {
        if ((!raceStarted && checkpointIndex != 0) || raceFinished) return;

        if(checkpointIndex == lastCheckpointIndex + 1)
        {
            UpdateCheckpoint(checkpointIndex);

            HideCheckpointMissedText();
        }
        else
        {
            bool validLapFinish = isCircuit && raceStarted && lastCheckpointIndex == checkpoints.Length - 1 && checkpointIndex == 0;

            if (validLapFinish)
            {
                HideCheckpointMissedText();
                UpdateCheckpoint(checkpointIndex);

            }
            else
            {

                ShowCheckpointMissedText();
            }
        }

    }
    
    private void UpdateCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex == 0)
        {
            if (!raceStarted)
            {
                StartRace();
            }
            else if (isCircuit && lastCheckpointIndex == checkpoints.Length - 1)
            {
                OnLapFinish();
            }
        }
        else if (!isCircuit && lastCheckpointIndex == checkpoints.Length - 1)
        {
            OnLapFinish();
        }

        lastCheckpointIndex = checkpointIndex;

    }
    
    #endregion

    #region Race Management

    private void OnLapFinish()
    {
        currentLap++;

        if (currentLapTime < bestLapTime)
        {
            bestLapTime = currentLapTime;
        }
        if (currentLap > totalLaps)
        {
            EndRace();
        }
        else
        {
            currentLapTime = 0f;
            lastCheckpointIndex = isCircuit ? 0 : -1;
        }

    }

    private void StartRace()
    {
        raceStarted = true;
        raceFinished = false;
    }

    private void EndRace()
    {
        raceStarted = false;
        raceFinished = true;

    }

    private void UpdateTimers()
    {
        currentLapTime += Time.deltaTime;
        overralRaceTime += Time.deltaTime;
    }
    
    private void UpdateUI()
    {
        currentLapTimeText.text = FormatTime(currentLapTime);
        overallRaceTimeText.text = FormatTime(overralRaceTime);
        lapText.text = "Tour: " + currentLap + "/" + totalLaps;
        bestLapTimeText.text = FormatTime(bestLapTime);

        UpdateCheckpointMissedText();
    }

    private void UpdateCheckpointMissedText()
    {
        if (missed)
        {
            float alpha = Mathf.PingPong(Time.time * 2, 1);
            Color newColor = missedText.color;
            newColor.a = alpha;
            missedText.color = newColor;
        }
    }
    private void ShowCheckpointMissedText()
    {
        if (!missed)
        {
            missedText.gameObject.SetActive(true);
            missed = true;
        }
    }

    private void HideCheckpointMissedText()
    {
        if (missed)
        {
            missedText.gameObject.SetActive(false);
            missed = false;
        }
    }

    #endregion

    #region Utility Functions

    private string FormatTime(float time)
    {
        if (float.IsInfinity(time) || time < 0) return "--:--";
        
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00} : {1:00}", minutes, seconds);

    }

   #endregion

}
