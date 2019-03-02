# SuperFastDB
Super fast database designed for bigdata that uses processing mechanisms like CPU, GPU and Hybrid.
Banco de dados super r�pido projetado para bigdata que utiliza mecanismos de processamentos como CPU, GPU e H�brido.

Portuguese language (L�ngua Portuguesa): 
	Este projeto � um prot�tipo, ou seja, um conceito daquilo que servir� de base para transform�-lo em uma vers�o beta. No entanto, o objetivo � lan�ar este simplificado conceito com recursos extremamente limitados para n�o tardar o seu lan�amento.
	O contribuidor deve ter em mente que este projeto � altamente baseado em processamento paralelo devido as implementa��es futuras que temos em mente com processadores gr�ficos, a GPU, mas que tamb�m possa ser configurado para trabalhar em modo sequencial definido por um arquivo de configura��o.

	Tendo em mente que nosso objetivo � processamento de dados com bigdata, � terminantemente proibido:
		O uso de t�cnicas recursivas devido o p�ssimo desempenho e alto consumo de mem�ria pois elas s�o alocadas em pilhas (stack). O processamento sequencial, linha por linha, � indiscutivelmente mais r�pido.
		O uso excessivo de m�todos deve ser altamente reconsiderado pois tamb�m afeta o desempenho.
		
	Para atingirmos os mais altos n�veis de desempenho e estar a frente de todos os outros bancos de dados � necess�rio ter uma mentalidade voltada para a programa��o estruturada do que orientada a objetos, apesar de fazermos o uso dos dois paradigmas deixando a orientada a objetos em �ltimo caso quando n�o tiver alternativas.
	Enxugar os bits � o segredo para manter um excelente desempenho. 
	
	Para tornar f�cil o entendimento do c�digo escrito e colaborar com os demais contribuidores para que possam dar 
	continuidade as implementa��es sem dificuldades, documente todos os seus m�todos, fun��es e cada linha do seu c�digo, mesmo que voc� considere ser evidentemente f�cil de compreender.
	
	Neste prot�tipo dever� ser codificado as seguintes solicita��es SQL:

	SELECT [Express�o];
	SELECT [Express�o] AS [NomeColunaResultado];
	SELECT [Express�o] FROM [Banco].[Tabela];
	SELECT [Express�o] FROM [Banco].[Tabela] WHERE [Express�o];

	Onde [Express�o], de modo geral, dever� ter a capacidade de realizar opera��es aritm�ticas, condicionais, atribui��es e , sempre respeitando a preced�ncia dos operadores praticadas pelos bancos de dados.

	Artigos relacionados a este assunto:
	https://pt.wikipedia.org/wiki/Express%C3%A3o_(computa%C3%A7%C3%A3o)
	https://pt.wikipedia.org/wiki/Atribui%C3%A7%C3%A3o_(computa%C3%A7%C3%A3o)

	http://www.vision.ime.usp.br/~pmiranda/mac122_2s14/aulas/aula13/aula13.html
	https://www.ime.usp.br/~pf/mac0122-2002/aulas/stacks.html

	A [Express�o] dever� permitir a entrada de valores a partir de vari�veis '@var' presentes no escopo do comando sql vindo de cima.
	A [Express�o] dever� permitir uso de fun��es como MAX(), MIN(), COALESCE,


	Onde [E