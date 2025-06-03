### [ Modelagem ](https://drive.google.com/file/d/1eU9bKm7yb8rmziR_P50M0-yaoEkZrAoN/view?usp=drive_link)
### [ Caso de uso ](https://excalidraw.com/#room=e304a1b783b96bd97dea,zUE57N0YvdqGjWg7izMN3w)
### [ Miro ](https://miro.com/welcomeonboard/dEFaTTZVSngyTmhBWXdZWlQ2bWJrcjJ0RGVpQUpucUVKTkRLTGVYQ1RrRVNJMzJhZm15SVllQkpDSEVhR3NETGFtQmcyWStSMFNMSllpbGRiaTRYSXR2L2djMUlVVXBnd1pJVW9KTzZIVmdyeXBMUGZFMFNTL21BL2xMOVBYMFFQdGo1ZEV3bUdPQWRZUHQzSGl6V2NBPT0hdjE=?share_link_id=93045832121)

---

# Endpoints Aluno:

> Os Perfis de Aluno, Manager e Admin são separados, não são o mesmo perfil! Se uma pessoa tem permissão de Manager, ela só tem acesso aos recursos do Manager, não tem acesso aos recursos de Aluno!

## 🔐 Login/forgotPass:

### 📌 POST /auth/login
Realiza o login do usuário com e-mail corporativo e senha.

**Exemplo**
📥 Request Body

```
{
  "email": "usuario@empresa.com",
  "password": "senha123"
}
```

📤 Response - Login comum
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "firstAccess": false
}
```

📤 Response - Primeiro acesso
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "firstAccess": true
}
```

### 📌 POST /auth/forgotPass
Envia um código de 5 dígitos para o e-mail corporativo do usuário para recuperação de senha.

📥 Request Body
```
{
  "email": "usuario@empresa.com"
}
```

📤 Response
```
{
  "response": true
}
```

### 📌 POST /auth/checkCode
Verifica se o código enviado ao e-mail está correto.

📥 Request Body
```
{
  "email": "usuario@empresa.com",
  "code": "12345"
}
```

📤 Response - Código válido
```
{
  "response": true
}
```

📤 Response - Código inválido
```
{
  "response": false
}
```

### 📌 POST /auth/resendCode
Reenvia o código de 5 dígitos para o e-mail corporativo do usuário.

📥 Request Body
```
{
  "email": "usuario@empresa.com"
}
```

📤 Response
```
{
  "response": true
}
```

### 📌 POST /auth/resetPassword
Redefine a senha do usuário após a verificação do código.

📥Request Body
```
{
  "email": "usuario@empresa.com",
  "newPassword": "senhaNova123"
}
```

📥Response
```
{
  "response": true
}
```

## 🏠 Home (Todos precisam do token):

### 📌 GET /home/progress
Retorna o progresso geral do usuário nos cursos. Requer o token de autenticação no header.

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response Aluno
```
{
  "username": "João da Silva",
  "isManager": false,
  "isAdmin": false,
  "totalCourses": 10,
  "ongoingCourses": 4,
  "completeCourses": 6,
  "percenteGeneral": 60
}
```
📤 Response Gestor
```
{
  "username": "João da Silva",
  "isManager": true,
  "isAdmin": false,
  "totalEmployees": 10,
  "totalCourses": 25,
  "totalRegistrations": 8,
  "completionRate": 60
}
```

📤 Response Admin
```
{
  "username": "João da Silva",
  "isManager": false,
  "isAdmin": true,
  "não_sei_ainda": 10
}
```


### 📌 GET /home/coursesInProgress
Retorna até 8 cursos que o usuário está fazendo atualmente.

Difficulty:
1 - Iniciante
2 - Intermediário
3 - Avançado

🔐 Headers
```
Authorization: Bearer {token}
```


📤 Response
```
[
  {
    "id": 1,
    "title": "Lógica de Programação",
    "image": "https://cdn.exemplo.com/curso1.png",
    "progress": 40,
    "description": "Python é uma das linguagens que mais cresce no Mercado de Trabalho e atualmente uma das mais usadas e requisitadas pelas empresas.",
    "rating": 4.8,
    "participants": 157,
    "difficulty": 2,
    "category": "Programação"
  },
  {
    "id": 2,
    "title": "Banco de Dados",
    "image": "https://cdn.exemplo.com/curso2.png",
    "progress": 75,
    "description": "Python é uma das linguagens que mais cresce no Mercado de Trabalho e atualmente uma das mais usadas e requisitadas pelas empresas.",
    "rating": 4.8,
    "participants": 157,
    "difficulty": 2,
    "category": "Programação"
  }
]
```

