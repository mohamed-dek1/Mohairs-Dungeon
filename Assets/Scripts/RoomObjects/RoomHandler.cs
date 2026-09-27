using UnityEngine;
using System;

public class RoomHandler : MonoBehaviour
{
    public event Action OnRoomEnter;
    public event Action OnRoomExit;
    public event Action OnRoomCleared;

    // ==========================
    // Serialized fields
    // ==========================
    [SerializeField] private RoomObject[] roomObjects;

    // ==========================
    // Private fields
    // ==========================
    private int enemiesRemaining = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (RoomObject obj in roomObjects)
            obj.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && OnRoomEnter != null)
            OnRoomEnter();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && OnRoomExit != null)
            OnRoomExit();
    }

    public void RegisterEnemy()
    {
        enemiesRemaining++;
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;

        if (enemiesRemaining == 0 && OnRoomCleared != null)
            OnRoomCleared();
    }
}
