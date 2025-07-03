# GUIA DE USO - POSTMAN COLLECTION ORGANIZADO

## 📋 Visão Geral

O Postman Collection da API Iduca foi completamente reorganizado por **nível de acesso**, facilitando os testes e o uso da API.

---

## 🚀 COMO USAR

### 1. **Importar o Collection**
```
Arquivo: API-Iduca-Postman-Collection-ORGANIZED.json
```

### 2. **Configurar Variáveis**
As seguintes variáveis já estão pré-configuradas:
- `base_url`: http://localhost:5284
- `admin_token`: (será preenchido automaticamente)
- `user_token`: (será preenchido automaticamente) 
- `manager_token`: (será preenchido automaticamente)
- IDs de exemplo para testes

### 3. **Fluxo de Teste Recomendado**

#### **Passo 1: Autenticação**
1. Execute **"0. AUTENTICAÇÃO > Login - Administrador"**
   - Os tokens serão salvos automaticamente
2. Execute **"Login - Gestor"** e **"Login - Usuário Comum"**

#### **Passo 2: Setup Inicial (Admin)**
1. **Criar Empresa**: `3. ADMINISTRADOR > Empresas > Criar Empresa`
2. **Criar Categorias**: `3. ADMINISTRADOR > Categorias > Criar Categoria`
3. **Criar Usuários**: `3. ADMINISTRADOR > Usuários > Criar Usuário`
4. **Criar Cursos**: `3. ADMINISTRADOR > Cursos > Criar Curso`

#### **Passo 3: Testes de Usuário Comum**
1. **Visualizar Categorias**: `1. USUÁRIO COMUM > Categorias > Listar Categorias`
2. **Visualizar Cursos**: `1. USUÁRIO COMUM > Cursos > Listar Cursos`
3. **Auto-matrícula**: `1. USUÁRIO COMUM > Cursos > Matricular-se em Curso`

#### **Passo 4: Testes de Gestor**
1. **Ver Hierarquia**: `2. GESTOR > Hierarquia > Obter Árvore Hierárquica`
2. **Gerenciar Subordinados**: `2. GESTOR > Hierarquia > Obter Subordinados`
3. **Matrícula em Massa**: `2. GESTOR > Hierarquia > Matricular Equipe em Curso`

---

## 🎯 ESTRUTURA DO COLLECTION

### **0. AUTENTICAÇÃO (Público)**
- ✅ Login para todos os tipos de usuário
- ✅ Logout
- ✅ Tokens salvos automaticamente

### **1. USUÁRIO COMUM - Funcionalidades Básicas**
- 📖 **Apenas visualização** de conteúdo
- 🎓 **Auto-matrícula** em cursos
- 👤 **Gestão de perfil** pessoal
- 📅 **Calendário** pessoal

### **2. GESTOR - Hierarquia e Equipe**
- 🏗️ **Gestão de hierarquia** organizacional
- 👥 **Gerenciamento de subordinados**
- 📊 **Relatórios da equipe**
- 🎓 **Matrícula em massa** da equipe

### **3. ADMINISTRADOR - Gestão Completa**
- 🏢 **CRUD de empresas**
- 📚 **CRUD de categorias e cursos**
- 👥 **Gestão completa de usuários**
- 📊 **Relatórios administrativos**

### **4. UTILITÁRIOS E TESTES**
- ✅ **Health checks**
- 🧪 **Endpoints de teste**
- 🔍 **Verificações gerais**

---

## 🔑 TOKENS E AUTENTICAÇÃO

### Tokens Automáticos
Os requests de login salvam automaticamente os tokens:
```javascript
// Script automático nos logins
if (pm.response.code === 200) {
    var responseJson = pm.response.json();
    pm.collectionVariables.set('user_token', responseJson.token);
    pm.collectionVariables.set('user_id', responseJson.user.id);
}
```

### Uso dos Tokens
- **{{user_token}}**: Para endpoints de usuário comum
- **{{manager_token}}**: Para endpoints de gestor  
- **{{admin_token}}**: Para endpoints administrativos

---

## 📝 EXEMPLOS DE TESTE

### **Teste Completo de Fluxo**

#### 1. **Criar Infraestrutura (Admin)**
```json
POST /api/admin/companies
{
  "name": "Empresa Teste Ltda",
  "cnpj": "12.345.678/0001-90",
  "email": "contato@empresateste.com"
}
```

#### 2. **Criar Categoria (Admin)**
```json
POST /api/admin/categories
{
  "name": "Desenvolvimento",
  "description": "Cursos de programação"
}
```

#### 3. **Criar Curso (Admin)**
```json
POST /api/admin/courses
{
  "title": "JavaScript Básico",
  "description": "Aprenda JavaScript do zero",
  "categoryId": "{{category_id}}",
  "duration": 40
}
```

#### 4. **Visualizar como Usuário**
```
GET /api/categories (usar user_token)
GET /api/courses (usar user_token)
```

#### 5. **Auto-matrícula**
```
POST /api/courses/{{course_id}}/enroll (usar user_token)
```

#### 6. **Gestão Hierárquica (Gestor)**
```
GET /api/hierarchy/tree (usar manager_token)
POST /api/hierarchy/enroll-team/{{course_id}} (usar manager_token)
```

---

## ⚠️ DICAS IMPORTANTES

### **Ordem de Execução**
1. ✅ Sempre fazer login primeiro
2. ✅ Criar infraestrutura (empresa, categorias) antes dos testes
3. ✅ Usar o token correto para cada tipo de operação

### **Variáveis Úteis**
```
{{base_url}} - URL base da API
{{company_id}} - ID da empresa para testes
{{course_id}} - ID do curso para testes
{{user_id}} - ID do usuário para testes
{{category_id}} - ID da categoria para testes
```

### **Headers Automáticos**
Os headers de autorização estão pré-configurados:
```
Authorization: Bearer {{user_token}}
Authorization: Bearer {{manager_token}}  
Authorization: Bearer {{admin_token}}
```

---

## 🐛 RESOLUÇÃO DE PROBLEMAS

### **Token Inválido/Expirado**
1. Execute novamente o login correspondente
2. Verifique se está usando o token correto para o endpoint

### **Erro 403 (Forbidden)**
1. Verifique se está usando o token do nível correto:
   - Endpoints `/api/admin/*` requerem `admin_token`
   - Endpoints `/api/hierarchy/*` requerem `manager_token` ou superior
   - Endpoints básicos aceitam qualquer token válido

### **Erro 404 (Not Found)**
1. Verifique se os IDs nas variáveis estão corretos
2. Certifique-se de que os recursos foram criados primeiro

### **Dados não Encontrados**
1. Execute primeiro os endpoints de criação (Admin)
2. Atualize os IDs nas variáveis do collection

---

## 📊 MONITORAMENTO

### **Testes de Saúde**
```
GET /api/health - Verifica se a API está funcionando
GET /api/home - Página inicial da API
```

### **Logs Úteis**
Os scripts do Postman mostram logs no console:
```javascript
console.log('Token de admin salvo:', responseJson.token);
```

---

## 🎉 CONCLUSÃO

Este collection organizado facilita:
- ✅ **Testes sistemáticos** por nível de acesso
- ✅ **Desenvolvimento** com exemplos claros
- ✅ **Documentação** viva da API
- ✅ **Depuração** eficiente de problemas

Para dúvidas ou problemas, consulte o `RELATORIO-FINAL-ORGANIZACAO.md` com a documentação completa da estrutura da API.