### 📌 GET /home/calendar
Retorna os lembretes do usuário e as datas de prazos de atividades/provas.

Type:
- 1 - Lembrete do usuário
- 2 - Atividade
- 3 - Prova

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
[
  {
    "date": "2025-05-15",
    "type": 1,
    "description": "Estudar para a prova de C#"
  },
  {
    "date": "2025-05-17",
    "type": 2,
    "description": "Prazo final da atividade de Banco de Dados"
  },
  {
    "date": "2025-05-20",
    "type": 3,
    "description": "Prova de Node.js"
  }
]
```

## 📚 Cursos (Todos precisam de token):

### 📌 GET /courses
Retorna a lista paginada de cursos, com suporte a busca por nome, filtro por categoria, filtro por dificuldade e paginação (12 cursos por página).

🔐 Headers
```
Authorization: Bearer {token}
```

🧾 Query Params

| Parâmetro     | Tipo   | Descrição                                   |
| ------------- | ------ | ------------------------------------------- |
| `page`        | number | Número da página (ex: 1, 2, 3...)           |
| `search`      | string | Nome do curso para busca (opcional)         |
| `category`   | string | Categoria do curso (opcional)               |
| `difficulty` | number | Dificuldade do curso (1, 2 ou 3) (opcional) |


📤 Response
```
{
  "currentPage": 1,
  "totalPages": 3,
  "courses": [
    {
      "id": 2,
      "title": "Banco de Dados",
      "image": "https://cdn.exemplo.com/curso2.png",
      "progress": 75,
      "description": "Python é uma das linguagens que mais cresce no Mercado de Trabalho e atualmente uma das mais usadas e requisitadas pelas empresas.",
      "rating": 4.8,
      "participants": 157,
      "difficulty": 2,
      "category": "Programação"
    },
    {
      "id": 5,
      "title": "Git e GitHub",
      "image": "https://cdn.exemplo.com/curso5.png",
      "progress": 20,
      "description": "Aprenda a versionar seus projetos com Git e GitHub do zero.",
      "rating": 4.6,
      "participants": 98,
      "difficulty": 1,
      "category": "Ferramentas"
    }
  ]
}
```

### 📌 GET /categories
Retorna a lista de categorias disponíveis para o usuário escolher.

📤 Response
```
[
  { "id": 1, "name": "Programação" },
  { "id": 2, "name": "UX/UI" },
  { "id": 3, "name": "DevOps" },
  { "id": 4, "name": "Gestão" },
  { "id": 5, "name": "Banco de Dados" },
  { "id": 6, "name": "Inteligência Artificial" },
  { "id": 7, "name": "Mecânica" }
]
```


## 🗓️ Calendário

### 📌 GET /calendar
Retorna todos os eventos do usuário em até 1 ano (6 meses antes e 6 meses depois do dia atual): lembretes, prazos de atividades e provas.

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
[
  {
    "date": "2025-05-15",
    "type": 1,
    "description": "Estudar para a prova de C#"
  },
  {
    "date": "2025-05-17",
    "type": 2,
    "description": "Prazo final da atividade de Banco de Dados"
  },
  {
    "date": "2025-05-20",
    "type": 3,
    "description": "Prova de Node.js"
  }
]
```

