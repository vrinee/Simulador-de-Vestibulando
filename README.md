# Simulador-de-Vestibulando

A premissa do jogo é sobre um vestibulando que esta estudando para conseguir passar nos vestibulares mais concorridos, assim você adquire vagas que podem ser usadas para comprar materiais para melhorar seus estudos.

O jogo consiste de uma fórmula que pode ser modificada com upgrades e seus valores (base, coeficiente e expoente) que também podem ser aprimorados.

## Game Manager
Este é o gerenciador do jogo, aqui onde acontece a mágica que faz o jogo fucionar.
mexeremos no arquivo GameManager.cs

### Variáveis
Podemos começar declarando as variáveis que serão utilizadas para o calcúlo da base:
A partir da linha 19 se faz as mudanças
```c#
public double numBase = 1; //base da fórmula

public double numBaseIncrement = 1; //incremento da base da fórmula

public double numBaseScaling = 1.2; //fator de escala do incremento da base da fórmula

public double numBaseUpgradeCost = 1; //custo do upgrade da base da fórmula

public double numBaseUpgradeCostScaling = 1.1; //fator de escala do custo do upgrade da base da fórmula

public int numBaseUpgradeLevel = 0; //nível do upgrade da base da fórmula

```

### Funções

Agora podemos descomentar este código da linha 89:
```C#
/* public double GetNumBaseCost()
    {
        return numBaseUpgradeCost;
    }
 */
```
Este código realiza o que é chamado de GET, uma função que é chamada por outros objetos ou partes do código para adquirir um valor que é privado(embora neste caso seja público).
Neste projeto usaremos essa função para adquirir o valor do upgrade da base da fórmula no botão que faremos futuramente.

Neste momento podemos descomentar a grande seção de código da função CalculateFormula() que está encontrada na linha 180-206 (pode variar).

Dentro deste código vamos fazer o calculo da função:
Na linha 197 podemos criar um switch com três casos:
```C#
    Switch(formulaType){
        case 0:
        break;
        case 1:
        break;
        case 2:
        break;
        default:
        break;
    }
```
Isso é uma função nativa do C# que recebe um valor int e executa um código baseado no número recebido.
Em cada case(caso) será feito as fórmulas para calculo de vagas que mudará de acordo com o upgrade do tipo de vagas.
As fórmulas propostas são:

```C#
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
```

As fórmulas são respectivamente:
*   (Base ^ Expoente) * Coeficiente
*   (Base * Coeficiente) ^ Expoente
*   (Base * Coeficiente) ^ (Expoente * Coeficiente)
*   (Base ^ Expoente) * Coeficiente      Esta serve para caso o formulaType não se encaixe nos cases

Acima desta função tem várias funções nomeadas UpgradeXXXXXXXXXX(), descomente a seguinte linha de cada função e também da função Start():

```C#
    //CalculateFormula();
```

Agora decomente a função da linha 109:
```C#
    /*     public double[] UpgradeBase() //Função para fazer o upgrade da base da fórmula  (handling feito no botão)
    {
        
    } */
```
Esta função será usada pelo botão para realizar o upgrade na base da fórmula.
Usaremos as variaveis declaradas [aqui](#variáveis), para fazer o calculo aqui.
Teremos que aumentar o nível do upgrade, deduzir o custo do upgrade das suas vagas, aumentar o custo do próximo upgrade, incrementar a base, incrementar o incremento, e finalmente retornar os valores necessários para o botão (custo,incremento e nível).
Da seguinte forma:

```C#
    numBaseUpgradeLevel++; // Aumenta o nível do upgrade

    vagas -= numBaseUpgradeCost; // Deduz o custo do upgrade de suas vagas totais

    numBase += numBaseIncrement; // Adiciona o incremento na base, essencialmente realizando o Upgrade

    numBaseIncrement *= numBaseScaling; // Incrementa o incremento

    numBaseUpgradeCost *= numBaseUpgradeCostScaling; // Aumenta o preço do próximo upgrade

    CalculateFormula(); // Atualiza a fórmula agora que seus valores mudaram

    return new double[] { numBaseUpgradeCost, numBaseIncrement, numBaseUpgradeLevel }; //Devolve o custo do upgrade, o quanto ele vai aumentar e o nível atual do upgrade
```

Explicando brevemente o que cada coisa significa:

*   numBaseUpgradeLevel++: ele incrementa o valor da variavel por 1
*   -= : realiza a operação de retirar o valor à direita da variavel a esquerda
*   += : similar a anterior porém com soma
*   *= : novamente traz a ideia dos anteriores embora use multiplicação
*   CalculateFormula() : Chama a função para ela ser executada
*   return new double[] {...} : Realiza o retorno da função, retornando uma matriz do tipo double com os valores escolhidos

## baseUpgrade

Este é o código para o botão para o upgrade da base, onde será feito o controle do upgrade.
Será usado o arquivo baseUpgrade.cs

Começamos dentro da função PressButton alocando os valores da função de upgrade dentro de uma matriz:
```C#
    double[] handler = gameManager.UpgradeBase();//pega os valores da função de upgrade do gameManager
```

Agora iremos separar os valores do handler em suas devidas variaveis na linha 25:
```C#
    cost = handler[0];
    increment = handler[1];
    level = handler[2];
```

Na função Start() teremos que inicializar as variaveis principais do código:
```C#
        cost = gameManager.GetNumBaseCost(); // usa get para pegar o valor do upgrade
        increment = gameManager.numBaseIncrement; //usa acesso direto devido a ser public
        level = gameManager.numBaseUpgradeLevel;
```
Aqui fizemos a definição de duas formas, a primeira para o custo onde usamos o GET que criamos no gameManager, este sendo o ideal para qualquer sistema, mas também é usado na segunda e terceira variavel o acesso direto a variavel, não sendo recomendado para uso em outros sistemas.

## Mexendo na Unity

Agora iremos mexer na própria Unity, sendo que as instruções serão diretamente dadas pelo instrutor