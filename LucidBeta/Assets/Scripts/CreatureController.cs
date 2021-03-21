using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    float moveSpeed = 1f;
    Vector3 moveTarget;

    Rigidbody2D rb;

    public GameObject[] creatureSprites;
    public GameObject currentSprite;

    public Creature info;
    public Creature.CreatureType type;

    public int friendshipLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = Random.Range(0.1f, 1f) * 0.1f;

        moveTarget = transform.position;
        StartCoroutine("doNextMoveTarget");

        foreach (GameObject g in creatureSprites)
            g.SetActive(false);

        type = (Creature.CreatureType)Random.Range(0, System.Enum.GetNames(typeof(Creature.CreatureType)).Length);
        info = new Creature(type);
        currentSprite = creatureSprites[(int)type];

        currentSprite.SetActive(true);

        MainManager.creatureManager.CheckNewEncounter(this);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -5, 0), 0);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, moveTarget, moveSpeed * Time.fixedDeltaTime));
    }

    bool clicked = false;
    private void OnMouseDown()
    {
        clicked = true;
    }
    private void OnMouseUp()
    {
        if (clicked)
        {
            if (!MainManager.CamPan_JustReleased && !MainManager.MouseOnUI)
            {
                MainManager.uiManager.UI_ShowDialogBox(this);
            }
        }
        clicked = false;
    }


    IEnumerator doNextMoveTarget()
    {
        while (true)
        {
            moveTarget = transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-2, 2));
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

}

public struct Creature
{
    public string name;

    public string defaultDialog;

    public enum CreatureType
    {
        GLOOMY, GLOW, BIG, EFFORT, JITTERY, AIR
    }

    public Creature(CreatureType c)
    {
        name = "Dong";
        defaultDialog = "hurrdurr";

        switch (c)
        {
            case CreatureType.GLOOMY:
                name = "Gloomy";
                defaultDialog = "Everyday is feels so grey...";
                break;
            case CreatureType.GLOW:
                name = "Glow";
                defaultDialog = "I love bright things!";
                break;
            case CreatureType.BIG:
                name = "Big";
                defaultDialog = "I AM A WHALE.";
                break;
            case CreatureType.EFFORT:
                name = "Effort";
                defaultDialog = "All work and no play makes me a hard worker!";
                break;
            case CreatureType.JITTERY:
                name = "Jittery";
                defaultDialog = "Fast! Everything is so fast!";
                break;
            case CreatureType.AIR:
                name = "Air";
                defaultDialog = "I'm so light, so free!";
                break;
        }

    }
}