using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyGainMarker : MonoBehaviour
{

    TextMeshPro t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, 1, 0) * Time.deltaTime;

        Color c = t.color;
        c.a -= 0.5f * Time.deltaTime;

        t.color = c;

        if (c.a <= 0f)
            Destroy(gameObject);
    }
}
