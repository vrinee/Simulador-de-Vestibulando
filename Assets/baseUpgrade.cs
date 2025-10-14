using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class baseUpgrade : MonoBehaviour
{
    GameManager gameManager;

    public Button button;

    public TMP_Text text;

//declaração das variaveis
    private double cost; 

    private double increment;

    private double level;

    public void PressButton()
    {
        
        string costStr;
        string incrementStr;
        //aranja os valores em suas devidas variaveis
        
        //trata as variaveis para elas serem mais bonitas no display
        if (cost > 1000)
        {
            costStr = cost.ToString("1.0e00");
        }
        else
        {
            costStr = cost.ToString("0.0");
        }
        if (increment > 1000)
        {
            incrementStr = increment.ToString("1.0e00");
        }
        else
        {
            incrementStr = increment.ToString("0.0");
        }
        
        text.text = $"Lapis lv.{level} \nBase +{incrementStr} \nCusto: {costStr} vagas";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>(); // procura o primeiro objeto com gameManager
        
    }
    // Update is called once per frame
    void Update()
    {
        if(cost >= gameManager.GetVagas())
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    } 
}
