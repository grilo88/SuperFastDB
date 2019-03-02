# SuperFastDB
Super fast database designed for bigdata that uses processing mechanisms like CPU, GPU and Hybrid.
Banco de dados super rápido projetado para bigdata que utiliza mecanismos de processamentos como CPU, GPU e Híbrido.

Portuguese language (Língua Portuguesa): 
	Este projeto é um protótipo, ou seja, um conceito daquilo que servirá de base para transformá-lo em uma versão beta. No entanto, o objetivo é lançar este simplificado conceito com recursos extremamente limitados para não tardar o seu lançamento.
	O contribuidor deve ter em mente que este projeto é altamente baseado em processamento paralelo devido as implementações futuras que temos em mente com processadores gráficos, a GPU, mas que também possa ser configurado para trabalhar em modo sequencial definido por um arquivo de configuração.

	Tendo em mente que nosso objetivo é processamento de dados com bigdata, é terminantemente proibido:
		O uso de técnicas recursivas devido o péssimo desempenho e alto consumo de memória pois elas são alocadas em pilhas (stack). O processamento sequencial, linha por linha, é indiscutivelmente mais rápido.
		O uso excessivo de métodos deve ser altamente reconsiderado pois também afeta o desempenho.
		
	Para atingirmos os mais altos níveis de desempenho e estar a frente de todos os outros bancos de dados é necessário ter uma mentalidade voltada para a programação estruturada do que orientada a objetos, apesar de fazermos o uso dos dois paradigmas deixando a orientada a objetos em último caso quando não tiver alternativas.
	Enxugar os bits é o segredo para manter um excelente desempenho. 
	
	Para tornar fácil o entendimento do código escrito e colaborar com os demais contribuidores para que possam dar 
	continuidade as implementações sem dificuldades, documente todos os seus métodos, funções e cada linha do seu código, mesmo que você considere ser evidentemente fácil de compreender.
	
	Neste protótipo deverá ser codificado as seguintes solicitações SQL:

	SELECT [Expressão];
	SELECT [Expressão] AS [NomeColunaResultado];
	SELECT [Expressão] FROM [Banco].[Tabela];
	SELECT [Expressão] FROM [Banco].[Tabela] WHERE [Expressão];

	Onde [Expressão], de modo geral, deverá ter a capacidade de realizar operações aritméticas, condicionais, atribuições e , sempre respeitando a precedência dos operadores praticadas pelos bancos de dados.

	Artigos relacionados a este assunto:
	https://pt.wikipedia.org/wiki/Express%C3%A3o_(computa%C3%A7%C3%A3o)
	https://pt.wikipedia.org/wiki/Atribui%C3%A7%C3%A3o_(computa%C3%A7%C3%A3o)

	http://www.vision.ime.usp.br/~pmiranda/mac122_2s14/aulas/aula13/aula13.html
	https://www.ime.usp.br/~pf/mac0122-2002/aulas/stacks.html

	A [Expressão] deverá permitir a entrada de valores a partir de variáveis '@var' presentes no escopo do comando sql vindo de cima.
	A [Expressão] deverá permitir uso de funções como MAX(), MIN(), COALESCE,


	Onde [E