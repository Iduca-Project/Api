# 📖 Documentação das Rotas da API Iduca

## 🔐 Autenticação

### Credenciais Padrão do Admin
- **Email:** `admin@iduca.com`
- **Senha:** `admin123`

### POST /api/auth/login
**Descrição:** Fazer login de usuário  
**Autenticação:** Não requerida

**Request:**
```json
{
  "email": "admin@iduca.com",
  "password": "admin123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "firstAccess": true
}
```

**Response (400 Bad Request):**
```json
{
  "message": "Usuário não encontrado ou credenciais inválidas"
}
```

---

## 👥 Usuários

> **Nota:** Todas as rotas de usuário requerem autenticação com token Bearer

### POST /api/users
**Descrição:** Criar novo usuário  
**Autenticação:** Bearer Token (Apenas Admins)

**Request:**
```json
{
  "name": "João Silva",
  "identity": "12345678901",
  "email": "joao.silva@exemplo.com",
  "password": "123456",
  "isAdmin": false,
  "responsibleId": null,
  "companyId": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "image": "https://example.com/avatar.jpg",
  "interests": [
    "3f2504e0-4f89-11d3-9a0c-0305e82c3302",
    "3f2504e0-4f89-11d3-9a0c-0305e82c3303"
  ]
}
```

**Notas:**
- `companyId`: GUID da empresa (obrigatório)
- `responsibleId`: GUID do responsável (opcional, pode ser null)
- `interests`: Array de GUIDs das categorias de interesse (pode ser array vazio)
- `isAdmin`: boolean indicando se é administrador

**Response (201 Created):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3304",
  "name": "João Silva",
  "identity": "12345678901",
  "email": "joao.silva@exemplo.com",
  "isAdmin": false,
  "responsibleId": null,
  "companyId": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "image": "https://example.com/avatar.jpg",
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/users/{id}
**Descrição:** Buscar usuário por ID  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3304",
  "name": "João Silva",
  "identity": "12345678901",
  "email": "joao.silva@exemplo.com",
  "isAdmin": false,
  "responsibleName": null,
  "companyName": "Empresa Exemplo",
  "image": "https://example.com/avatar.jpg",
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/users/all
**Descrição:** Listar usuários com filtros e paginação  
**Autenticação:** Bearer Token (Apenas Admins)

**Query Parameters:**
- `Name` (opcional): Filtrar por nome
- `Email` (opcional): Filtrar por email
- `CompanyId` (opcional): Filtrar por empresa
- `IsAdmin` (opcional): Filtrar por tipo de usuário
- `Page` (padrão: 1): Página atual
- `MaxItems` (padrão: 10): Itens por página

**Exemplo:** `/api/users/all?Name=João&IsAdmin=false&Page=1&MaxItems=10`

**Response (200 OK):**
```json
{
  "users": [
    {
      "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3304",
      "name": "João Silva",
      "identity": "12345678901",
      "email": "joao.silva@exemplo.com",
      "isAdmin": false,
      "responsibleName": null,
      "companyName": "Empresa Exemplo",
      "image": "https://example.com/avatar.jpg",
      "createdAt": "2025-07-02T14:30:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "maxItems": 10
}
```

### PUT /api/users
**Descrição:** Atualizar usuário  
**Autenticação:** Bearer Token (Apenas Admins)

**Request:**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3304",
  "name": "João Silva Atualizado",
  "identity": "12345678901",
  "email": "joao.silva.novo@exemplo.com",
  "isAdmin": false,
  "responsibleId": null,
  "companyId": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "image": "https://example.com/new-avatar.jpg",
  "interests": []
}
```

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3304",
  "name": "João Silva Atualizado",
  "identity": "12345678901",
  "email": "joao.silva.novo@exemplo.com",
  "isAdmin": false,
  "responsibleName": null,
  "companyName": "Empresa Exemplo",
  "image": "https://example.com/new-avatar.jpg",
  "updatedAt": "2025-07-02T15:30:00Z"
}
```

### DELETE /api/users/{id}
**Descrição:** Deletar usuário (soft delete)  
**Autenticação:** Bearer Token (Apenas Admins)

**Response (204 No Content)**

---

## 📂 Categorias

### POST /api/categories
**Descrição:** Criar nova categoria  
**Autenticação:** Bearer Token

**Request:**
```json
{
  "name": "Tecnologia"
}
```