### 📌 GET /calendar/next
Retorna os eventos dos próximos 7 dias (lembretes + prazos + provas).

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
[
  {
    "date": "2025-05-15",
    "type": 1,
    "description": "Estudar para a prova de C#"
  },
  {
    "date": "2025-05-17",
    "type": 2,
    "description": "Prazo final da atividade de Banco de Dados"
  }
]
```

### 📌 POST /calendar/reminder
Permite ao usuário adicionar um lembrete pessoal. Todos os lembretes criados manualmente terão type: 1.

🔐 Headers
```
Authorization: Bearer {token}
```

📥 Request Body
```
{
  "title": "Estudar para a prova",
  "date": "2025-05-18"
}
```

📤 Response
```
{
  "response": true
}
```

## 👤 Perfil (Todos precisam de token):

### 📌 GET /profile
Retorna todas as informações do usuário logado.

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
{
  "photoUser": "https://cdn.exemplo.com/perfil/usuario123.png",
  "name": "Sabrina Mortean",
  "email": "sabrina@empresa.com",
  "interests": ["Programação", "UX/UI", "Banco de Dados"],
  "completedCourses": 4,
  "averageTest": 8.7,
  "completedCoursesList": [
    {
      "id": 1,
      "title": "Node.js Avançado",
      "image": "https://cdn.exemplo.com/cursos/node.png",
      "certificateAvailable": true
    },
    {
      "id": 2,
      "title": "Banco de Dados",
      "image": "https://cdn.exemplo.com/cursos/bd.png",
      "certificateAvailable": true
    }
  ]
}
```

🔹 Se o usuário não tiver interesses, o array "interests" vem vazio: []

### 📌 GET /certificate/:id/image
Retorna a imagem do certificado de um curso finalizado.

🔐 Headers
```
Authorization: Bearer {token}
```

🔁 Params
> :id = ID do curso

📤 Response
```
{
  response: "certificado.png"
}
```
> image/png ou image/jpeg (para exibir direto no "&lt;img src="..." /&gt;")

### 📌 GET /certificate/:id/pdf
Retorna o PDF do certificado para download.

🔐 Headers
```
Authorization: Bearer {token}
```

🔁 Params
> :id = ID do curso

📤 Response
```
{
  response: "certificado.pdf"
}
```

## ✏️ Editar Perfil (Todos precisam de token, menos o get de interesses):


### 📌 GET /interests
Retorna a lista de interesses disponíveis para o usuário escolher (máximo de 5 na hora de salvar).

📤 Response
```
[
  { "id": 1, "name": "Programação" },
  { "id": 2, "name": "UX/UI" },
  { "id": 3, "name": "DevOps" },
  { "id": 4, "name": "Gestão" },
  { "id": 5, "name": "Banco de Dados" },
  { "id": 6, "name": "Inteligência Artificial" },
  { "id": 7, "name": "Mecânica" }
]
```

### 📌 PUT /profile
Permite que o usuário edite sua foto de perfil e/ou seus interesses (até 5).

🔐 Headers
```
Authorization: Bearer {token}
```

📥 Request Body

- photoUser (opcional): arquivo de imagem (.jpg, .png, etc.)
- interests (opcional): array de strings com até 5 interesses, mas podem ser menos de 5

> O usuário pode mandar apenas a foto, apenas os interesses, ou os dois.

📤 Exemplo
```
{
  "photoUser": arquivo.png/.jpg
  "interests": [1, 2, 4, 6, 7]
}
```

📤 Response
```
{
  "response": true
}
```

> ⚠️ Se forem enviados mais de 5 interesses, deve retornar um erro!


## 📘 Detalhes do curso (Todos precisam de token):

### 📌 GET /courses/:id
Retorna as informações gerais de um curso + lista de módulos.

Type:
- 1 - Aula escrita
- 2 - Aula em vídeo
- 3 - Atividade múltipla escolha
- 4 - Atividade PDF

🔐 Headers
```
Authorization: Bearer {token}
```

🔁 Params:
> :id = ID do curso

📤 Response:
```
{
  "id": 5,
  "title": "Git and GitHub",
  "image": "https://cdn.exemplo.com/curso5.png",
  "description": "Learn how to version your projects with Git and GitHub from scratch.",
  "rating": 4.6,
  "participants": 98,
  "progress": 20,
  "difficulty": 1,
  "duration": "10:00:00",
  "category": "Tools",
  "haveExam": true,
  "modules": [
    {
      "id": 1,
      "title": "Getting Started with Git",
      "description": "Understand what Git is and how to start using it.",
      "content": [
        {
          "id": 101,
          "type": 1,
          "title": "Introduction to Git",
          "completed": true
        },
        {
          "id": 102,
          "type": 2,
          "title": "Installing Git",
          "completed": false
        },
        {
          "id": 103,
          "type": 3,
          "title": "Practice: Git Init",
          "completed": false
        },
        {
          "id": 104,
          "type": 4,
          "title": "Git Basics Test",
          "completed": false
        }
      ]
    },
    {
      "id": 2,
      "title": "Working with GitHub",
      "description": "Push your code to GitHub and collaborate with others.",
      "content": [
        {
          "id": 201,
          "type": 1,
          "title": "Creating a GitHub Repository",
          "completed": false
        },
        {
          "id": 202,
          "type": 2,
          "title": "Cloning and Pulling",
          "completed": false
        }
      ]
    }
  ]
}
```

