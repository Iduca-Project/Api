# Rotas de Cursos - API Iduca

## Estrutura de Relacionamentos

### Principais Entidades
- **User**: Usuário do sistema
- **Course**: Curso disponível
- **UserCourse**: Matrícula do usuário no curso (tabela de relacionamento)
- **Module**: Módulos do curso
- **Lesson**: Lições dentro dos módulos
- **UserLesson**: Progresso do usuário nas lições (tabela user_lesson)
- **Exercise**: Exercícios dos módulos
- **Question**: Questões dos exercícios/provas
- **Alternative**: Alternativas das questões
- **Category**: Categorias dos cursos
- **Company**: Empresa do usuário

## Rotas Implementadas

### 1. Rotas Básicas de Curso

#### GET /api/courses
- **Descrição**: Lista todos os cursos com paginação e filtros
- **Parâmetros**: `Page`, `MaxItems`, `Name`, `Difficulty`, `Categories[]`
- **Response**: Lista de cursos com informações básicas + número de estudantes

#### GET /api/courses/{id}
- **Descrição**: Busca um curso específico
- **Response**: Detalhes do curso + número de estudantes matriculados

#### POST /api/courses
- **Descrição**: Cria novo curso
- **Requer**: Admin
- **Body**: `Name`, `Description`, `Difficulty`, `Image`, `TotalHours`, `Categories[]`

#### PATCH /api/courses
- **Descrição**: Atualiza curso existente
- **Requer**: Admin
- **Body**: `Id`, `Name`, `Description`, `Difficulty`, `Image`, `TotalHours`, `Categories[]`

## Rotas a Implementar

### 2. Matrícula em Cursos

#### POST /api/courses/{courseId}/enroll
- **Descrição**: Matricula o usuário logado no curso
- **Requer**: Autenticação
- **Response**: Confirmação da matrícula
```json
{
  "userId": "guid",
  "courseId": "guid",
  "enrolledAt": "datetime",
  "message": "Matrícula realizada com sucesso"
}
```

#### DELETE /api/courses/{courseId}/unenroll
- **Descrição**: Remove matrícula do usuário no curso
- **Requer**: Autenticação
- **Response**: Confirmação da remoção

### 3. Cursos do Usuário com Progresso

#### GET /api/users/my-courses
- **Descrição**: Lista cursos em que o usuário está matriculado com progresso
- **Requer**: Autenticação
- **Response**: 
```json
{
  "courses": [
    {
      "courseId": "guid",
      "courseName": "string",
      "description": "string",
      "image": "string",
      "enrolledAt": "datetime",
      "progress": {
        "totalModules": 10,
        "completedModules": 3,
        "totalLessons": 50,
        "completedLessons": 15,
        "totalExercises": 25,
        "completedExercises": 8,
        "percentageComplete": 30.0,
        "lastAccessedLesson": {
          "lessonId": "guid",
          "lessonTitle": "string",
          "moduleTitle": "string"
        }
      },
      "estimatedTimeToComplete": "PT10H30M",
      "certificate": null
    }
  ]
}
```

#### GET /api/courses/{courseId}/my-progress
- **Descrição**: Progresso detalhado do usuário em um curso específico
- **Requer**: Autenticação + matrícula no curso
- **Response**:
```json
{
  "courseId": "guid",
  "courseName": "string",
  "enrolledAt": "datetime",
  "progress": {
    "modules": [
      {
        "moduleId": "guid",
        "moduleName": "string",
        "index": 1,
        "isCompleted": false,
        "lessons": [
          {
            "lessonId": "guid",
            "lessonTitle": "string",
            "isCompleted": true,
            "completedAt": "datetime"
          }
        ],
        "exercises": [
          {
            "exerciseId": "guid",
            "exerciseTitle": "string",
            "isCompleted": false,
            "score": null,
            "attemptCount": 0
          }
        ]
      }
    ],
    "overallProgress": {
      "percentageComplete": 35.5,
      "timeSpent": "PT5H20M",
      "estimatedTimeRemaining": "PT4H10M"
    }
  }
}
```

### 4. Estatísticas por Empresa e Categoria