**Response (201 Created):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3305",
  "name": "Tecnologia",
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/categories/{id}
**Descrição:** Buscar categoria por ID  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3305",
  "name": "Tecnologia",
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/categories/all
**Descrição:** Listar todas as categorias  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
[
  {
    "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3305",
    "name": "Tecnologia",
    "createdAt": "2025-07-02T14:30:00Z"
  },
  {
    "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3306",
    "name": "Negócios",
    "createdAt": "2025-07-02T14:31:00Z"
  }
]
```

### GET /api/categories/search
**Descrição:** Buscar categoria por nome  
**Autenticação:** Bearer Token

**Query Parameters:**
- `Name` (obrigatório): Nome da categoria para buscar

**Exemplo:** `/api/categories/search?Name=Tecnologia`

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3305",
  "name": "Tecnologia",
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### DELETE /api/categories/{id}
**Descrição:** Deletar categoria  
**Autenticação:** Bearer Token

**Response (200 OK)**

---

## 🏢 Empresas

### POST /api/company
**Descrição:** Criar nova empresa  
**Autenticação:** Bearer Token

**Request:**
```json
{
  "name": "Empresa Exemplo Ltda",
  "description": "Descrição da empresa"
}
```

**Response (201 Created):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "name": "Empresa Exemplo Ltda",
  "description": "Descrição da empresa",
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/company
**Descrição:** Buscar empresa por nome  
**Autenticação:** Bearer Token

**Query Parameters:**
- `Name` (obrigatório): Nome da empresa para buscar

**Exemplo:** `/api/company?Name=Empresa%20Exemplo`

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "name": "Empresa Exemplo Ltda",
  "description": "Descrição da empresa",
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/company/{id}
**Descrição:** Buscar empresa por ID  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "name": "Empresa Exemplo Ltda",
  "description": "Descrição da empresa",
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/company/all
**Descrição:** Listar todas as empresas  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
[
  {
    "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
    "name": "Empresa Exemplo Ltda",
    "description": "Descrição da empresa",
    "createdAt": "2025-07-02T14:30:00Z"
  }
]
```

### PATCH /api/company/{id}
**Descrição:** Atualizar empresa  
**Autenticação:** Bearer Token

**Request:**
```json
{
  "name": "Empresa Exemplo Ltda - Atualizada",
  "description": "Nova descrição da empresa"
}
```

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
  "name": "Empresa Exemplo Ltda - Atualizada",
  "description": "Nova descrição da empresa",
  "updatedAt": "2025-07-02T15:30:00Z"
}
```

### DELETE /api/company/{id}
**Descrição:** Deletar empresa  
**Autenticação:** Bearer Token

**Response (204 No Content)**

---

## 📚 Cursos

### POST /api/course
**Descrição:** Criar novo curso  
**Autenticação:** Bearer Token

**Request:**
```json
{
  "name": "Curso de Programação",
  "description": "Aprenda programação do zero",
  "difficulty": 1,
  "image": "https://example.com/curso-imagem.jpg",
  "totalHours": 40,
  "categories": [
    "3f2504e0-4f89-11d3-9a0c-0305e82c3305",
    "3f2504e0-4f89-11d3-9a0c-0305e82c3306"
  ]
}
```

**Notas:**
- `difficulty`: 0 = Iniciante, 1 = Intermediário, 2 = Avançado
- `categories`: Array de GUIDs das categorias associadas
- `companyId` não é necessário - o curso será associado automaticamente

**Response (201 Created):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3307",
  "name": "Curso de Programação",
  "createdAt": "2025-07-02T14:30:00Z",
  "updatedAt": "2025-07-02T14:30:00Z",
  "disabledAt": null
}
```

### GET /api/course/{id}
**Descrição:** Buscar curso por ID  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
{
  "name": "Curso de Programação",
  "description": "Aprenda programação do zero",
  "difficulty": 1,
  "image": "https://example.com/curso-imagem.jpg",
  "totalHours": 40,
  "students": 15,
  "categories": [
    {
      "name": "Tecnologia"
    },
    {
      "name": "Programação"
    }
  ]
}
```

### GET /api/course/all
**Descrição:** Listar cursos com filtros e paginação  
**Autenticação:** Bearer Token

**Query Parameters:**
- `Name` (opcional): Filtrar por nome
- `Difficulty` (opcional): Filtrar por dificuldade (0=Iniciante, 1=Intermediário, 2=Avançado)
- `Page` (padrão: 1): Página atual
- `MaxItems` (padrão: 10): Itens por página

**Exemplo:** `/api/course/all?Page=1&MaxItems=10&Name=Programação&Difficulty=1`

**Response (200 OK):**
```json
{
  "courses": [
    {
      "name": "Curso de Programação",
      "description": "Aprenda programação do zero",
      "difficulty": 1,
      "image": "https://example.com/curso-imagem.jpg",
      "totalHours": 40,
      "students": 15,
      "categories": [
        {
          "name": "Tecnologia"
        }
      ]
    }
  ]
}
```

### PATCH /api/course
**Descrição:** Atualizar curso  
**Autenticação:** Bearer Token

**Request:**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3307",
  "name": "Curso de Programação Avançada",
  "description": "Aprenda programação avançada",
  "difficulty": 2,
  "image": "https://example.com/novo-curso-imagem.jpg",
  "totalHours": 60,
  "categories": [
    "3f2504e0-4f89-11d3-9a0c-0305e82c3305"
  ]
}
```