> Se o usuário não tiver iniciado o curso, o "Progress" fica em 0

## 📘 Aulas (Todos precisam de token):

### 📌 GET /lessons/:id
Retorna os dados completos de uma aula (escrita ou em vídeo) + info sobre a próxima aula (se tiver).

🔐 Headers
```
Authorization: Bearer {token}
```
🔁 Params
> :id = ID da aula

Type da aula escrita:
- 1 - Texto
- 2 - Imagem

📤 Response (Se for aula de texto):
```
{
  "id": 101,
  "type": 1,
  "title": "Introduction to Git",
  "courseId": 5,
  "completed": true,
  "content": [
    {
      "type": 1,
      "title": "Git x Github",
      "value": "Git is a distributed version control system..."
    },
    {
      "type": 2,
      "value": "https://cdn.exemplo.com/git-example.png"
    },
    {
      "type": 1,
      "title": "Pessoas usando o github",
      "value": "It allows multiple people to collaborate on code..."
    }
  ],
  "nextLesson": {
    "id": 102,
    "type": 2,
    "title": "Installing Git"
  }
}
```

> Se não tiver próxima aula, retorna nextLesson como false


## 🧠 Se for vídeo (type 2):

```
{
  "id": 101,
  "type": 2,
  "title": "Introduction to Git",
  "courseId": 5,
  "completed": true,
  "content": [
    {
      "type": 2,
      "value": "https://cdn.exemplo.com/videos/lesson102.mp4"
    }
  ]
  "nextLesson": {
    "id": 102,
    "type": 2,
    "title": "Installing Git"
  }
}
```
## 🧠 Se for atividade de múltipla escolha (type 3):

```
{
"id": 1,
"type": 3,
"title": "Prática: Git",
"courseId": 5,
completed: false,
"content": [
    {
      "id": 1,
      "question": "What is the command to initialize a Git repository?",
      "options": [
        { "id": "1", "text": "git start", "alternative": "a" },
        { "id": "2", "text": "git init", "alternative": "b" },
        { "id": "3", "text": "git begin", "alternative": "c" }
      ]
    },
    {
      "id": 2,
      "question": "What file tracks your commits?",
      "options": [
        { "id": "1", "text": ".git/config", "alternative": "a" },
        { "id": "2", "text": ".gitignore", "alternative": "b" },
        { "id": "3", "text": ".git", "alternative": "c" }
      ]
    }
  }
}
```
## 🧠 Se for atividade de PDF (type 4):

```
{
  "id": 108,
  "type": 3,
  "title": "Upload your Git Project",
  "courseId": 5,
  "type": 4,
  "completed": false,
  "description": "Send a PDF explaining how you initialized and committed your project using Git."
}
```

### 📌 POST /activities/:id/submitQuiz

Para enviar as respostas do usuário nas atividades de múltipla escolha.

📥 Request Body
```
{
  "answers": [
    { "questionId": 1, "selectedOptionId": "b" },
    { "questionId": 2, "selectedOptionId": "c" }
  ]
}
```
📤 Response:
```
{
    "response": true
}
```
### 📌 POST /activities/:id/upload
📥 Request Body
> file: PDF enviado pelo usuário

📤 Response:
```
{
    "response": true
}
```

### 📌 GET /test/:id

> :id é o id do Curso!

Para pegar as questões e alternativas de uma prova.

```
{
"id": 1,
"title": "Prova Final",
"courseId": 5,
completed: false,
"content": [
    {
      "id": 1,
      "question": "What is the command to initialize a Git repository?",
      "options": [
        { "id": "1", "text": "git start", "alternative": "a" },
        { "id": "2", "text": "git init", "alternative": "b" },
        { "id": "3", "text": "git begin", "alternative": "c" }
      ]
    },
    {
      "id": 2,
      "question": "What file tracks your commits?",
      "options": [
        { "id": "1", "text": ".git/config", "alternative": "a" },
        { "id": "2", "text": ".gitignore", "alternative": "b" },
        { "id": "3", "text": ".git", "alternative": "c" }
      ]
    }
  }
}
```


