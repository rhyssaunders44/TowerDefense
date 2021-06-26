using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject moneyPanel;
    [SerializeField] private Image panelImage;
    [SerializeField] private Color32 colLerp;
    [SerializeField] private float startTime;
    [SerializeField] private float lerpTime;
    [SerializeField] private Image heatBar;
    [SerializeField] private Image baseHealthBar;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    public static int baseHp;
    [SerializeField] private int maxBaseHp;
    public static bool outOfMoney;
    public static bool changing;
    [SerializeField] private bool paused;

    private void Start()
    {
        Time.timeScale = 1;
        paused = false;
        maxBaseHp = 10;
        baseHp = maxBaseHp;
        outOfMoney = false;
        changeMoney(BuildManagerScript.currentCash);
        imagecolorChanger(baseHp, maxBaseHp, baseHealthBar, Color.red, Color.yellow, Color.green);
    }

    private void Update()
    {

        if (changing)
        {
            changeMoney(BuildManagerScript.currentCash);
        }

        if (outOfMoney)
        {
            BuildManagerScript.obstructed = true;

            changeMoney(BuildManagerScript.currentCash);

            if(changing)
                colorFlash(panelImage);
        }

        //changes the hethbar color based on heat amount
        imagecolorChanger(BuildManagerScript.heat, BuildManagerScript.heatMax, heatBar, Color.green, Color.yellow, Color.red);

        //changes healthbar color baseed on hp
        imagecolorChanger(baseHp, maxBaseHp, baseHealthBar, Color.red, Color.yellow, Color.green);

        if(baseHp <= 0)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                PauseMenu(paused);
            }
            if (!paused)
            {
                PauseMenu(!paused);
            }
        }
    }

    public void PauseMenu(bool isPaused)
    {
        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }

    }

    //my attempt to get the money panel to flash red if you cant buy the selected turret
    //doesnt work
    public void colorFlash(Image flashingObject)
    {
        startTime = Time.time;
        lerpTime = startTime / 1;

        if (lerpTime > 1)
            lerpTime = 1;

        colLerp = Color32.Lerp(Color.red, Color.clear , lerpTime);
        flashingObject.color = colLerp;

        if (startTime > 1 && BuildManagerScript.currentCash > 0)
        {
            outOfMoney = false;
        }
    }

    public void changeMoney(int money)
    {
        moneyText.text = "Credits:" + money.ToString();
        changing = false;
    }

    public void imagecolorChanger(float currentNumber, float maxNumber, Image image, Color col1, Color col2, Color col3)
    {

        image.fillAmount = currentNumber / maxNumber;

        if (currentNumber <= (maxNumber *0.3f))
        {
            image.color = col1;
        }

        if (currentNumber > ( maxNumber * 0.3f) && currentNumber < (maxNumber * 0.8f))
        {
            image.color = col2;
        }

        else if (currentNumber >= (maxNumber * 0.8f))
        {
            image.color = col3;
        }

    }

    public void restartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        inGameUI.SetActive(false);
        gameOverPanel.SetActive(true);
    }

}
