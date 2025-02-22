using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollAnimation : MonoBehaviour
{
    private float waitTime = 2;
    private float scrollSpeed = 100;
    public GameObject sceneManager;

    private float goToMainMenu = 30;
    void Update()
    {
        goToMainMenu -= Time.deltaTime;
        if (goToMainMenu < 0)
        {
            sceneManager.GetComponent<GameSceneManager>().LoadMainScene();
        }
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
    }
}
