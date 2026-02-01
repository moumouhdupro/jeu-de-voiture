using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        if (!rb.CompareTag("Player")) return;

        triggered = true;
        RaceManager.Instance.CheckpointReached(checkpointIndex);
    }
    
}
