using UnityEngine;

public class ScrollAnimation : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private float scrollSpeed = 100;
    private float goToMainMenu = 25;

    private void Update()
    {
        goToMainMenu -= Time.deltaTime;
        if (goToMainMenu < 0) sceneManager.GetComponent<GameSceneManager>().LoadMainScene();
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
    }
}
