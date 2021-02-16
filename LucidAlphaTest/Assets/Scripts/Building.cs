using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool placed = false;
    bool colliderTouching = false;

    public static int cost = 100;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TickRun");
    }

    // Update is called once per frame
    void Update()
    {
        if (!placed)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            transform.position = worldPosition;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -5, 0), 0);
        }
    }

    void Tick()
    {
        if (!placed) return;
        MainManager.instance.AddDreamPoints(1);
    }
    IEnumerator TickRun()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(5f);
        }
    }
    void PlaceBuilding()
    {
        foreach (Collider2D c in Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0))
        {
            if (c.gameObject != gameObject)
                return;
        }
        placed = true;
    }
    private void OnMouseDown()
    {
        PlaceBuilding();
    }
}
