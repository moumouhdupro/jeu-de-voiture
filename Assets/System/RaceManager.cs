using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI currentLapTimeText;
    [SerializeField] private TextMeshProUGUI overallLapTimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;
    [SerializeField] private TextMeshProUGUI lapText;

    [Header("Race Settings")]
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private int lastCheckpointIndex = -1;
    [SerializeField] private bool isCircuit = false;
    [SerializeField] private int totalLaps = 1;

    private int currentLap = 1;

    private bool raceStarted = false;
    private bool raceFinished = false;

    [Header("Lap Timer")]
    private float currentLapTime = 0f;
    private float bestLapTime = Mathf.Infinity;
    private float overallLapTime = 0f;

    #region Unity Functions
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

    #region CheckpointManagement

    public void CheckPointReached(int checkpointIndex)
    {
        if ((!raceStarted && checkpointIndex != 0) || raceFinished) return;

        if (checkpointIndex == lastCheckpointIndex + 1)
        {
            //UpdateCheckpoint();
        }
    }

    public void UpdateCheckpoint(int checkpointIndex)
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
        else if (!isCircuit && checkpointIndex == checkpoints.Length - 1)
        {
            OnLapFinish();
        }

        lastCheckpointIndex = checkpointIndex;
    }
    #endregion

    #region Race Managment
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
        raceFinished = true;
        raceStarted = false;
    }
    private void UpdateTimers()
    {
        currentLapTime += Time.deltaTime;
        overallLapTime += Time.deltaTime;

    }

    private void UpdateUI()
    {
        currentLapTimeText.text = FormatTime(currentLapTime);
        overallLapTimeText.text = FormatTime(overallLapTime);
        lapText.text = "Tour : " + currentLap + "/" + totalLaps;
        bestLapTimeText.text = FormatTime(bestLapTime);
    }

    #endregion

    #region Utility Functions

    private string FormatTime(float time)
    {
        if (float.IsInfinity(time) || time < 0) return "--:--";
        
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);


    }
    #endregion
}