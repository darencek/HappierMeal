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
    public Creature.CreatureType type = Creature.CreatureType.GLOOMY;

    public int friendshipLevel = 0;

    Animator spriteAnimator;
    SpriteRenderer spriteRenderer;

    Vector2 _lastPos;
    bool moving = false;

    bool spriteReady = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = Random.Range(0.1f, 1f) * 0.1f;

        moveTarget = transform.position;
        _lastPos = transform.position;
        StartCoroutine("doNextMoveTarget");

        StartCoroutine("delayInit");
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

    // Update is called once per frame
    void Update()
    {
        if (!spriteReady) return;

        spriteAnimator.SetInteger("animState", moving ? 1 : 0);
        spriteRenderer.flipX = (moveTarget.x < transform.position.x);


    }

    private void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, moveTarget, moveSpeed * Time.fixedDeltaTime));
        moving = rb.position != _lastPos;
        _lastPos = rb.position;
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

    IEnumerator delayInit()
    {
        yield return new WaitForEndOfFrame();
        UpdateSprite();
    }
    IEnumerator doNextMoveTarget()
    {
        while (true)
        {
            moveTarget = transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
            yield return new WaitForSeconds(Random.Range(5f, 20f));
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