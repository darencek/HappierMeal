using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = Camera.main.transform.position;

        v.x = Mathf.PingPong(Time.time/30f, 0.1f);
        v.y = Mathf.PingPong(Time.time/47f, 0.1f);

        Camera.main.transform.position = v;

        if (Input.anyKeyDown)
        {
            GetComponent<Animator>().SetTrigger("skip");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        StartCoroutine("StartGameCR");
    }

    IEnumerator StartGameCR()
    {
        GetComponent<Animator>().SetTrigger("start");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene("MainScene");
    }
}