# Endpoints Manager:

## 🧑‍💼 Manager - Dashboard

### 📌 GET /manager/dashboard
Retorna os dados principais da tela inicial do manager, com informações sobre o progresso do time e gráficos de desempenho.

>✅ Regras de acesso
>🔒 Apenas usuários com permissão de manager podem acessar.


>⚠️ O backend identifica o manager logado pelo token JWT e traz as informações com base no time dele.

📤 Response:
```
{
  "totalEmployees": 12,
  "totalCourses": 25,
  "totalEnrollments": 47,
  "completionRate": 73,
  "performanceByCategory": [
    { "category": "Programming", "score": 46 },
    { "category": "Design", "score": 89 },
    { "category": "Management", "score": 35 }
  ],
  "courseStatus": {
    "completed": 59,
    "inProgress": 29,
    "notStarted": 2
  }
}
```


## 🧑‍🏫 Manager - Cursos e Inscrições

### 📌 GET /manager/team
Retorna a lista dos colaboradores do time do manager logado.

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
[
  {
    "id": 21,
    "name": "Ana Costa",
    "email": "ana.costa@empresa.com"
  },
  {
    "id": 22,
    "name": "João Lima",
    "email": "joao.lima@empresa.com"
  }
]
```

### 📌 GET /manager/courses-status?employeeId={id}
Retorna a lista de cursos com status de inscrição de um colaborador específico (útil para o manager saber se a pessoa já fez o curso ou não).

Status
- 1 - Completo
- 2 - Em progresso
- 3 - Não iniciado

📤 Response
```
[
  {
    "courseId": 2,
    "title": "Banco de Dados",
    "status": 1
  },
  {
    "courseId": 5,
    "title": "Git e GitHub",
    "status": 3
  }
]
```

📌 POST /manager/enroll
Inscreve um colaborador em um curso.

🔐 Headers
```
Authorization: Bearer {token}
```

📥 Body
```
{
  "employeeId": 22,
  "courseId": 5
}
```

📤 Response
```
{
  "response": True
}
```


### 💡 Sistema de Competência por Categoria
> Pra não deixar o sistema injusto (tipo uma pessoa faz um curso iniciante e já é 100% mestre em "Programação"), vamos montar uma média ponderada por categoria com base na dificuldade dos cursos feitos + desempenho.

Cada curso tem uma pontuação:

- Dificuldade 1 → peso 1
- Dificuldade 2 → peso 2
- Dificuldade 3 → peso 3

A pontuação do colaborador em uma categoria seria:
```
(nota1 × peso1 + nota2 × peso2 + ...) / (peso1 + peso2 + ...)
```

E aí você gera uma barra de “nível de competência” pra cada categoria. Exemplo:
```
[
  {
    "category": "Programação",
    "competenceLevel": 82 // Média ponderada dos cursos feitos nessa categoria
  },
  {
    "category": "UX/UI",
    "competenceLevel": 47
  }
]
```


## 🧑‍💼 Tela de Lista de Colaboradores
Aqui é a listinha geral. A ideia é ser tipo uma visão resumida que ajude o manager a entender rapidamente quem tá indo bem, quem tá travado e onde precisa agir.

### 📌 GET /manager/employeesSummary
🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
[
  {
    "id": 21,
    "name": "Ana Costa",
    "email": "ana@empresa.com",
    "coursesCompleted": 4,
    "coursesInProgress": 2,
    "averageScore": 87, // Média das notas dos cursos
    "topCategory": "Programação", // Onde ela tem melhor desempenho
  },
  {
    "id": 22,
    "name": "João Lima",
    "email": "joao@empresa.com",
    "coursesCompleted": 1,
    "coursesInProgress": 1,
    "averageScore": 59,
    "topCategory": "DevOps",
  }
]
```

## 📊 Tela do Colaborador Específico (Dashboard)
Quando o manager clica em um colaborador, abre um dashboard com tudo que ele precisa saber sobre aquela pessoa. Sugestão do que pode ter:

### 📌 GET /manager/employee/{id}/dashboard

