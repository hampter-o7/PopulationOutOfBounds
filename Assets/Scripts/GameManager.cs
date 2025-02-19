using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI _animalCountText;
    public int animalCount;

    void Start()
    {
        _animalCountText.text = "Current animal count: " + animalCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