**Response (200 OK):**
```json
{
  "name": "Curso de Programação Avançada",
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3307",
  "updatedAt": "2025-07-02T15:30:00Z",
  "categories": [
    {
      "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3305",
      "name": "Tecnologia"
    }
  ],
  "modules": [],
  "students": 15
}
```

### DELETE /api/course/{id}
**Descrição:** Deletar curso  
**Autenticação:** Bearer Token

**Response (204 No Content)**

---

## 📖 Módulos

### POST /api/modules
**Descrição:** Criar novo módulo  
**Autenticação:** Bearer Token

**Request:**
```json
{
  "name": "Módulo 1: Introdução",
  "description": "Módulo introdutório do curso",
  "courseId": "3f2504e0-4f89-11d3-9a0c-0305e82c3307",
  "order": 1
}
```

**Response (201 Created):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3308",
  "name": "Módulo 1: Introdução",
  "description": "Módulo introdutório do curso",
  "courseName": "Curso de Programação",
  "order": 1,
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/modules/{id}
**Descrição:** Buscar módulo por ID  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3308",
  "name": "Módulo 1: Introdução",
  "description": "Módulo introdutório do curso",
  "courseName": "Curso de Programação",
  "order": 1,
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/modules/course/{courseId}
**Descrição:** Buscar módulos por curso  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
[
  {
    "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3308",
    "name": "Módulo 1: Introdução",
    "description": "Módulo introdutório do curso",
    "courseName": "Curso de Programação",
    "order": 1,
    "createdAt": "2025-07-02T14:30:00Z"
  },
  {
    "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3309",
    "name": "Módulo 2: Conceitos Básicos",
    "description": "Conceitos básicos de programação",
    "courseName": "Curso de Programação",
    "order": 2,
    "createdAt": "2025-07-02T14:31:00Z"
  }
]
```

### PUT /api/modules
**Descrição:** Atualizar módulo  
**Autenticação:** Bearer Token

**Request:**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3308",
  "name": "Módulo 1: Introdução Atualizada",
  "description": "Módulo introdutório atualizado",
  "courseId": "3f2504e0-4f89-11d3-9a0c-0305e82c3307",
  "order": 1
}
```

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3308",
  "name": "Módulo 1: Introdução Atualizada",
  "description": "Módulo introdutório atualizado",
  "courseName": "Curso de Programação",
  "order": 1,
  "updatedAt": "2025-07-02T15:30:00Z"
}
```

### DELETE /api/modules/{id}
**Descrição:** Deletar módulo  
**Autenticação:** Bearer Token

**Response (204 No Content)**

---

## 📚 Lições

### POST /api/lessons
**Descrição:** Criar nova lição  
**Autenticação:** Bearer Token

**Request:**
```json
{
  "name": "Lição 1: Introdução ao HTML",
  "description": "Aprenda os conceitos básicos do HTML",
  "moduleId": "3f2504e0-4f89-11d3-9a0c-0305e82c3308",
  "order": 1,
  "content": "Conteúdo da lição em HTML ou texto",
  "videoUrl": "https://example.com/video.mp4",
  "duration": 1800
}
```

**Response (201 Created):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3310",
  "name": "Lição 1: Introdução ao HTML",
  "description": "Aprenda os conceitos básicos do HTML",
  "moduleName": "Módulo 1: Introdução",
  "order": 1,
  "content": "Conteúdo da lição em HTML ou texto",
  "videoUrl": "https://example.com/video.mp4",
  "duration": 1800,
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### GET /api/lessons/module/{moduleId}
**Descrição:** Buscar lições por módulo  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
[
  {
    "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3310",
    "name": "Lição 1: Introdução ao HTML",
    "description": "Aprenda os conceitos básicos do HTML",
    "moduleName": "Módulo 1: Introdução",
    "order": 1,
    "content": "Conteúdo da lição em HTML ou texto",
    "videoUrl": "https://example.com/video.mp4",
    "duration": 1800,
    "createdAt": "2025-07-02T14:30:00Z"
  }
]
```

