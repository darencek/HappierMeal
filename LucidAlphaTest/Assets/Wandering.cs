using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : MonoBehaviour
{
    [SerializeField]
    float Speed;
    [SerializeField]
    float Range;
    [SerializeField]    
    float maxDistance;

    float idleTime;
    float stopTime = 3;
    float waitTime = 5;

    float mood = 0;
    public GameObject FloatingMood1;
    public GameObject FloatingMood2;

    Vector2 Position;

    void Start()
    {
        WanderingAI();
        mood = Random.Range(1, 3);
    }

    void Update()
    {
        idleTime += Time.deltaTime;

        if (idleTime < stopTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, Position, Speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Position) < Range)
            {
                WanderingAI();
            }
        }
        else
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;

                if (mood == 1)
                {
                    ShowFloatingMood1();
                    mood = 0;
                }
                else if (mood == 2)
                {
                    ShowFloatingMood2();
                    mood = 0;
                }
            }
            else
            {
                Destroy(GameObject.FindWithTag("Mood"));
                mood = Random.Range(1, 3);
                idleTime = 0;
                waitTime = 5;
            }
        }
    }

    void WanderingAI()
    {
        Position = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }

    void ShowFloatingMood1()
    {
        Instantiate(FloatingMood1, transform.position, Quaternion.identity);
    }
    void ShowFloatingMood2()
    {
        Instantiate(FloatingMood2, transform.position, Quaternion.identity);
    }
}
