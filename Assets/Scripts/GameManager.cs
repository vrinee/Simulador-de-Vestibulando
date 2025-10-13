using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{


    public bool debugMode = false; //modo de debug para testes

    public GameObject modalPrefab;

    public Transform modalSpawnPoint;

    [Header("Base da Fórmula")]
    public double numBase = 1;//base da fórmula

    public double numBaseIncrement = 1; //incremento da base da fórmula

    public double numBaseScaling = 1.2; //fator de escala do incremento da base da fórmula

    public double numBaseUpgradeCost = 1; //custo do upgrade da base da fórmula

    public double numBaseUpgradeCostScaling = 1.1; //fator de escala do custo do upgrade da base da fórmula

    public int numBaseUpgradeLevel = 0; //nível do upgrade da base da fórmula

    [Header("Coeficiente da Fórmula")]
    public double coefficient = 1;//coeficiente da fórmula

    public double coefficientScaling = 1.05; //fator de escala do coeficiente da fórmula

    public double coefficientUpgradeCost = 1000; //custo do upgrade do coeficiente da fórmula

    public double coefficientUpgradeCostScaling = 2; //fator de escala do custo do upgrade do coeficiente da fórmula

    public int coefficientUpgradeLevel = 0; //nível do upgrade do coeficiente da fórmula

    [Header("Exponente da Fórmula")]
    public double exponent = 1;//exponente da fórmula

    public double exponentScaling = 1.01; //fator de escala do exponente da fórmula

    public double exponentUpgradeCost = 10000; //custo do upgrade do exponente da fórmula

    public double exponentUpgradeCostScaling = 10000; //fator de escala do custo do upgrade do exponente da fórmula

    public int exponentUpgradeLevel = 0; //nível do upgrade do exponente da fórmula

    [Header("Tipo da Fórmula")]
    public int formulaType = 0; // tipo/nível da fórmula que está sendo usada

    public int maxFormulaType = 2; // número máximo de tipos/níveis de fórmulas

    public double formulaUpgradeCost = 1000; // custo para fazer o upgrade da fórmula

    public double formulaUpgradeCostScaling = 1e10; // fator de escala do custo para fazer o upgrade da fórmula

    [Header("Elementos da UI")]
    public TMP_Text vagasText; //Elemento de UI para a quantidade de vagas

    public TMP_Text vagasPorSegundoText; //Elemento de UI para a quantidade de vagas por segundo

    public TMP_Text formulaText; //Elemento de UI para a fórmula

    public TMP_Text debugText; //Elemento de UI para debug (se necessário)

    private string formulaString = ""; //string para mostrar a fórmula que está sendo usada

    private double vagas = 0; //variável que guarda o valor calculado da fórmula

    private double vagasPorSegundo = 0; //variável que guarda o valor de vagas por segundo

    private bool isVagaSmall = false; //variável para saber se as vagas são pequenas (menos que 1000)

    private bool isVPSSmall = false; //variável para saber se as vagas por segund são pequenas (menos que 1000)

    private bool infinity = false;

    
    public double GetVagas() //Função para devolver o valor atual de vagas
    {
        return vagas;
    }

    public double GetNumBaseCost()
    {
        return numBaseUpgradeCost;
    }

    public double GetCoefficientCost()
    {
        return coefficientUpgradeCost;
    }

    public double GetExponentCost()
    {
        return exponentUpgradeCost;
    }

    public double GetFormulaCost()
    {
        return formulaUpgradeCost;
    }

    public double[] UpgradeBase() //Função para fazer o upgrade da base da fórmula  (handling feito no botão)
    {
        numBaseUpgradeLevel++;
        vagas -= numBaseUpgradeCost;
        numBase += numBaseIncrement;
        numBaseIncrement *= numBaseScaling;
        numBaseUpgradeCost *= numBaseUpgradeCostScaling;
        CalculateFormula();
        return new double[] { numBaseUpgradeCost, numBaseIncrement, numBaseUpgradeLevel }; //Devolve o custo do upgrade, o quanto ele vai aumentar e o nível atual do upgrade
    }

    public double[] UpgradeCoefficient() //Função para fazer o upgrade do coeficiente da fórmula  (handling feito no botão)
    {
        coefficientUpgradeLevel++;
        vagas -= coefficientUpgradeCost;
        coefficient *= coefficientScaling;
        coefficientUpgradeCost *= coefficientUpgradeCostScaling;
        CalculateFormula();
        return new double[] { coefficientUpgradeCost, coefficientScaling, coefficientUpgradeLevel }; //Devolve o custo do upgrade, o quanto ele vai aumentar e o nível atual do upgrade
    }

    public double[] UpgradeExponent() //Função para fazer o upgrade do exponente da fórmula  (handling feito no botão)
    {
        exponentUpgradeLevel++;
        vagas -= exponentUpgradeCost;
        exponent *= exponentScaling;
        exponentUpgradeCost *= exponentUpgradeCostScaling;
        CalculateFormula();
        return new double[] { exponentUpgradeCost, exponentScaling, exponentUpgradeLevel }; //Devolve o custo do upgrade, o quanto ele vai aumentar e o nível atual do upgrade
    }
    
    public double[] UpgradeFormula()
    {
        formulaType++;
        vagas -= formulaUpgradeCost;
        formulaUpgradeCost *= formulaUpgradeCostScaling;
        if (formulaType >= maxFormulaType) formulaUpgradeCost = 0; //se já estiver no nível máximo, o custo é 0. Fazer handler no botão
        CalculateFormula();
        return new double[] { formulaUpgradeCost, formulaType, maxFormulaType };
    }
    private void UpdateVagas()
    {
        float tempoPassado = Time.deltaTime; //tempo que passou desde o último frame
        vagas += vagasPorSegundo * tempoPassado;
        vagas = Math.Max(vagas, 0); //garante que vagas nunca fique negativa
        if(vagas < 1000)
        {
            isVagaSmall = true;
        }
        else
        {
            isVagaSmall = false;
        }
    }

    private void UpdateGUI()
    {
        string vagasStr;
        if (infinity)
        {
            vagasStr = "Infinito!!";
        }
        else
        {
            vagasStr = isVagaSmall ? vagas.ToString("0.##") : vagas.ToString("0.00e0");
        }
        string vpsStr = isVPSSmall ? vagasPorSegundo.ToString("0.##") + "/s" : vagasPorSegundo.ToString("0.00e0") + "/s";
        vagasText.text = $"Vagas: {vagasStr}";
        vagasPorSegundoText.text = $"Vagas por segundo: {vpsStr}";
        formulaText.text = $"Fórmula: {formulaString}";
    }
    private void CalculateFormula() //fução para fazer o calculo do valor da fórmula baseado em qual nível ela está
    {
        string numBaseStr = "";
        string coefficientStr = coefficient.ToString("0.##");
        string exponentStr = exponent.ToString("0.##");
        if (numBase >= 1000)
        {
            numBaseStr = numBase.ToString("0.00e0");
        }
        else
        {
            numBaseStr = numBase.ToString("0.##");
        }
        switch (formulaType)
        {
            case 0:
                formulaString = $"({numBaseStr}^{exponentStr}) * {coefficientStr}";
                vagasPorSegundo = Math.Pow(numBase, exponent) * coefficient;
                break;
            case 1:
                formulaString = $"({numBaseStr} * {coefficientStr})^{exponentStr}";
                vagasPorSegundo = Math.Pow(numBase * coefficient, exponent);
                break;
            case 2:
                formulaString = $"({numBaseStr} * {coefficientStr})^({exponentStr} * {coefficientStr})";
                vagasPorSegundo = Math.Pow(numBase * coefficient, exponent * coefficient);
                break;
            default:
                formulaString = $"({numBaseStr}^{exponentStr}) * {coefficientStr}";
                vagasPorSegundo = Math.Pow(numBase, exponent) * coefficient;
                break;
        }
        if (vagasPorSegundo < 1000)
        {
            isVPSSmall = true;
        }
        else
        {
            isVPSSmall = false;
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CalculateFormula();
        UpdateGUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVagas();
        UpdateGUI();
        if(Input.GetKeyDown(KeyCode.V))
        {
            OnDebugAddVagasClick();
        }
        if (vagas == double.PositiveInfinity && !infinity)
        {
            infinity = true;
            Instantiate(modalPrefab, modalSpawnPoint);
        }
    }
    
    public void OnDebugAddVagasClick()
    {
        vagas += double.MaxValue;
    }
}
