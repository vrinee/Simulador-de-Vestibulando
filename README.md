# Simulador-de-Vestibulando

A premissa do jogo é sobre um vestibulando que esta estudando para conseguir passar nos vestibulares mais concorridos, assim você adquire vagas que podem ser usadas para comprar materiais para melhorar seus estudos.

O jogo consiste de uma fórmula que pode ser modificada com upgrades e seus valores (base, coeficiente e expoente) que também podem ser aprimorados.

## Game Manager
Este é o gerenciador do jogo, aqui onde acontece a mágica que faz o jogo fucionar.

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

Agora podemos descomentar este código da linha 89:
```C#
/* public double GetNumBaseCost()
    {
        return numBaseUpgradeCost;
    }
 */
```
Este código realiza o que é chamado de GET, uma função que é chamada por outros objetos ou partes do código para adquirir um valor que é privado(embora neste caso seja público).