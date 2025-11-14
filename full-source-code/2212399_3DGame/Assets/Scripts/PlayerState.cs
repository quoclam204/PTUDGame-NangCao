using System;
using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float currentHealth, maxHealth;
    public float currentCalories, maxCalories;
    public float currentHydration, maxHydration;
    public bool isHydrationActive;
    float distanceTraveled = 0;
    Vector3 lastPosition;
    public GameObject player;
    public static PlayerState Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories; // start with full calories
        currentHydration = maxHydration; // start with full hydration
        StartCoroutine(decreaseHydration());
        if (player != null)
            lastPosition = player.transform.position; // prevent initial large distance spike
    }

    IEnumerator decreaseHydration()
    {
        while(true)
        {
            currentHydration -= 1;
            yield return new WaitForSeconds(10); 
            if (currentHydration < 0)
                currentHydration = 0;
        }
    }

    void Update()
    {
        distanceTraveled += Vector3.Distance(player.transform.position, lastPosition);
        lastPosition = player.transform.position;
        if (distanceTraveled >= 20)
        {
            currentCalories -= 1;
            distanceTraveled = 0;
        }
    }
    public void setHealth(float health)
    {
        currentHealth = health;
    }

    public void setCalories(float calories)
    {
        currentCalories = calories;
    }

    public void setHydration(float hydration)
    {
        currentHydration = hydration;
    }

}
