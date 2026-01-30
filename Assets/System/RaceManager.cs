using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("Race Settings")]
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private int lastCheckpointIndex = -1;
    [SerializeField] private bool isCircuit = false;
    [SerializeField] private int totalLaps = 1;

    private int currentLap = 1;

    private bool raceStarted = false;
    private bool raceFinished = false;

    #region Unity Functions
    private void Awake()
    {
        if (Instance == true)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region CheckpointManagement

    public void CheckPointReached(int checkpointIndex)
    {
        if ((!raceStarted && checkpointIndex != 0) || raceFinished) return;

        if (checkpointIndex == lastCheckpointIndex + 1)
        {
            UpdateCheckpoint(checkpointIndex);
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

    #region StartRace
    private void StartRace()
    {
        raceStarted = true;
        raceFinished = false;
    }

    private void OnLapFinish()
    {
        currentLap++;

        if (currentLap > totalLaps)
        {
            EndRace();
        }
    }

    private void EndRace()
    {
        raceFinished = true;
        raceStarted = false;
    }
    #endregion


}