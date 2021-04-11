using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooman : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject sprite;

    float speed = 10;
    List<Vector3> path = new List<Vector3>();

    float delay = 0;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.2f, 0.5f);

        SpriteRenderer ren = sprite.GetComponent<SpriteRenderer>();
        ren.sprite = sprites[Random.Range(0, sprites.Length)];
        ren.color = new Color(Random.Range(0.6f, 0.8f), Random.Range(0.6f, 0.8f), Random.Range(0.6f, 0.8f));
    }

    // Update is called once per frame
    void Update()
    {
        if (path.Count > 0)
        {
            Vector3 tar = path[0];
            transform.position = Vector3.MoveTowards(transform.position, tar, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, tar) < 0.01f)
                path.RemoveAt(0);

            sprite.transform.localPosition = new Vector3(0, Mathf.PingPong(Time.time * speed, 0.03f), 0);
        }
        else
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }
            else
            {
                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                GameObject target = buildings[Random.Range(0, buildings.Length)];
                if (target.GetComponent<Building>().type != Building.BuildingType.NONE)
                {
                    path = PathfindingManager.instance.GetPath(transform.position, target.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0));
                    delay = Random.Range(5, 10);
                }
            }
        }
    }
}
