# SuperFastDB
##### English (Inglês)

Super fast database designed for bigdata that uses processing mechanisms like CPU, GPU and Hybrid.

##### Portuguese-Brazilian (Português-Brasileiro)
Banco de dados super rápido projetado para bigdata que utiliza mecanismos de processamentos como CPU, GPU e Híbrido.

Este projeto é um protótipo, ou seja, um conceito daquilo que servirá de base para transformá-lo em uma versão beta. No entanto, o objetivo é lançar este simplificado conceito com recursos extremamente limitados para não tardar o seu lançamento.

O contribuidor deve ter em mente que este projeto é altamente baseado em processamento paralelo devido as implementações futuras que temos em mente com processadores gráficos, a GPU, mas que também possa ser configurado para trabalhar em modo sequencial definido por um arquivo de configuração.

Tendo em mente que nosso objetivo é o processamento de dados em massa, bigdata, é proibido:
* O uso de técnicas recursivas devido o péssimo desempenho e alto consumo de memória feitas por alocação em pilhas (stack). O processamento sequencial, linha por linha, é indiscutivelmente mais rápido.
* O uso excessivo de métodos também deve ser reconsiderado pois afeta o desempenho, salvo quando não há alternativa.
		
Para atingirmos o melhor desempenho e estar sempre a frente de outros bancos de dados é necessário:
* Ter uma mentalidade voltada para a programação estruturada do que orientada a objetos, apesar de fazermos o uso dos dois paradigmas, deixando a orientada a objetos sempre em último caso quando não há alternativas.
* Sempre enxugar os bits, mantendo ou melhorando o alto desempenho.
	
Para tornar fácil o entendimento do código escrito e colaborar com os demais contribuidores, para que possam dar 
continuidade às implementações sem dificuldades, documente todos os seus métodos, funções e cada linha do seu código, mesmo que você considere ser evidentemente fácil de entender.

### O que é preciso ser feito?

Neste protótipo deverá ser codificado as seguintes solicitações SQL:

```sql
CREATE DATABASE [NovoBanco];
DELETE DATABASE [NovoBanco];
USE [NomeDoBanco];

CREATE TABLE [NovaTabela]([Coluna1] NovoTipo, [Coluna2] NovoTipo);
DELETE TABLE [Tabela];
TRUNCATE TABLE [Banco].[Tabela];

INSERT INTO [Tabela] VALUES ([valor1], [valor2]), ([valor1], [valor2]);

SELECT [Expressão];
SELECT [Expressão] AS [NomeColunaResultado];
SELECT [Expressão] FROM [Banco].[Tabela];
SELECT [Expressão] FROM [Banco].[Tabela] WHERE [Expressão];
SELECT Coluna1, Max(Coluna2) FROM [Banco].[Tabela] WHERE [Expressão] GROUP BY Coluna1


```

Onde **[Expressão]**, de modo geral, deverá ter a capacidade de realizar operações aritméticas, condicionais, atribuições e comparações, respeitando a precedência dos operadores praticadas pelos mecanismos de bancos de dados.

## Expressão

