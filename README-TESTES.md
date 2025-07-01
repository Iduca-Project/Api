# 🧪 Guia de Testes das Rotas da API

## 📋 Resumo das Rotas Implementadas

### 👥 **Users (Usuários)**
- `POST /api/users` - Criar usuário
- `GET /api/users/{id}` - Buscar usuário por ID
- `GET /api/users/all` - Listar usuários com filtros e paginação
- `PUT /api/users` - Atualizar usuário
- `DELETE /api/users/{id}` - Excluir usuário

### 📂 **Categories (Categorias)**
- `POST /api/categories` - Criar categoria
- `GET /api/categories/{id}` - Buscar categoria por ID
- `GET /api/categories/all` - Listar todas as categorias
- `GET /api/categories/search?Name={nome}` - Buscar categoria por nome
- `DELETE /api/categories/{id}` - Excluir categoria

### 📚 **Courses (Cursos)**
- `POST /api/course` - Criar curso
- `GET /api/course/{id}` - Buscar curso por ID
- `GET /api/course/all` - Listar cursos com filtros
- `PATCH /api/course` - Atualizar curso
- `DELETE /api/course/{id}` - Excluir curso

### 📖 **Modules (Módulos)**
- `POST /api/modules` - Criar módulo
- `GET /api/modules/{id}` - Buscar módulo por ID
- `GET /api/modules/course/{courseId}` - Buscar módulos por curso
- `PUT /api/modules` - Atualizar módulo
- `DELETE /api/modules/{id}` - Excluir módulo

## 🚀 Como Executar os Testes

### Opção 1: Usar o VS Code REST Client
1. Instale a extensão "REST Client" no VS Code
2. Abra o arquivo `test-routes.http`
3. Execute as requisições clicando em "Send Request"

### Opção 2: Usar PowerShell (Windows)
```powershell
cd "C:\Users\Aluno\Desktop\Amilton\Api"
.\test-api.ps1
```

### Opção 3: Usar Bash (Linux/Mac/WSL)
```bash
cd "/c/Users/Aluno/Desktop/Amilton/Api"
chmod +x test-api.sh
./test-api.sh
```

### Opção 4: Usar Postman ou Insomnia
Importe as requisições do arquivo `test-routes.http` ou crie manualmente.

## 📝 Ordem Recomendada de Testes

1. **Primeiro, criar dados básicos:**
   - Criar empresas
   - Criar categorias

2. **Depois, testar usuários:**
   - Criar usuários
   - Listar usuários
   - Buscar por filtros
   - Atualizar usuários

3. **Por último, testar cursos e módulos:**
   - Criar cursos
   - Criar módulos
   - Testar relacionamentos

## ⚠️ Observações Importantes

- **IDs**: Substitua os GUIDs `00000000-0000-0000-0000-000000000000` pelos IDs reais retornados pelas requisições
- **Banco de Dados**: A API usa MySQL, certifique-se de que está configurado corretamente
- **Ambiente**: Certifique-se de que o arquivo `.env` está configurado com a string de conexão do banco

## 🔧 Exemplo de Requisições

### Criar Usuário
```http
POST http://localhost:5284/api/users
Content-Type: application/json

{
  "name": "João Silva",
  "identity": "12345678901",
  "email": "joao.silva@teste.com",
  "password": "123456",
  "isAdmin": false,
  "responsibleId": null,
  "companyId": "GUID-DA-EMPRESA",
  "image": "https://example.com/avatar.jpg",
  "interests": ["GUID-CATEGORIA-1", "GUID-CATEGORIA-2"]
}
```

### Listar Usuários com Filtros
```http
GET http://localhost:5284/api/users/all?Name=João&IsAdmin=false&Page=1&MaxItems=10
```

### Criar Categoria
```http
POST http://localhost:5284/api/categories
Content-Type: application/json

{
  "name": "Tecnologia",
  "description": "Categoria sobre tecnologia"
}
```

## 🐛 Possíveis Problemas

1. **Erro 500**: Verifique se o banco de dados está rodando
2. **Erro 404**: Verifique se as rotas estão corretas
3. **Erro de validação**: Verifique se todos os campos obrigatórios estão preenchidos
4. **Foreign Key**: Certifique-se de usar IDs válidos para relacionamentos

## ✅ Funcionalidades Testadas

- [x] CRUD completo de Usuários
- [x] CRUD completo de Categorias  
- [x] CRUD de Cursos (existente + melhorias)
- [x] CRUD de Módulos (existente + Update)
- [x] Validações com FluentValidation
- [x] Mapeamento com AutoMapper
- [x] Arquitetura CQRS com MediatR
- [x] Soft Delete nos repositórios
- [x] Hash de senhas com BCrypt
- [x] Relacionamentos entre entidades
