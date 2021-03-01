using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMockup : MonoBehaviour
{
    public float scrollTimer = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        scrollTimer = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        scrollTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && scrollTimer <= 0)
        {
            AdvanceDialogue();
        }
    }

    public void AdvanceDialogue()
    {
        gameObject.SetActive(false);
    }
}