##### Artigos de estudo relacionados à expressão:
[Expressão na Computação](https://pt.wikipedia.org/wiki/Express%C3%A3o_(computa%C3%A7%C3%A3o))

[Atribuição na Computação](https://pt.wikipedia.org/wiki/Atribui%C3%A7%C3%A3o_(computa%C3%A7%C3%A3o))

[Associatividade de operadores](https://pt.wikipedia.org/wiki/Associatividade_de_operadores)

[Operador Lógico](https://pt.wikipedia.org/wiki/Operador_l%C3%B3gico)

[IBM: Operadores e Expressões](https://www.ibm.com/support/knowledgecenter/pt-br/SSL5ES_2.2.3/intel/ITM_portal/adminuse/terminal_scriptoperatorexpression_tep.html)

[Operador Ternário](https://blog.alura.com.br/o-que-e-o-operador-ternario/)

Operadores unários são operadores que trabalham apenas com um valor ao seu lado direito, por exemplo: --1. Temos um operador unário - e um número -1 do lado direito, isto resultará em um jogo de sinais: (-) com (-1) é igual a 1.

[Operadores Aritméticos Unários](https://docs.microsoft.com/pt-br/cpp/c-language/unary-arithmetic-operators?view=vs-2017)

Operador unário de negação é usado par inverter resultado de um valor booleano. Ex.: !(1 = 1) o resultado será false:

[Operador Unário de Negação](http://diegonascimentojava.blogspot.com/2013/09/operadores-unarios-negacao-pre-e-pos.html)

[Operadores Sobrecarregáveis](https://docs.microsoft.com/pt-br/dotnet/csharp/programming-guide/statements-expressions-operators/overloadable-operators)

[Precedência de Operadores do MySql](https://dev.mysql.com/doc/refman/5.6/en/operator-precedence.html)

[Precedência de Operadores do Sql Server 2017](https://docs.microsoft.com/pt-br/sql/t-sql/language-elements/operator-precedence-transact-sql?view=sql-server-2017)

[Notação Polonesa Inversa (ou *RPN* na sigla em inglês, de Reverse Polish Notation)](https://pt.wikipedia.org/wiki/Nota%C3%A7%C3%A3o_polonesa_inversa);

[Converter de **Notação infixa** para **Notação pós-fixa**](http://www.vision.ime.usp.br/~pmiranda/mac122_2s14/aulas/aula13/aula13.html)

[**Pilhas** e o cálculo do valor de uma expressão **posfixa**](https://www.ime.usp.br/~pf/mac0122-2002/aulas/stacks.html)

[Calculadora **RPN** em C#](http://www.math.bas.bg/bantchev/place/rpn/rpn.c%23.html)

[Calculadora RPN OnLine](http://www.alcula.com/calculators/rpn/)

[Exemplo C# Calculadora RPN](https://github.com/Igormandello/calculadora-csharp)

##### Como funciona?

A calculadora RPN irá **avaliar** a `[Expressão]` considerando a presença de: variáveis **'@var'** no escopo do comando sql, strings e números escritos entre aspas, números inteiros e decimais, operadores de precedência e funções.

[Site para fazer testes de expressão matemática](https://pt.numberempire.com/expressioncalculator.php)

```sql
Exemplo 1: 
SELECT 1 + 2 - 4 * 3; # Deverá retornar o valor -9

Exemplo 2:
SET @num = 23;
SELECT 1 + 2 - 4 + @num; # Deverá retornar o valor 25

Exemplo 3:
SELECT 1 + 2 - 4 = 3; # Deverá retornar os números 0 ou 1 (True ou False)
```

A *[Expressão]* deverá permitir uso de funções de string, numéricos, Data e Hora, Conversão e Fluxo de Controle como mostra a seguir:

##### Funções de String
São usados em todas as expressões avaliadas pela calculadora RPN. Tanto na instrução Select quanto na cláusula Where.
```sql
CONCAT(string1,string2,...)
INSERT(string,posicao,comprimento,nova_string)
LEFT(string,comprimento)
LTRIM(string)
REPLACE(string,de_str,para_str)
RIGTH(string,comprimento)
TRIM(string)
UCASE(string) e LOWER(string)
UPPER(string)
RTRIM(string)
SUBSTRING (expressão,pos_inicio,comprimento)  
```
##### Funções Numéricas
São usados em todas as expressões avaliadas pela calculadora RPN. Tanto na instrução Select quanto na cláusula Where.
```sql
ABS(número)
ACOS(número)
ASIN(número)
ATAN(número)
COS(número)
COT(número)
MOD(dividendo,divisor)
SIN(número)
SQRT(número)
TAN(número)
```
##### Funções de Data e Hora
São usados em todas as expressões avaliadas pela calculadora RPN. Tanto na instrução Select quanto na cláusula Where.
```sql
TIME()
DATE()
DAY()
MONTH(data)
YEAR(data)
HOUR(hora)
MINUTE(hora)
SECOND(hora)
```
##### Funções de Conversão
São usados em todas as expressões avaliadas pela calculadora RPN. Tanto na instrução Select quanto na cláusula Where.
```sql
CAST(valor AS novo_tipo)
CONVERT(valor, novo_tipo)
```
##### Funções de Fluxo de Controle
São usados em todas as expressões avaliadas pela calculadora RPN. Tanto na instrução Select quanto na cláusula Where.
```sql
IF()
IFNULL()
NULLIF()
CASE..WHEN..THEN..ELSE..END
```
##### Operadores de Comparação
São usados em todas as expressões avaliadas pela calculadora RPN. Tanto na instrução Select quanto na cláusula Where.
```sql
=, >, <, >=, <=, !=, <>
```
##### Operadores Lógicos exclusivos da cláusula Where
Usadas somente na cláusula Where, a calculadora RPN deverá esperar resultado condicional (0 ou 1):

```sql
AND, OR, IS, IN, NOT, LIKE e BETWEEN.
```

## Group By e Order By

##### Funções de Agregação 
```sql
MAX()
MIN()
SUM()
AVG()
COUNT()
ALIAS
```
