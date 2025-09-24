using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{

    public bool debugMode = false; //modo de debug para testes

    [Header("Base da Fórmula")]
    public double numBase = 1;//base da fórmula

    public double numBaseIncrement = 1; //incremento da base da fórmula

    public double numBaseScaling = 1.1; //fator de escala do incremento da base da fórmula

    public double numBaseUpgradeCost = 10; //custo do upgrade da base da fórmula

    public double numBaseUpgradeCostScaling = 1.15; //fator de escala do custo do upgrade da base da fórmula

    public int numBaseUpgradeLevel = 0; //nível do upgrade da base da fórmula

    [Header("Coeficiente da Fórmula")]
    public double coefficient = 1;//coeficiente da fórmula

    public double coefficientScaling = 1.1; //fator de escala do coeficiente da fórmula

    public double coefficientScalingUpgrade = 1.05; //fator de escala do incremento do coeficiente da fórmula

    public double coefficientUpgradeCost = 50; //custo do upgrade do coeficiente da fórmula

    public double coefficientUpgradeCostScaling = 1.15; //fator de escala do custo do upgrade do coeficiente da fórmula

    public int coefficientUpgradeLevel = 0; //nível do upgrade do coeficiente da fórmula

    [Header("Exponente da Fórmula")]
    public double exponent = 1;//exponente da fórmula

    public double exponentScaling = 1.1; //fator de escala do exponente da fórmula

    public double exponentUpgradeCost = 100; //custo do upgrade do exponente da fórmula

    public double exponentUpgradeCostScaling = 1.15; //fator de escala do custo do upgrade do exponente da fórmula

    public int exponentUpgradeLevel = 0; //nível do upgrade do exponente da fórmula

    [Header("Tipo da Fórmula")]
    public int formulaType = 0; // tipo/nível da fórmula que está sendo usada

    public int maxFormulaType = 3; // número máximo de tipos/níveis de fórmulas

    public double formulaUpgradeCost = 1000; // custo para fazer o upgrade da fórmula

    public double formulaUpgradeCostScaling = 5; // fator de escala do custo para fazer o upgrade da fórmula

    [Header("Elementos da UI")]
    public TMP_Text vagasText; //Elemento de UI para a quantidade de vagas

    public TMP_Text vagasPorSegundoText; //Elemento de UI para a quantidade de vagas por segundo

    public TMP_Text formulaText; //Elemento de UI para a fórmula

    public TMP_Text debugText; //Elemento de UI para debug (se necessário)

    private string formulaString = ""; //string para mostrar a fórmula que está sendo usada

    private double vagas = 0; //variável que guarda o valor calculado da fórmula

    private double vagasPorSegundo = 0; //variável que guarda o valor de vagas por segundo

    private bool isSmall = false; //variável para saber se as vagas são pequenas (menos que 1000)

    public double GetVagas() //Função para devolver o valor atual de vagas
    {
        return vagas;
    }

    public double[] UpgradeBase() //Função para fazer o upgrade da base da fórmula  (handling feito no botão)
    {
        numBaseUpgradeLevel++;
        double cost = numBaseUpgradeCost;
        vagas -= cost;
        numBase += numBaseIncrement;
        numBaseIncrement *= numBaseScaling;
        numBaseUpgradeCost *= numBaseUpgradeCostScaling;
        CalculateFormula();
        return new double[] { cost, numBaseIncrement, numBaseUpgradeLevel }; //Devolve o custo do upgrade, o quanto ele vai aumentar e o nível atual do upgrade
    }

    public double[] UpgradeCoefficient() //Função para fazer o upgrade do coeficiente da fórmula  (handling feito no botão)
    {
        coefficientUpgradeLevel++;
        double cost = coefficientUpgradeCost;
        vagas -= cost;
        coefficient *= coefficientScaling;
        coefficientScaling *= coefficientScalingUpgrade;
        coefficientUpgradeCost *= coefficientUpgradeCostScaling;
        CalculateFormula();
        return new double[] { cost, coefficientScaling, coefficientUpgradeLevel }; //Devolve o custo do upgrade, o quanto ele vai aumentar e o nível atual do upgrade
    }

    public double[] UpgradeExpoent() //Função para fazer o upgrade do exponente da fórmula  (handling feito no botão)
    {
        exponentUpgradeLevel++;
        double cost = exponentUpgradeCost;
        vagas -= cost;
        exponent *= exponentScaling;
        exponentUpgradeCost *= exponentUpgradeCostScaling;
        CalculateFormula();
        return new double[] { cost, exponentScaling, exponentUpgradeLevel }; //Devolve o custo do upgrade, o quanto ele vai aumentar e o nível atual do upgrade
    }
    
    public double[] UpgradeFormula()
    {
        formulaType++;
        double cost = formulaUpgradeCost;
        vagas -= cost;
        formulaUpgradeCost *= formulaUpgradeCostScaling;
        if (formulaType >= maxFormulaType) cost = 0; //se já estiver no nível máximo, o custo é 0. Fazer handler no botão
        CalculateFormula();
        return new double[] { cost, formulaType, maxFormulaType };
    }
    private void UpdateVagas()
    {
        float tempoPassado = Time.deltaTime; //tempo que passou desde o último frame
        debugText.text = $"Debug: {tempoPassado.ToString("0.0000")}";
        vagas += vagasPorSegundo * tempoPassado;
        vagas = Math.Max(vagas, 0); //garante que vagas nunca fique negativa
        if(vagas < 1000 && isSmall)
        {
            isSmall = true;
        }
        else if (vagas >= 1000 && isSmall)
        {
            isSmall = false;
        }
    }

    private void UpdateGUI()
    {
        string vagasStr = isSmall ? vagas.ToString("0.##") : vagas.ToString("0.00e0");
        vagasText.text = $"Vagas: {vagasStr}";
        vagasPorSegundoText.text = $"Vagas por segundo: {vagasPorSegundo.ToString("0.00e0")}";
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
            case 3:
                formulaString = $"({numBaseStr} * {coefficientStr})^{exponentStr}";
                vagasPorSegundo = Math.Pow(numBase * coefficient, exponent * exponent);
                break;
            default:
                formulaString = $"({numBaseStr}^{exponentStr}) * {coefficientStr}";
                vagasPorSegundo = Math.Pow(numBase, exponent) * coefficient;
                break;
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
        if (!debugMode) return;
        if (Input.GetKeyDown(KeyCode.V))
        {
            vagas += 1000;
            return;
        }

        if (Input.GetKeyDown(KeyCode.B)){
            UpgradeBase();
            return;
        }

        if (Input.GetKeyDown(KeyCode.C)){
            UpgradeCoefficient();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E)){
            UpgradeExpoent();
            return;
        }

        if (Input.GetKeyDown(KeyCode.F)){
            UpgradeFormula();
            return;
        }

        //adicionar coisas para debug (ex: apertar teclas para ganhar vagas, etc)
    }
}