### GET /api/lessons/{id}
**Descrição:** Buscar lição por ID com progresso do usuário  
**Autenticação:** Bearer Token

**Response (200 OK):**
```json
{
  "id": "3f2504e0-4f89-11d3-9a0c-0305e82c3310",
  "name": "Lição 1: Introdução ao HTML",
  "description": "Aprenda os conceitos básicos do HTML",
  "moduleName": "Módulo 1: Introdução",
  "order": 1,
  "content": "Conteúdo da lição em HTML ou texto",
  "videoUrl": "https://example.com/video.mp4",
  "duration": 1800,
  "isCompleted": false,
  "completedAt": null,
  "watchedDuration": 0,
  "createdAt": "2025-07-02T14:30:00Z"
}
```

### DELETE /api/lessons/{id}
**Descrição:** Deletar lição  
**Autenticação:** Bearer Token

**Response (204 No Content)**

---

## 📊 Status da API

### Informações Gerais
- **Base URL:** `http://localhost:5284/api`
- **Formato:** JSON
- **Charset:** UTF-8
- **Versão:** 1.0

### Logs e Auditoria
A API possui sistema de logs automático que registra:
- Tentativas de login (sucesso e falha)
- Criação, edição e exclusão de recursos
- Acessos a recursos protegidos
- Erros de autenticação e autorização

### Seed de Dados
A API executa automaticamente o seed dos dados iniciais na inicialização:
- **Admin padrão:** admin@iduca.com / admin123
- **Empresa padrão:** Sistema Iduca
- **Categorias básicas:** Tecnologia, Negócios, Educação

---

## 🔑 Autenticação

A API usa autenticação Bearer Token. Para acessar rotas protegidas, inclua o header:

```
Authorization: Bearer {seu-token-aqui}
```

### Níveis de Acesso
- **Público**: Rota de login
- **Autenticado**: Usuário logado pode acessar
- **Admin**: Apenas usuários administradores podem acessar

### Exemplo de Uso
```bash
# Login com admin padrão
curl -X POST http://localhost:5284/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@iduca.com", "password": "admin123"}'

# Usar token retornado para acessar rotas protegidas
curl -X GET http://localhost:5284/api/users/all \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Criar um novo usuário (apenas admin)
curl -X POST http://localhost:5284/api/users \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "name": "João Silva",
    "identity": "12345678901",
    "email": "joao.silva@exemplo.com",
    "password": "123456",
    "isAdmin": false,
    "companyId": "guid-da-empresa"
  }'
```

---

## 🔧 Troubleshooting

### Problemas Comuns

#### 401 Unauthorized
- Verifique se o token está sendo enviado no header `Authorization: Bearer {token}`
- Confirme se o token não expirou
- Teste fazer login novamente para obter um novo token

#### 403 Forbidden
- Algumas rotas requerem privilégios de admin
- Use as credenciais do admin: `admin@iduca.com` / `admin123`

#### 404 Not Found
- Verifique se a URL está correta
- Confirme se o recurso (ID) existe na base de dados

#### 500 Internal Server Error
- Verifique se a API está rodando: `http://localhost:5284`
- Consulte os logs da aplicação para mais detalhes

### Scripts de Teste Disponíveis
A API inclui scripts PowerShell para testes automatizados:
- `test-auth.ps1` - Teste de autenticação
- `test-admin-login.ps1` - Login específico do admin
- `test-users-routes.ps1` - Teste das rotas de usuários
- `test-collection-organized.ps1` - Teste da coleção Postman organizada

---

## 📝 Notas Importantes

1. **Token de Autenticação**: Tokens são validados usando um sistema customizado. Não há expiração automática implementada no momento.

2. **Logs e Auditoria**: Todas as ações são registradas automaticamente no sistema de logs.

3. **Seed Automático**: Na primeira execução, o sistema cria automaticamente:
   - Admin padrão
   - Empresa padrão
   - Categorias básicas

4. **Permissões**: 
   - Usuários normais podem acessar recursos da própria empresa
   - Admins podem acessar todos os recursos do sistema

5. **Soft Delete**: Recursos deletados não são removidos permanentemente, apenas marcados como excluídos.

---

*Documentação atualizada em: 2 de julho de 2025*
*Versão da API: 1.0*
