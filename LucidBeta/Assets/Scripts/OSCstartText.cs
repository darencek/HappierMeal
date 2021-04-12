using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OSCstartText : MonoBehaviour
{

    TextMeshProUGUI tx;
    // Start is called before the first frame update
    void Start()
    {
        tx = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Color c = tx.color;
        c.a = Mathf.PingPong(Time.time, 2f);
        tx.color = c;
    }
}
