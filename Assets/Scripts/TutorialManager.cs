using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("----------Managers----------")]
    [SerializeField] private GameManager gameManager;
    [Header("----------Texts----------")]
    [SerializeField] private TextMeshProUGUI tutorialText;
    [Header("----------Objects----------")]
    [SerializeField] private GameObject spawnAnimalButtons;
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject tutorialButton;
    private int doneWithYouClicks = 0;
    private int tutorialClicks = 0;

    private readonly string text1 = "On the top left of your screen there is a tool selector.\n\n The top tool in the circle is the selected one.\n\n Try selecting a different tool.";
    private readonly string text2 = "With certain tools you can do different actions. You can build fences with a hammer, break fences and chop trees with an axe, make a planting bed with a hoe, plant seeds, harvest grown seeds with a scythe and shovel the stumps out with a shovel.";
    private readonly string text3 = "Try hoeing the ground and growing some seeds. I will speed up growth of your seeds.\n\n To use a tool just right click with your mouse on a tile (can also hold and drag).";
    private readonly string text4 = "You should say thanks next time for speeding things up (•̀ - •́ ). In the real game you'll have to wait 60 seconds of your time to grow them.\n Anyway, on the bottom left there are buttons for spawning animals. Try spawning a chicken.";
    private readonly string text5 = "Ok so I don't know if you can see the chicken you just spawned in. Try moving around with middle mouse button and zooming out with the scroll wheel. Once you find a chicken just click it so I can check that you learned something.";
    private readonly string text6 = "Good job! I'm proud of you. (.•ᵕ•.)\n\n So if you didn't notice, clicking on the animal brings it back to its spawn point. During the scarry night... animals can escape from their pen! ( ˶°□°)";
    private readonly string text7 = "When we are at the topic of the night already... Let me tell you that it's not pretty. A horrible monster spawns at night and tries to kill all your animals. You can try to build some fences with the hammer now. Give it a try!";
    private readonly string text8 = "One or two more things. The monster is faster every night so you really want to get to a 100 animals (win condition) FAST. You start with one each. Be careful about the ratio of the animals though so they dont kill each other. You can check the ratios later in the settings.";
    private readonly string text9 = "Oh and I totally forgot to mention this... On the top right you can see your resources. Seeds, grass and meat for spawning new animals, manure for making new planting beds and logs for building fences. You start with 20 of each so be careful how you spend 'em.";
    private readonly string text10 = "You can figure out everything else on your own... It should be pretty straight forward.\n\n And by the way, no problem for the info (¬ ͜  ͡¬). Now you can go play your little game. Bye! (>v<)/";
    private readonly string angryText1 = "I said a different tool... (•`_´•)";
    private readonly string angryText2 = "I sad \"try spawning a chicken\". Don't get funny with me! (¬_¬)";
    private readonly string angryText3 = "I said \"THE HAMMER\"! \\(`0´)/\n\nNow when I want you to pick the hammer now you don't want to do it huh. How dare you?";
    private readonly string imDoneWithYouText = "I've had enough. I you are so smart and don't need me... go play on your own. (•̀ ᴖ •́ )";

    public void AdvanceTutorial(int click)
    {
        tutorialClicks++;
        if (tutorialClicks == 1)
        {
            if (!CheckIfTutorialButtonClick(click)) return;
            tutorialText.text = text1;
            tutorialCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            tutorialButton.SetActive(false);
        }
        else if (tutorialClicks == 2)
        {
            if (click == 1)
            {
                if (CheckIfDoneWithYourClicks()) return;
                tutorialText.text = angryText1;
                doneWithYouClicks++;
                tutorialClicks--;
                return;
            }
            if (!CheckIfAnyOtherClick(click, 2)) return;
            tutorialText.text = text2;
            tutorialCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            tutorialButton.SetActive(true);
            doneWithYouClicks = 0;
        }
        else if (tutorialClicks == 3)
        {
            if (!CheckIfTutorialButtonClick(click)) return;
            tutorialText.text = text3;
        }
        else if (tutorialClicks == 4)
        {
            if (!CheckIfTutorialButtonClick(click)) return;
            tutorialCanvas.SetActive(false);
        }
        else if (tutorialClicks == 5)
        {
            if (!CheckIfAnyOtherClick(click, 4)) return;
            tutorialText.text = text4;
            tutorialCanvas.SetActive(true);
            tutorialCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            tutorialButton.SetActive(false);
            spawnAnimalButtons.GetComponentsInChildren<Button>().ToList().ForEach(button => button.interactable = true);
        }
        else if (tutorialClicks == 6)
        {
            if (click == 5)
            {
                if (CheckIfDoneWithYourClicks()) return;
                tutorialClicks--;
                tutorialText.text = angryText2;
                doneWithYouClicks++;
                return;
            }
            if (!CheckIfAnyOtherClick(click, 6)) return;
            spawnAnimalButtons.GetComponentsInChildren<Button>().ToList().ForEach(button => button.interactable = false);
            tutorialText.text = text5;
            tutorialCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            tutorialButton.SetActive(true);
            doneWithYouClicks = 0;
        }
        else if (tutorialClicks == 7)
        {
            if (!CheckIfTutorialButtonClick(click)) return;
            tutorialCanvas.SetActive(false);
        }
        else if (tutorialClicks == 8)
        {
            if (!CheckIfAnyOtherClick(click, 7)) return;
            tutorialText.text = text6;
            tutorialCanvas.SetActive(true);
        }
        else if (tutorialClicks == 9)
        {
            if (!CheckIfTutorialButtonClick(click)) return;
            tutorialText.text = text7;
            tutorialCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            tutorialButton.SetActive(false);
        }
        else if (tutorialClicks == 10)
        {
            if (click == 2)
            {
                if (CheckIfDoneWithYourClicks()) return;
                tutorialText.text = angryText3;
                tutorialClicks--;
                doneWithYouClicks++;
                return;
            }
            if (!CheckIfAnyOtherClick(click, 1)) return;
            tutorialCanvas.SetActive(false);
            doneWithYouClicks = 0;
        }
        else if (tutorialClicks == 11)
        {
            if (!CheckIfAnyOtherClick(click, 11)) return;
            tutorialCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            tutorialCanvas.SetActive(true);
            tutorialButton.SetActive(true);
            tutorialText.text = text8;
        }
        else if (tutorialClicks == 12)
        {
            if (!CheckIfTutorialButtonClick(click)) return;
            tutorialText.text = text9;
        }
        else if (tutorialClicks == 13)
        {
            if (!CheckIfTutorialButtonClick(click)) return;
            tutorialText.text = text10;
        }
        else if (tutorialClicks == 14)
        {
            if (!CheckIfTutorialButtonClick(click)) return;
            StartCoroutine(StartGameFromTutorial(0));
        }
    }

    private void Start()
    {
        gameManager = gameManager.GetComponent<GameManager>();
    }

    private IEnumerator StartGameFromTutorial(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Tutorial.isTutorial = false;
        gameManager.ReloadScene();
    }

    private bool CheckIfDoneWithYourClicks()
    {
        if (doneWithYouClicks > 10) return true;
        if (doneWithYouClicks == 10)
        {
            tutorialText.text = imDoneWithYouText;
            tutorialCanvas.SetActive(true);
            tutorialCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            tutorialButton.SetActive(false);
            StartCoroutine(StartGameFromTutorial(5));
            doneWithYouClicks++;
            return true;
        }
        return false;
    }

    private bool CheckIfTutorialButtonClick(int click)
    {
        return CheckIfAnyOtherClick(click, -1);
    }

    private bool CheckIfAnyOtherClick(int click, int number)
    {
        if (click != number)
        {
            tutorialClicks--;
            return false;
        }
        return true;
    }
}