📤 Response
```
{
  "employeeId": 21,
  "name": "Ana Costa",
  "email": "ana@empresa.com",
  "competencies": [
    { "category": "Programação", "competenceLevel": 85 },
    { "category": "UX/UI", "competenceLevel": 60 },
    { "category": "Gestão", "competenceLevel": 45 }
  ],
  "courses": {
    "completed": [
      {
        "title": "Banco de Dados",
        "category": "Programação",
        "difficulty": 2,
        "score": 92
      }
    ],
    "inProgress": [
      {
        "title": "Gestão de Projetos",
        "category": "Gestão",
        "difficulty": 2,
        "progress": 40
      }
    ],
    "notStarted": [
      {
        "title": "Introdução ao Figma",
        "category": "UX/UI",
        "difficulty": 1
      }
    ]
  },
  "averageScore": 87,
  "totalCourses": 7,
  "coursesCompleted": 4
}
```

## 📤 Exportação de Relatórios
### 📌 GET /manager/export
Gera um relatório com os dados dos colaboradores e permite o download em formato .pdf ou .xlsx.

🔐 Headers
```
Authorization: Bearer {token}
```

🧾 Query Params

| Parâmetro     | Tipo   | Descrição                                   |
| ------------- | ------ | ------------------------------------------- |
| `format`        | string | Formato do relatório: "pdf" ou "xlsx"        |
| `teamId`      | number | ID do time (opcional, se o manager gerencia mais de um time)       |


## ✅ Exemplo

### 📌 GET /manager/export?format=pdf
📤 Response (PDF ou Excel)
Arquivo para download com o relatório do time

💡 O que vai no relatório?

- Nome do colaborador
- Cursos feitos
- Progresso (%)
- Notas finais
- Competências por categoria (se tiver)
- Média geral


## 🧑‍💼 Gerenciamento de Colaboradores
### 📌 POST /manager/employees
Cria um novo colaborador no sistema, vinculado ao time do manager autenticado.

🔐 Headers
```
Authorization: Bearer {token}
```

📥 Body
```
{
  "id": 123456  // Id do funcionário na empresa
  "name": "João Silva",
  "email": "joao@empresa.com",
  "isManager": False
}
```

📤 Response
```
{
  "message": "Employee created successfully",
  "employeeId": 45
}
```

# Endpoints Admin:

## 🏢 Empresas (Admin)

### 📌 GET /admin/companies
Lista todas as empresas cadastradas no sistema.

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
{
  "companies": [
    {
      "id": 1,
      "name": "Empresa Alfa",
    },
    {
      "id": 2,
      "name": "Beta Ltda",
    }
  ]
}
```

### 📌 POST /admin/companies
Cadastra uma nova empresa.

🔐 Headers
```
Authorization: Bearer {token}
```
📥 Body
```
{
  "name": "Empresa Gama",
}
```

📤 Response
```
{
  "message": "Company created successfully",
  "companyId": 3
}
```

### 📌 DELETE /admin/companies/{companyId}
Deleta uma empresa (e no backend eles cuidam da exclusão dos funcionários vinculados).

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
{
  "message": "Company and related employees deleted successfully"
}
```

## 📚 Cursos (Admin)

### 📌 POST /admin/course
Cria um curso novo

