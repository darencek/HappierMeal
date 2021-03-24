using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FruitTree_Fruit : MonoBehaviour
{
    public Building parent;
    public GameObject moneyCounterPrefab;
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
                float an = 7000;
                MainManager.instance.zees += an;

                GameObject g = Instantiate(moneyCounterPrefab, transform.position + Vector3.forward * 10f, Quaternion.identity);
                g.GetComponent<TextMeshPro>().text = "$" + an;
            }
        }
    }
}
