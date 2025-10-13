using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class formulaUpgrade : MonoBehaviour
{
    GameManager gameManager;

    public Button button;

    public TMP_Text text;

//declaração das variaveis
    private double cost; 

    private double level;

    private double maxlevel;

    bool Max = false;

    public void PressButton()
    {
        double[] handler = gameManager.UpgradeFormula(); //pega os valores da função de upgrade do gameManager
        string costStr;
        //aranja os valores em suas devidas variaveis
        cost = handler[0];
        level = handler[1];
        maxlevel = handler[2];
        //trata as variaveis para elas serem mais bonitas no display
        if (cost == 0) Max = true;
        if (cost > 1000)
        {
            costStr = cost.ToString("1.0e00");
        }
        else
        {
            costStr = cost.ToString("0.0");
        }
        
        text.text = $"Chazinho \nFormula lv.{level}/{maxlevel} \nCusto: {costStr} vagas";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>(); // procura o primeiro objeto com gameManager
        cost = gameManager.GetFormulaCost(); // usa get para pegar o valor do upgrade
        level = gameManager.formulaType; //usa acesso direto devido a ser public
        maxlevel = gameManager.maxFormulaType;
    }
    // Update is called once per frame
    void Update()
    {
        if(cost >= gameManager.GetVagas() || Max)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}

