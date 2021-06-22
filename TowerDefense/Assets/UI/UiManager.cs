using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject moneyPanel;
    [SerializeField] private Image panelImage;
    [SerializeField] private Color32 colLerp;
    [SerializeField] private float startTime;
    [SerializeField] private float lerpTime;
    public static bool outOfMoney;
    public static bool changing;

    private void Start()
    {
        outOfMoney = false;
        changeMoney(BuildManagerScript.currentCash);
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
    }

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
}
