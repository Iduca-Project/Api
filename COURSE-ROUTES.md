# Documentação das Rotas de Curso e Analytics - API Iduca

## Estrutura de Relacionamentos

### Principais Entidades
- **User**: Usuário do sistema
- **Course**: Curso disponível
- **UserCourse**: Matrícula do usuário no curso (tabela de relacionamento)
- **Module**: Módulos do curso
- **Lesson**: Lições dentro dos módulos
- **Exercise**: Exercícios dos módulos
- **Category**: Categorias dos cursos
- **Company**: Empresa do usuário

## 🟢 Rotas Implementadas

### 1. Rotas Básicas de Curso

#### GET /api/courses/all
- **Descrição**: Lista todos os cursos com paginação e filtros
- **Autenticação**: Requerida (Admin ou Usuário)
- **Parâmetros Query**:
  - `name` (string, opcional): Filtrar por nome do curso
  - `difficulty` (int, opcional): Filtrar por nível de dificuldade (1-5)
  - `category` (array de GUIDs, opcional): Filtrar por categorias
  - `page` (int, padrão: 1): Página atual
  - `maxItems` (int, padrão: 10): Itens por página

#### GET /api/courses/{id}
- **Descrição**: Obter detalhes de um curso específico
- **Autenticação**: Requerida (Admin ou Usuário)
- **Parâmetros**: `id` (GUID): ID do curso

#### POST /api/courses
- **Descrição**: Criar um novo curso
- **Autenticação**: Requerida (Admin)
- **Body**:
```json
{
  "name": "Nome do Curso",
  "description": "Descrição do curso",
  "difficulty": 3,
  "image": "url-da-imagem",
  "totalHours": 40,
  "categories": ["guid-categoria-1", "guid-categoria-2"]
}
```

#### PATCH /api/courses/{id}
- **Descrição**: Atualizar um curso existente
- **Autenticação**: Requerida (Admin)
- **Parâmetros**: `id` (GUID): ID do curso
- **Body**: Campos opcionais para atualização

#### DELETE /api/courses/{id}
- **Descrição**: Excluir um curso (soft delete)
- **Autenticação**: Requerida (Admin)
- **Parâmetros**: `id` (GUID): ID do curso

### 2. Rotas de Matrícula e Progresso

#### POST /api/courses/{courseId}/enroll
- **Descrição**: Matricular usuário em um curso
- **Autenticação**: Requerida (Admin ou Usuário)
- **Parâmetros**: `courseId` (GUID): ID do curso
- **Body**:
```json
{
  "userId": "guid-do-usuario"
}
```
- **Response**:
```json
{
  "courseId": "guid-do-curso",
  "userId": "guid-do-usuario",
  "enrolledAt": "2024-01-01T10:00:00Z",
  "message": "Usuário matriculado com sucesso no curso!",
  "initialProgress": {
    "totalModules": 5,
    "completedModules": 0,
    "totalLessons": 25,
    "completedLessons": 0,
    "totalExercises": 10,
    "completedExercises": 0,
    "percentageComplete": 0.0,
    "lastAccessedLesson": null
  }
}
```

#### GET /api/users/my-courses
- **Descrição**: Listar cursos do usuário com progresso
- **Autenticação**: Requerida (Admin ou Usuário)
- **Query Parameters**: `userId` (GUID): ID do usuário (obrigatório)
- **Response**:
```json
{
  "courses": [
    {
      "courseId": "guid-do-curso",
      "courseName": "Nome do Curso",
      "description": "Descrição do curso",
      "image": "url-da-imagem",
      "enrolledAt": "2024-01-01T10:00:00Z",
      "progress": {
        "totalModules": 5,
        "completedModules": 2,
        "totalLessons": 25,
        "completedLessons": 15,
        "totalExercises": 10,
        "completedExercises": 5,
        "percentageComplete": 60.0,
        "lastAccessedLesson": {
          "lessonId": "guid-da-licao",
          "lessonTitle": "Título da Lição",
          "moduleTitle": "Título do Módulo"
        }
      },
      "estimatedTimeToComplete": "PT40H",
      "certificate": null
    }
  ]
}
```

#### POST /api/lessons/{lessonId}/complete
- **Descrição**: Marcar uma lição como concluída
- **Autenticação**: Requerida (Admin ou Usuário)
- **Parâmetros**: `lessonId` (GUID): ID da lição
- **Body**:
```json
{
  "userId": "guid-do-usuario"
}
```
- **Response**:
```json
{
  "lessonId": "guid-da-licao",
  "userId": "guid-do-usuario",
  "completedAt": "2024-01-01T10:30:00Z",
  "message": "Lição concluída com sucesso!",
  "newProgressPercentage": 65.0
}
```

