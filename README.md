# Testes arquiteturais com ArchUnitNET e xUnit

Um sistema de chamados pode funcionar e ainda violar sua arquitetura. Este exemplo educacional demonstra como automatizar cinco regras com ArchUnitNET.

## Duas versões do mesmo código

- `sem-testes-arquiteturais/`: quatro testes unitários passando, apesar das violações.
- `com-testes-arquiteturais/`: os mesmos quatro testes passando e cinco testes arquiteturais falhando intencionalmente.

Cada pasta contém uma solução independente `Tickets.slnx`. O código funcional, os testes unitários e o diagrama são idênticos. A segunda versão apenas adiciona o projeto `TestesArquiteturais` à solução.

## Organização

| Projeto | Responsabilidade |
|---|---|
| Apresentacao | API com controllers MVC: criar e listar chamados |
| Aplicacao | Serviço de criação e módulo simples de notificações |
| Dominio | Chamado com identificador e título |
| Infra | Repositório em memória e normalizador de título |
| TestesUnitarios | Comportamentos funcionais com xUnit |
| TestesArquiteturais | Regras com ArchUnitNET; somente na segunda versão |

A API usa controllers MVC para demonstrar literalmente a regra de controllers. Não há banco, autenticação ou envio real de notificações. Os dados são perdidos ao encerrar a API. A infraestrutura armazena identificadores e títulos sem depender do domínio, permitindo demonstrar a referência inversa indevida sem criar um ciclo entre projetos.

## Executar os testes

Requisito: SDK .NET 10 e acesso ao NuGet na primeira restauração. Na raiz:

```bash
dotnet test sem-testes-arquiteturais/Tickets.slnx
dotnet test com-testes-arquiteturais/Tickets.slnx
```

O primeiro comando deve passar (4 testes). O segundo deve terminar com código de saída diferente de zero: 4 testes unitários passam e 5 arquiteturais falham. Essas falhas são o resultado esperado da demonstração.

Para executar apenas os unitários da segunda versão:

```bash
dotnet test com-testes-arquiteturais/TestesUnitarios/TestesUnitarios.csproj
```

## As cinco regras

Os comentários `Violação` marcam os pontos relevantes nas duas versões. Os testes ficam em `com-testes-arquiteturais/TestesArquiteturais/ArchitectureTests.cs`.

| Nível / teste | Regra | Violação |
|---|---|---|
| Básico / Nivel1 | Serviços devem terminar com Service | `Aplicacao/Tickets/Services/TicketHandler.cs`: classe com sufixo Handler |
| Básico / Nivel2 | Domínio não pode depender da infraestrutura | `Dominio/Ticket.cs`: usa TicketTitleFormatter |
| Intermediário / Nivel3 | Controllers não podem acessar repositórios | `Apresentacao/TicketsController.cs`: listagem acessa TicketRepository |
| Intermediário / Nivel4 | Módulos não podem depender diretamente um do outro | `TicketHandler`: chama NotificationService |
| Avançado / Nivel5 | Dependências devem respeitar o diagrama | A mesma chamada entre módulos viola `arquitetura.puml` |

Os serviços são identificados pelo namespace, não pelo sufixo que queremos verificar. Os módulos Tickets e Notifications são namespaces dentro da aplicação, separados das quatro camadas técnicas.

O diagrama PlantUML declara os dois módulos sem seta entre eles: nenhuma dependência entre esses módulos é permitida. O quinto teste lê esse arquivo diretamente; não é necessário instalar PlantUML ou Java. Dependências externas aos componentes representados não são verificadas por essa regra. Os níveis 4 e 5 protegem a mesma fronteira por mecanismos diferentes.

## Executar a API

Escolha uma versão:

```bash
dotnet run --project sem-testes-arquiteturais/Apresentacao -- --urls http://localhost:5080
# Ou execute a outra versão, após parar a primeira:
dotnet run --project com-testes-arquiteturais/Apresentacao -- --urls http://localhost:5080
```

Em outro terminal:

```bash
curl -X POST http://localhost:5080/tickets -H 'Content-Type: application/json' -d '{"title":"  Impressora indisponível  "}'
curl http://localhost:5080/tickets
```

A criação retorna o identificador e o título sem espaços nas extremidades. A listagem retorna um objeto que associa identificadores aos títulos.

Testes unitários verificam comportamentos; testes arquiteturais verificam regras estruturais explicitamente definidas. Eles reduzem a necessidade de inspeção humana nesses cenários, mas não substituem a definição e evolução da arquitetura.

Referência: [guia oficial do ArchUnitNET](https://github.com/TNG/ArchUnitNET/blob/main/documentation/docs/guide.md).
