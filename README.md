# MyGym API - Agendamento de Aulas

API RESTful desenvolvida em **ASP.NET Core 8** para gerenciar alunos, aulas e agendamentos em uma academia.  
Utiliza **SQLite** como banco de dados, **Entity Framework Core** para acesso aos dados e **AutoMapper** para mapeamento de DTOs.

---

## 📋 Funcionalidades

- Cadastro e gerenciamento de **Alunos**, com planos (`Mensal`, `Trimestral`, `Anual`)
- Cadastro e gerenciamento de **Aulas**, com tipo, data/hora e capacidade máxima
- Agendamento de alunos em aulas, respeitando:
  - Limite de aulas mensais conforme plano do aluno
  - Capacidade máxima da aula
- Geração de relatório mensal para cada aluno, com:
  - Total de aulas agendadas no mês
  - Tipos de aula mais frequentes
- Listagem de agendamentos por aluno ou por aula

---

## 🚀 Como rodar o projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 ou VS Code
- SQLite (não precisa instalar, será criado localmente)

### Passos

1. Clone este repositório ou copie os arquivos para sua máquina.
2. Configure a string de conexão em `appsettings.json` (exemplo já usa SQLite local):
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Data Source=academia.db"
      }
    }
    ```
3. Abra o terminal na pasta do projeto e crie as migrações:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```
4. Execute a API:
    ```bash
    dotnet run
    ```
5. Acesse o Swagger para documentação e testes interativos:
    ```
    https://localhost:{PORT}/swagger
    ```

---

## 🔌 Endpoints disponíveis

| Método | Endpoint                                  | Descrição                                  |
|--------|-------------------------------------------|--------------------------------------------|
| GET    | `/api/aluno`                             | Lista todos os alunos                      |
| GET    | `/api/aluno/{id}`                        | Consulta aluno por ID                      |
| POST   | `/api/aluno`                             | Cria novo aluno                           |
| PUT    | `/api/aluno/{id}`                        | Atualiza aluno                            |
| DELETE | `/api/aluno/{id}`                        | Remove aluno                             |
| GET    | `/api/aluno/{id}/relatorio`              | Relatório mensal do aluno                  |
| GET    | `/api/aula`                              | Lista todas as aulas                       |
| GET    | `/api/aula/{id}`                         | Consulta aula por ID                      |
| POST   | `/api/aula`                              | Cria nova aula                           |
| PUT    | `/api/aula/{id}`                         | Atualiza aula                            |
| DELETE | `/api/aula/{id}`                         | Remove aula                             |
| POST   | `/api/agendamento?alunoId={id}&aulaId={id}` | Agenda aluno em aula                      |
| GET    | `/api/agendamento/por-aluno/{alunoId}`   | Lista agendamentos do aluno                |
| GET    | `/api/agendamento/por-aula/{aulaId}`     | Lista agendamentos da aula                  |

---

## 📖 Exemplos de requisições JSON

### 1. Criar Aluno
**POST** `/api/aluno`

```json
{
  "nome": "Lucas",
  "plano": "Mensal"
}

```
### 2. Criar Aula
**POST** `/api/aluno`

```json
{
  "tipo": "Cross",
  "dataHora": "2025-07-05T10:00:00",
  "capacidadeMaxima": 10
}
```


### 3. Agendar Aluno na Aula
**POST** `/api/agendamento?alunoId=1&aulaId=1`

```json
{
  "mensagem": "Agendamento realizado com sucesso.",
  "agendamento": {
    "aluno": "Lucas",
    "plano": "Mensal",
    "aula": "Cross",
    "dataHora": "2025-07-05T10:00:00"
  }
}
```

### 4. Listar Agendamentos por Aluno
**GET** `/api/agendamento/por-aluno/1`

```json
[
  {
    "id": 1,
    "aluno": "Lucas",
    "aula": "Cross",
    "dataHora": "2025-07-05T10:00:00"
  }
]
```

### 5. Listar Agendamentos por Aula
**GET** `/api/agendamento/por-aula/1`

```json
[
  {
    "id": 1,
    "aluno": "Lucas",
    "aula": "Cross",
    "dataHora": "2025-07-05T10:00:00"
  }
]
```

### 6. Relatório Mensal do Aluno
**GET** `/api/aluno/1/relatorio`

```json
{
  "aluno": "Lucas Fabris",
  "plano": "Mensal",
  "totalDeAulasNoMes": 1,
  "tiposMaisFrequentes": [
    {
      "tipo": "Cross",
      "quantidade": 1
    }
  ]
}
```

## ⚙️ Regras de negócio importantes

- O aluno **não pode agendar mais aulas do que o permitido no seu plano**:
  - Mensal: até 12 aulas por mês
  - Trimestral: até 20 aulas por mês
  - Anual: até 30 aulas por mês
- A capacidade da aula **não pode ser ultrapassada**
- Um aluno pode estar agendado em várias aulas, desde que respeite os limites acima

---

## 🛠️ Tecnologias utilizadas

- C# com ASP.NET Core 8 Web API
- Entity Framework Core com SQLite
- AutoMapper para mapeamento DTO <-> Model
- Swagger para documentação automática da API

---

## 💡 Dicas para testes

- Utilize o Swagger UI para testar os endpoints e visualizar os exemplos
- Pode usar ferramentas como Postman ou Thunder Client para testes avançados
- Sempre crie alunos e aulas antes de fazer agendamentos
- Use os relatórios para validar os agendamentos feitos





