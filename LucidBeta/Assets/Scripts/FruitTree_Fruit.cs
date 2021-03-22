using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree_Fruit : MonoBehaviour
{
    public Building parent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && !MainManager.MouseOnUI && !parent.CancelClick)
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = transform.position.z;
            if (Vector3.Distance(p, transform.position) < 0.5f)
            {
                parent.CancelClick = true;
                gameObject.SetActive(false);
                MainManager.instance.zees += 7000;
            }
        }
    }
}