#### GET /api/analytics/company-performance
- **Descrição**: Estatísticas de desempenho dos usuários da mesma empresa em uma categoria
- **Requer**: Autenticação
- **Parâmetros**: `categoryId` (obrigatório)
- **Response**:
```json
{
  "company": {
    "companyId": "guid",
    "companyName": "string"
  },
  "category": {
    "categoryId": "guid",
    "categoryName": "string"
  },
  "analytics": {
    "totalEmployees": 150,
    "employeesInCategory": 45,
    "courseStats": [
      {
        "courseId": "guid",
        "courseName": "string",
        "enrolledEmployees": 30,
        "completedEmployees": 18,
        "averageScore": 78.5,
        "averageCompletionTime": "PT15H30M",
        "topPerformers": [
          {
            "userId": "guid",
            "userName": "string",
            "score": 95.0,
            "completionTime": "PT12H15M",
            "completedAt": "datetime"
          }
        ],
        "strugglingEmployees": [
          {
            "userId": "guid",
            "userName": "string",
            "currentProgress": 25.0,
            "lastActivity": "datetime",
            "needsSupport": true
          }
        ]
      }
    ],
    "exercisePerformance": {
      "averageExerciseScore": 76.8,
      "mostDifficultExercises": [
        {
          "exerciseId": "guid",
          "exerciseTitle": "string",
          "courseName": "string",
          "averageScore": 45.2,
          "attemptCount": 156
        }
      ]
    },
    "examPerformance": {
      "averageExamScore": 81.3,
      "passRate": 85.5,
      "retakeRate": 22.1
    }
  }
}
```

#### GET /api/analytics/my-ranking
- **Descrição**: Ranking do usuário logado comparado com colegas da empresa na mesma categoria
- **Requer**: Autenticação
- **Parâmetros**: `categoryId` (obrigatório)
- **Response**:
```json
{
  "userRanking": {
    "position": 8,
    "totalParticipants": 45,
    "percentile": 82.2,
    "userScore": 88.5,
    "companyAverage": 78.5
  },
  "topRanking": [
    {
      "position": 1,
      "userName": "string",
      "score": 96.8,
      "coursesCompleted": 5,
      "isCurrentUser": false
    }
  ],
  "nearbyRanking": [
    {
      "position": 7,
      "userName": "string", 
      "score": 89.2,
      "isCurrentUser": false
    },
    {
      "position": 8,
      "userName": "João Silva",
      "score": 88.5,
      "isCurrentUser": true
    },
    {
      "position": 9,
      "userName": "string",
      "score": 87.1,
      "isCurrentUser": false
    }
  ]
}
```

### 5. Rotas de Gestão de Progresso

#### POST /api/lessons/{lessonId}/complete
- **Descrição**: Marca uma lição como concluída
- **Requer**: Autenticação + matrícula no curso
- **Response**: Confirmação + atualização do progresso

#### POST /api/exercises/{exerciseId}/submit
- **Descrição**: Submete respostas de um exercício
- **Requer**: Autenticação + matrícula no curso
- **Body**: Array de respostas
- **Response**: Score + feedback + progresso atualizado

## Implementação Prioritária

1. **POST /api/courses/{courseId}/enroll** - Matrícula em curso
2. **GET /api/users/my-courses** - Cursos com progresso básico  
3. **POST /api/lessons/{lessonId}/complete** - Marcar lição como concluída
4. **GET /api/courses/{courseId}/my-progress** - Progresso detalhado
5. **GET /api/analytics/company-performance** - Estatísticas da empresa

## Notas Técnicas

### Cálculo de Progresso
- **Progresso por Módulo**: (Lições concluídas + Exercícios concluídos) / (Total de lições + Total de exercícios)
- **Progresso Geral**: Média ponderada dos módulos (considerando peso por número de lições/exercícios)
- **Critério de Conclusão**: Módulo considerado completo quando todas as lições foram assistidas e todos exercícios foram aprovados

### Performance
- Usar índices nas tabelas `user_lesson`, `user_course` 
- Cache de estatísticas de empresa (atualizar a cada 1 hora)
- Paginação obrigatória para listagens grandes

### Segurança
- Usuário só pode ver progresso de cursos em que está matriculado
- Estatísticas da empresa só são visíveis para usuários da mesma empresa
- Dados sensíveis (scores individuais) são anonimizados nas estatísticas gerais