```
{
  // Dados básicos do curso
  "title": "Git and GitHub",              // Nome do curso
  "image": "https://cdn.exemplo.com/curso5.png",  // Imagem/banner do curso
  "description": "Aprenda a versionar seus projetos com Git e GitHub do zero.",  // Descrição geral
  "difficulty": 1,                        // Dificuldade: 1 - fácil, 2 - médio, 3 - difícil
  "category": "Ferramentas",             // Categoria (deve ser uma das disponíveis no GET /categories)
  "duration": "10:00:00",                // Tempo estimado para conclusão (HH:MM:SS)
  "haveExam": true,                      // Se o curso tem prova final ou não

  // Lista de módulos do curso (cada módulo pode ter várias aulas e atividades)
  "modules": [
    {
      "title": "Introdução ao Git",       // Nome do módulo
      "description": "Entenda o que é o Git e como começar a usar.",  // Descrição do módulo

      // Conteúdo do módulo - aula ou atividade, cada item com tipo:
      // 1 = Aula escrita (texto/imagem)
      // 2 = Aula em vídeo
      // 3 = Atividade múltipla escolha
      // 4 = Atividade PDF (upload)
      "content": [
        {
          "type": 1,                     // Aula escrita
          "title": "Introdução ao Git", // Título da aula
          "content": [                  // Lista de blocos que compõem essa aula
            {
              "type": 1,                // Bloco de texto
              "title": "O que é Git?", // Título do bloco
              "value": "Git é um sistema de controle de versão distribuído..."  // Texto do bloco
            },
            {
              "type": 2,                // Bloco de imagem
              "value": "https://cdn.exemplo.com/git-exemplo.png"  // URL da imagem
            },
            {
              "type": 1,                // Outro bloco de texto
              "title": "Por que usar Git?",
              "value": "Git permite colaborar em projetos com várias pessoas..."
            }
          ]
        },
        {
          "type": 2,                   // Aula em vídeo
          "title": "Instalando o Git",
          "content": [
            {
              "type": 2,              // Link para o vídeo
              "value": "https://cdn.exemplo.com/videos/instalando-git.mp4"
            }
          ]
        },
        {
          "type": 3,                   // Atividade de múltipla escolha
          "title": "Prática: Git Init",
          "content": [
            {
              "id": 1,                // ID da pergunta, pode ser gerado pelo backend
              "question": "Qual comando inicializa um repositório Git?",
              "options": [
                { "id": "a", "text": "git start" },
                { "id": "b", "text": "git init" },
                { "id": "c", "text": "git begin" }
              ]
            },
            {
              "id": 2,
              "question": "Qual arquivo rastreia seus commits?",
              "options": [
                { "id": "a", "text": ".git/config" },
                { "id": "b", "text": ".gitignore" },
                { "id": "c", "text": ".git" }
              ]
            }
          ]
        },
        {
          "type": 4,                   // Atividade PDF
          "title": "Envie seu projeto Git",
          "description": "Envie um PDF explicando como você iniciou e comitou seu projeto usando Git."
          // O arquivo PDF será enviado depois em endpoint específico (upload)
        }
      ]
    },
    {
      "title": "Trabalhando com GitHub",
      "description": "Envie seu código para o GitHub e colabore com outras pessoas.",
      "content": [
        {
          "type": 1,
          "title": "Criando um repositório no GitHub",
          "content": [
            {
              "type": 1,
              "title": "Passo a passo",
              "value": "Primeiro, crie uma conta no GitHub..."
            }
          ]
        },
        {
          "type": 2,
          "title": "Clonando e puxando repositórios",
          "content": [
            {
              "type": 2,
              "value": "https://cdn.exemplo.com/videos/clonando-github.mp4"
            }
          ]
        }
      ]
    }
  ]
}
```

### 📌 POST para cadastrar uma prova (tipo atividade múltipla escolha)
```
{
  // ID do curso que essa prova pertence (pode ser passado na URL também)
  "courseId": "abc123",

  // Título da prova
  "title": "Prova Final - Git e GitHub",

  // Lista de perguntas da prova (mesmo formato da atividade múltipla escolha)
  "questions": [
    {
      "id": 1, // Pode ser gerado automaticamente se quiser
      "question": "Qual comando cria um novo repositório Git?",
      "options": [
        { "id": "a", "text": "git init" },
        { "id": "b", "text": "git start" },
        { "id": "c", "text": "git create" }
      ]
    },
    {
      "id": 2,
      "question": "Para que serve o arquivo .gitignore?",
      "options": [
        { "id": "a", "text": "Ignorar arquivos no commit" },
        { "id": "b", "text": "Armazenar configurações do repositório" },
        { "id": "c", "text": "Listar colaboradores do projeto" }
      ]
    }
  ]
}
```

### 📌 DELETE /admin/course/{idCourse}
Deleta um curso.

🔐 Headers
```
Authorization: Bearer {token}
```

📤 Response
```
{
  "message": "Course deleted successfully"
}
```

## 🧑‍💼 Criar Manager e Vincular Empresa

### 📌 POST /admin/managers
Cria um manager novo e vincula a uma empresa já cadastrada.

🔐 Headers
```
Authorization: Bearer {token}
```

📥 Body
```
{
  "id" 123456 // Id do funcionário na empresa
  "name": "Lucas Andrade",
  "email": "lucas@empresa.com",
}
```

📤 Response
```
{
  "message": "Manager created and linked to company successfully",
  "managerId": 15
}
```