### 3. Rotas de Analytics (Somente Admin)

#### GET /api/analytics/companies/{companyId}
- **Descrição**: Obter estatísticas detalhadas de uma empresa
- **Autenticação**: Requerida (Admin)
- **Parâmetros**: `companyId` (GUID): ID da empresa
- **Query Parameters**:
  - `startDate` (DateTime, opcional): Data inicial para filtro
  - `endDate` (DateTime, opcional): Data final para filtro
- **Response**:
```json
{
  "companyId": "guid-da-empresa",
  "companyName": "Nome da Empresa",
  "statistics": {
    "totalUsers": 150,
    "totalCourses": 25,
    "totalEnrollments": 450,
    "completedCourses": 200,
    "averageCompletionRate": 75.5,
    "totalHoursLearned": 1200,
    "activeUsers": 89
  },
  "courseBreakdown": [
    {
      "courseId": "guid-do-curso",
      "courseName": "Nome do Curso",
      "enrollments": 45,
      "completions": 32,
      "completionRate": 71.1,
      "averageProgress": 85.2,
      "totalHours": 40
    }
  ],
  "categoryBreakdown": [
    {
      "categoryId": "guid-da-categoria",
      "categoryName": "Tecnologia",
      "coursesCount": 8,
      "enrollments": 180,
      "popularityPercentage": 40.0
    }
  ],
  "generatedAt": "2024-01-01T12:00:00Z"
}
```

#### GET /api/analytics/categories/{categoryId}
- **Descrição**: Obter estatísticas detalhadas de uma categoria
- **Autenticação**: Requerida (Admin)
- **Parâmetros**: `categoryId` (GUID): ID da categoria
- **Query Parameters**:
  - `startDate` (DateTime, opcional): Data inicial para filtro
  - `endDate` (DateTime, opcional): Data final para filtro
- **Response**:
```json
{
  "categoryId": "guid-da-categoria",
  "categoryName": "Tecnologia",
  "analytics": {
    "totalCourses": 15,
    "totalEnrollments": 320,
    "completedEnrollments": 240,
    "averageCompletionRate": 75.0,
    "totalHoursContent": 600,
    "uniqueUsers": 180,
    "popularityRank": 85.5
  },
  "topCourses": [
    {
      "courseId": "guid-do-curso",
      "courseName": "Nome do Curso",
      "enrollments": 65,
      "completions": 52,
      "completionRate": 80.0,
      "averageRating": 4.5,
      "totalHours": 40
    }
  ],
  "companyBreakdown": [
    {
      "companyId": "guid-da-empresa",
      "companyName": "Nome da Empresa",
      "enrollments": 120,
      "uniqueUsers": 85,
      "completionRate": 78.3
    }
  ],
  "generatedAt": "2024-01-01T12:00:00Z"
}
```

## Códigos de Resposta

- **200 OK**: Requisição bem-sucedida
- **201 Created**: Recurso criado com sucesso
- **400 Bad Request**: Dados inválidos na requisição
- **401 Unauthorized**: Token de autenticação inválido ou ausente
- **403 Forbidden**: Usuário não tem permissão para a operação
- **404 Not Found**: Recurso não encontrado
- **409 Conflict**: Conflito (ex: usuário já matriculado no curso)
- **500 Internal Server Error**: Erro interno do servidor

## Headers Obrigatórios

Todas as rotas protegidas requerem o header:
```
Authorization: Bearer {jwt-token}
```

## Observações Importantes

1. **Progresso**: O cálculo de progresso é baseado no número de lições completadas em relação ao total de lições do curso
2. **Módulos Completados**: Um módulo é considerado completo quando todas as suas lições foram completadas
3. **Matrícula**: Um usuário só pode se matricular uma vez em cada curso
4. **Analytics**: As rotas de analytics são exclusivas para administradores e fornecem insights detalhados sobre engajamento e performance
5. **Filtros de Data**: Nas rotas de analytics, os filtros de data se aplicam à data de matrícula dos usuários
6. **Última Lição Acessada**: Refere-se à última lição que o usuário completou (não apenas visualizou)

## ⚠️ Funcionalidades em Desenvolvimento

### Rotas Futuras (Não Implementadas)
- DELETE /api/courses/{courseId}/unenroll - Cancelar matrícula
- GET /api/courses/{courseId}/my-progress - Progresso detalhado por módulo
- POST /api/exercises/{exerciseId}/submit - Submeter exercícios
- GET /api/analytics/my-ranking - Ranking do usuário na empresa
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
