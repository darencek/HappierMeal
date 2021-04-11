using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    float moveSpeed = 1f;
    float moveDelay = 0;
    List<Vector3> path = new List<Vector3>();

    public GameObject[] creatureSprites;
    public GameObject currentSprite;

    public Creature info;
    public Creature.CreatureType type = Creature.CreatureType.GLOOMY;

    public int friendshipLevel = 0;

    Animator spriteAnimator;
    SpriteRenderer spriteRenderer;

    bool moving = false;

    bool spriteReady = false;

    Creature.CreatureType lasttype;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = Random.Range(0.3f, 0.5f);

    }
    public void UpdateSprite()
    {
        foreach (GameObject g in creatureSprites)
            g.SetActive(false);

        info = new Creature(type);
        currentSprite = creatureSprites[(int)type];

        currentSprite.SetActive(true);
        spriteAnimator = currentSprite.GetComponent<Animator>();
        spriteRenderer = currentSprite.GetComponent<SpriteRenderer>();

        MainManager.creatureManager.CheckNewEncounter(this);

        spriteReady = true;
    }

    public void SetType(Creature.CreatureType t)
    {
        type = lasttype = t;
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (lasttype != type)
            SetType(type);

        if (!spriteReady) return;

        if (path.Count > 0)
        {
            moving = true;
            Vector3 tar = path[0];
            transform.position = Vector3.MoveTowards(transform.position, tar, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, tar) < 0.01f)
                path.RemoveAt(0);

            spriteRenderer.transform.localScale = new Vector3((tar.x < transform.position.x) ? -1 : 1, 1, 1);
        }
        else
        {
            moving = false;
            if (moveDelay > 0)
            {
                moveDelay -= Time.deltaTime;
            }
            else
            {
                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                GameObject target = buildings[Random.Range(0, buildings.Length)];
                path = PathfindingManager.instance.GetPath(transform.position, target.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0));
                moveDelay = Random.Range(5, 10);
            }
        }

        spriteAnimator.SetInteger("animState", moving ? 1 : 0);
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

    public void RevealSequence()
    {
        StartCoroutine("RevealCoroutine");
    }

    IEnumerator RevealCoroutine()
    {
        MainManager.uiManager.blockUI = true;
        Time.timeScale = 0f;
        MainManager.instance.CamPan_SmoothToTarget(transform.position);
        yield return new WaitForSecondsRealtime(2f);
        MainManager.uiManager.blockUI = false;
        MainManager.uiManager.UI_ShowNewCreaturePopup(this);
        // Time.timeScale = 1f;
        // MainManager.instance.revealRunning = false;
        yield return null;
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
                defaultDialog = "Everyday I feel so grey...";
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