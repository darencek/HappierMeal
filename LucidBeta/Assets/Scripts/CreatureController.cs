using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreatureController : MonoBehaviour
{
    float moveSpeed = 1f;
    float moveDelay = 0;
    List<Vector3> path = new List<Vector3>();

    public GameObject[] creatureSprites;
    public GameObject currentSprite;

    public GameObject popupPrefab;

    public Creature info;
    public Creature.CreatureType type = Creature.CreatureType.GLOOMY;

    public int friendshipLevel = 1;

    Animator spriteAnimator;
    SpriteRenderer spriteRenderer;

    bool moving = false;

    bool spriteReady = false;

    Creature.CreatureType lasttype;

    public float helpTimer = 0f;
    public float helpCooldown = 0f;
    bool helpBonusGiven = false;
    int helpCheckLimit = 0;

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
            spriteAnimator.SetInteger("animState", 1);

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

            if (helpTimer > 0)
            {
                ExecuteHelp();
                spriteAnimator.SetInteger("animState", 2);
                helpTimer -= Time.deltaTime;
            }
            else
            {
                spriteAnimator.SetInteger("animState", 0);

                if (moveDelay > 0)
                {
                    moveDelay -= Time.deltaTime;
                }
                else
                {
                    if (helpCooldown <= 0)
                    {
                        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                        GameObject target = buildings[Random.Range(0, buildings.Length)];
                        Building bb = target.GetComponent<Building>();
                        if (helpCheckLimit++ < 10)
                        {
                            if (!bb.placing && CheckIfPreferredBuilding(bb.type))
                            {
                                path = PathfindingManager.instance.GetPath(transform.position, target.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0));
                                moveDelay = Random.Range(5, 10);
                                helpCooldown = Random.Range(10, 20) * 60;
                                helpTimer = 10f;
                                helpBonusGiven = false;
                                friendshipLevel++;
                            }
                        }
                        else
                        {
                            friendshipLevel--;
                            helpCooldown = 20;
                            helpCheckLimit = 0;
                        }
                    }
                    else
                    {
                        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                        GameObject target = buildings[Random.Range(0, buildings.Length)];
                        path = PathfindingManager.instance.GetPath(transform.position, target.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0));
                        moveDelay = Random.Range(5, 10);
                    }

                }
            }
        }

        friendshipLevel = Mathf.Clamp(friendshipLevel, 0, 5);

        if (helpCooldown > 0 && helpTimer <= 0)
            helpCooldown -= Time.deltaTime * MainManager.dreamTimeScale;
    }

    bool CheckIfPreferredBuilding(Building.BuildingType t)
    {
        switch (type)
        {
            case Creature.CreatureType.GLOW:
                if (t == Building.BuildingType.DREAM_MACHINE)
                    return true;
                break;
            case Creature.CreatureType.GLOOMY:
                if (t == Building.BuildingType.LARGE_FARM || t == Building.BuildingType.SMALL_FARM)
                    return true;
                break;
            case Creature.CreatureType.BIG:
                if (t == Building.BuildingType.BAKERY)
                    return true;
                break;
            case Creature.CreatureType.AIR:
                return true;
            case Creature.CreatureType.EFFORT:
                if (t == Building.BuildingType.FISHERY)
                    return true;
                break;
            case Creature.CreatureType.JITTERY:
                if (t == Building.BuildingType.DREAM_MACHINE || t == Building.BuildingType.BAKERY || t == Building.BuildingType.FACTORY)
                    return true;
                break;
        }
        return false;
    }

    void ExecuteHelp()
    {
        float scale = MainManager.dreamTimeScale / 1200;
        scale = Mathf.Max(1, scale);
        switch (type)
        {
            case Creature.CreatureType.GLOW:
                if (helpTimer < 5 && !helpBonusGiven)
                {
                    float bonus = MainManager.instance.rest_limit * 0.005f * (scale);
                    MainManager.instance.rest_resource += bonus;
                    helpBonusGiven = true;
                    helpTimer = 0;

                    GameObject g = Instantiate(popupPrefab, transform.position + Vector3.forward * 10f, Quaternion.identity);
                    g.GetComponent<TextMeshPro>().text = Mathf.FloorToInt(bonus) + " R";
                }
                break;
            case Creature.CreatureType.GLOOMY:
                if (helpTimer < 5 && !helpBonusGiven)
                {
                    GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                    foreach (GameObject bo in buildings)
                    {
                        if (Vector3.Distance(bo.transform.position, transform.position) <= 2f && CheckIfPreferredBuilding(bo.GetComponent<Building>().type))
                        {
                            bo.GetComponent<FarmPlot>().Fertilize(0.5f);
                            GameObject g = Instantiate(popupPrefab, transform.position + Vector3.forward * 10f, Quaternion.identity);
                            g.GetComponent<TextMeshPro>().text = "+Grow";
                            break;
                        }
                    }
                    helpBonusGiven = true;
                }
                break;
            case Creature.CreatureType.BIG:
                if (helpTimer < 5 && !helpBonusGiven)
                {
                    float bonus = MainManager.instance.energy_limit * 0.005f * (scale);
                    MainManager.instance.energy_resource += bonus;
                    helpTimer = 0;
                    helpBonusGiven = true;

                    GameObject g = Instantiate(popupPrefab, transform.position + Vector3.forward * 10f, Quaternion.identity);
                    g.GetComponent<TextMeshPro>().text = Mathf.FloorToInt(bonus) + " E";
                }
                break;
            case Creature.CreatureType.AIR:
                if (helpTimer < 8 && !helpBonusGiven)
                {
                    if (MainManager.instance.rest_resource >= MainManager.instance.rest_limit * 0.7f)
                    {
                        float bonus = MainManager.instance.energy_limit * 0.005f * (scale);
                        MainManager.instance.energy_resource += bonus;

                        GameObject g = Instantiate(popupPrefab, transform.position + Vector3.forward * 10f, Quaternion.identity);
                        g.GetComponent<TextMeshPro>().text = Mathf.FloorToInt(bonus) + " E";
                    }
                    helpTimer = 0;
                    helpBonusGiven = true;
                }
                break;
            case Creature.CreatureType.EFFORT:
                if (helpTimer < 4 && !helpBonusGiven)
                {
                    float bonus = MainManager.instance.energy_limit * 0.005f * (scale);
                    MainManager.instance.energy_resource += MainManager.instance.energy_limit * bonus;
                    helpBonusGiven = true;

                    GameObject g = Instantiate(popupPrefab, transform.position + Vector3.forward * 10f, Quaternion.identity);
                    g.GetComponent<TextMeshPro>().text = Mathf.FloorToInt(bonus) + " E";
                }
                break;
            case Creature.CreatureType.JITTERY:
                if (helpTimer < 5 && !helpBonusGiven)
                {
                    GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                    foreach (GameObject bo in buildings)
                    {
                        if (Vector3.Distance(bo.transform.position, transform.position) <= 2f && CheckIfPreferredBuilding(bo.GetComponent<Building>().type))
                        {
                            bo.GetComponent<Building>().energized = true;
                            break;
                        }
                    }
                    helpBonusGiven = true;
                }
                break;
        }
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
    public string hint;

    public enum CreatureType
    {
        GLOOMY, GLOW, BIG, EFFORT, JITTERY, AIR
    }

    public Creature(CreatureType c)
    {
        name = "Dong";
        defaultDialog = "hurrdurr";
        hint = "";

        switch (c)
        {
            case CreatureType.GLOOMY:
                name = "Dusty";
                defaultDialog = "Everyday I feel so grey...";
                hint = "Dusty wants more colour in his life. He will help to fertilize gardens to grow them faster.";

                break;
            case CreatureType.GLOW:
                name = "Cappy";
                defaultDialog = "I love bright things! I think they're neat.";
                hint = "Cappy likes to watch the glow of dream machines. He will help to make rest when he is happy.";
                break;
            case CreatureType.BIG:
                name = "Moby";
                defaultDialog = "I AM A BIG BOY. WOoAoAooAAooOOoo!";
                hint = "Moby loves snacks. He will hang around bakeries and help to bake cookies.";
                break;
            case CreatureType.EFFORT:
                name = "Frogbert";
                defaultDialog = "All work and no play makes me a hard worker!";
                hint = "Frogbert loves to fish. Build him a fishery and he will fish for you.";
                break;
            case CreatureType.JITTERY:
                name = "Fungy";
                defaultDialog = "Fast! Everything is so fast!";
                hint = "Fungy likes lots of energy. It will help to energize buildings to work faster.";
                break;
            case CreatureType.AIR:
                name = "Dandi";
                defaultDialog = "I'm so light, so free!";
                hint = "Dandi likes to see someone well rested. She will give you a boost if you are well rested.";
                break;
        }

    }
}