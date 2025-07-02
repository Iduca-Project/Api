# 📋 API Iduca - Coleção Postman Organizada por Permissões

## 📁 Estrutura da Coleção

A coleção `API-Iduca-Postman-Collection-Organized.json` está organizada de forma hierárquica para facilitar o entendimento das permissões e facilitar os testes:

### 🔐 Auth Controller
- **🌐 Público (Sem Autenticação)**
  - Login - Usuário Comum (captura token)
  - Login - Administrador (captura token)
  - Login - Credenciais Inválidas (teste de falha)

### 👥 Users Controller
- **👤 Usuário Comum**
  - Buscar Próprio Perfil
- **🔑 Administrador**
  - Criar Usuário (captura user_id)
  - Listar Todos os Usuários
  - Buscar Usuários com Filtros
  - Buscar Usuário por ID
  - Atualizar Usuário
  - Deletar Usuário

### 🏢 Companies Controller
- **👤 Usuário Comum**
  - Buscar Empresa por ID
  - Listar Todas as Empresas (captura company_id)
- **🔑 Administrador**
  - Criar Empresa (captura company_id)
  - Atualizar Empresa
  - Deletar Empresa

### 📂 Categories Controller
- **👤 Usuário Comum**
  - Buscar Categoria por ID
  - Listar Todas as Categorias (captura category_id)
  - Buscar Categoria por Nome
- **🔑 Administrador**
  - Criar Categoria (captura category_id)
  - Deletar Categoria

### 📚 Courses Controller
- **👤 Usuário Comum**
  - Buscar Curso por ID
  - Listar Todos os Cursos (captura course_id)
  - Listar Cursos com Filtros
- **🔑 Administrador**
  - Criar Curso (captura course_id)
  - Atualizar Curso
  - Deletar Curso

### 📖 Modules Controller
- **👤 Usuário Comum**
  - Buscar Módulo por ID
  - Listar Módulos por Curso (captura module_id)
- **🔑 Administrador**
  - Criar Módulo (captura module_id)
  - Atualizar Módulo
  - Deletar Módulo

### 📝 Lessons Controller
- **👤 Usuário Comum**
  - Buscar Aula por ID
  - Listar Aulas por Módulo (captura lesson_id)
- **🔑 Administrador**
  - Criar Aula (captura lesson_id)
  - Atualizar Aula
  - Deletar Aula

### 🧪 Testes de Segurança
- **❌ Testes de Acesso Negado**
  - Sem Token - Deve falhar (401)
  - Token Inválido - Deve falhar (401)
  - Usuário Comum tentando Admin - Deve falhar (403)

### 🎯 Fluxo Completo - Setup
Sequência automatizada para configurar todos os dados necessários:
1. Login Admin
2. Criar Empresa
3. Criar Categoria
4. Criar Curso
5. Criar Módulo
6. Criar Aula
7. Criar Usuário Comum

## 🔧 Como Usar

### 1. Importar no Postman
1. Abra o Postman
2. Clique em "Import"
3. Selecione o arquivo `API-Iduca-Postman-Collection-Organized.json`
4. A coleção será importada com todas as pastas organizadas

### 2. Configurar Variáveis
As seguintes variáveis são gerenciadas automaticamente:
- `base_url`: http://localhost:5284
- `auth_token`: Token JWT (atualizado automaticamente no login)
- `user_id`: ID do usuário criado
- `company_id`: ID da empresa criada
- `category_id`: ID da categoria criada
- `course_id`: ID do curso criado
- `module_id`: ID do módulo criado
- `lesson_id`: ID da aula criada

### 3. Executar Testes

#### Para Setup Inicial:
1. Execute toda a pasta "🎯 Fluxo Completo - Setup" em sequência
2. Isso criará todos os dados necessários e salvará os IDs nas variáveis

#### Para Testar Permissões de Usuário Comum:
1. Execute "Login - Usuário Comum" em Auth Controller
2. Teste as pastas "👤 Usuário Comum" de cada controller
3. Verifique que apenas operações de visualização funcionam

#### Para Testar Permissões de Admin:
1. Execute "Login - Administrador" em Auth Controller
2. Teste as pastas "🔑 Administrador" de cada controller
3. Verifique que todas as operações CRUD funcionam

#### Para Testar Segurança:
1. Execute a pasta "🧪 Testes de Segurança"
2. Verifique que os testes de acesso negado retornam os códigos corretos

## 🎨 Recursos da Coleção

### ✨ Automação
- **Captura Automática de IDs**: Todos os IDs importantes são capturados e salvos automaticamente
- **Scripts de Teste**: Validação automática de respostas e códigos de status
- **Fluxo Sequencial**: Ordem lógica para execução dos testes

### 🔒 Segurança
- **Separação Clara de Permissões**: Usuário comum vs Administrador
- **Testes de Autenticação**: Validação de tokens e acesso negado
- **Testes de Autorização**: Verificação de permissões específicas

### 📊 Organização
- **Estrutura Hierárquica**: Pastas e subpastas organizadas logicamente
- **Nomenclatura Clara**: Emojis e descrições facilitam identificação
- **Documentação Inline**: Cada request tem descrição detalhada

## 🚀 Executar Script de Teste Automatizado

Para executar todos os testes via PowerShell:

```powershell
.\test-collection-organized.ps1
```

Este script valida:
- ✅ Autenticação de admin e usuário comum
- ✅ Autorização correta (admin pode tudo, usuário limitado)
- ✅ Visualização de dados (usuário pode ver dados públicos)
- ✅ Segurança (bloqueio sem autenticação ou com token inválido)

## 📋 Status dos Endpoints

### Autenticação ✅
- [x] Login público funcionando
- [x] Captura de token automática
- [x] Validação de credenciais

### Autorização ✅
- [x] Separação usuário comum / admin
- [x] Bloqueio correto de operações não autorizadas
- [x] Permissões específicas por controller

### CRUD Completo ✅
- [x] Users: Visualização (usuário) + CRUD completo (admin)
- [x] Companies: Visualização (usuário) + CRUD completo (admin)
- [x] Categories: Visualização + busca (usuário) + Create/Delete (admin)
- [x] Courses: Visualização + filtros (usuário) + CRUD completo (admin)
- [x] Modules: Visualização (usuário) + CRUD completo (admin)
- [x] Lessons: Visualização (usuário) + CRUD completo (admin)

### Recursos Avançados ✅
- [x] Paginação em listagens
- [x] Filtros de busca
- [x] Captura automática de IDs
- [x] Testes de segurança
- [x] Fluxo de setup automatizado

## 🎯 Próximos Passos

1. **Validar na Prática**: Testar a aplicação com dados reais
2. **Logs de Auditoria**: Verificar se os logs estão sendo salvos corretamente
3. **Performance**: Testar com maior volume de dados
4. **Documentação**: Usar a coleção como base para documentação da API

---

**Nota**: Esta coleção substitui e melhora a organização da coleção anterior, mantendo todos os recursos e adicionando clara separação de permissões.
