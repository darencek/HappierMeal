using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool placed = false;
    bool colliderTouching = false;

    public int cost = 100;

    public int pointsAdd = 1;
    public float tickInterval = 10f;

    static float snapSize = 2.5f;

    BuildingScript buildingScript;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TickRun");
        buildingScript = GetComponent<BuildingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!placed)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            worldPosition.x = Mathf.Round(worldPosition.x / snapSize) * snapSize;
            worldPosition.y = Mathf.Round(worldPosition.y / snapSize) * snapSize;

            transform.position = worldPosition;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -5, 5), 0);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlaceBuilding();
            }
        }
    }

    void Tick()
    {
        if (!placed) return;
        MainManager.instance.AddDreamPoints(pointsAdd);
        if (!(buildingScript is null))
            buildingScript.Tick();
    }
    IEnumerator TickRun()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(tickInterval);
        }
    }
    void PlaceBuilding()
    {
        foreach (Collider2D c in Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0))
        {
            if (c.gameObject != gameObject && !c.isTrigger)
                return;
        }
        placed = true;
    }
}
