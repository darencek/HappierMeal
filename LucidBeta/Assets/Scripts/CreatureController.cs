using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    float moveSpeed = 1f;
    Vector3 moveTarget;

    public GameObject[] creatureSprites;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = Random.Range(0.1f, 0.3f);

        moveTarget = transform.position;
        StartCoroutine("doNextMoveTarget");

        foreach (GameObject g in creatureSprites)
            g.SetActive(false);

        creatureSprites[Random.Range(0, creatureSprites.Length - 1)].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -5, 0), 0);
    }

    private void OnMouseDown()
    {
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
