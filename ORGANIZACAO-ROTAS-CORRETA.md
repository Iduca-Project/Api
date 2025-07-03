# 📋 ORGANIZAÇÃO CORRETA DAS ROTAS API IDUCA

## 🚀 **ESTRUTURA FINAL ORGANIZADA**

### 🔐 **1. PÚBLICO (Sem autenticação)**
```
POST   /api/auth/login
POST   /api/auth/forgotPass  
POST   /api/auth/checkCode
POST   /api/auth/resendCode
POST   /api/auth/resetPassword
GET    /api/interests (AllowAnonymous)
```

### 👤 **2. USUÁRIOS COMUNS (user_token)**
```
# Home/Dashboard
GET    /api/home/progress
GET    /api/home/coursesInProgress
GET    /api/home/calendar

# Cursos (visualização e auto-matrícula)
GET    /api/courses
GET    /api/courses/{id}
POST   /api/courses/{courseId}/enroll

# Categorias (apenas visualização)
GET    /api/categories
GET    /api/categories/search?Name=xxx
GET    /api/categories/{id}
GET    /api/categories/all

# Calendário
GET    /api/calendar
GET    /api/calendar/next
POST   /api/calendar/reminder

# Perfil
GET    /api/profile
PUT    /api/profile
GET    /api/certificate/{id}/image
GET    /api/certificate/{id}/pdf

# Lições e Testes
GET    /api/lessons/{id}
GET    /api/test/{id}
```

### 🧑‍💼 **3. GESTORES/MANAGERS (manager_token)**
```
# Gestão de Equipe
GET    /api/manager/dashboard
GET    /api/manager/team
GET    /api/manager/courses-status?employeeId={id}
POST   /api/manager/enroll
GET    /api/manager/employeesSummary
GET    /api/manager/employee/{id}/dashboard
GET    /api/manager/export
POST   /api/manager/employees

# Hierarquia (gestão de subordinados)
GET    /api/hierarchy/tree
GET    /api/hierarchy/{userId}/subordinates
GET    /api/hierarchy/{userId}/superiors
POST   /api/hierarchy/{userId}/assign-responsible/{responsibleId}
DELETE /api/hierarchy/{userId}/remove-responsible
GET    /api/hierarchy/check-hierarchy/{superiorId}/{subordinateId}
GET    /api/hierarchy/accessible-users
POST   /api/hierarchy/enroll-team/{courseId}
```

### 👑 **4. ADMINISTRADORES (admin_token)**
```
# Empresas
GET    /api/admin/companies
POST   /api/admin/companies
PUT    /api/admin/companies/{companyId}
DELETE /api/admin/companies/{companyId}

# Cursos (criação/exclusão)
POST   /api/admin/course
DELETE /api/admin/course/{idCourse}

# Categorias (criação/exclusão)
POST   /api/admin/categories
DELETE /api/admin/categories/{categoryId}

# Managers
POST   /api/admin/managers
```

---

## ✅ **ROTAS REMOVIDAS/REORGANIZADAS:**

### ❌ **CompaniesController REMOVIDO**
- Todas as operações de companies agora estão em `/api/admin/companies`

### ❌ **Endpoints administrativos removidos de controllers de usuário:**
- `POST /api/categories` → `POST /api/admin/categories`
- `DELETE /api/categories/{id}` → `DELETE /api/admin/categories/{id}`
- `POST /api/courses` → `POST /api/admin/course`
- `DELETE /api/courses/{id}` → `DELETE /api/admin/course/{id}`

### ✅ **Controllers organizados por público-alvo:**
- **CategoriesController**: Apenas GET (usuários)
- **CoursesController**: GET + auto-matrícula (usuários)
- **ManagerController**: Gestão de equipe (gestores)
- **HierarchyController**: Gestão hierárquica (gestores/admins)
- **AdminController**: Todas as operações administrativas (admins)

---

## 🎯 **ROTAS CORRETAS PARA TESTES:**

### Para criar uma empresa:
✅ `POST {{base_url}}/api/admin/companies`

### Para criar uma categoria:
✅ `POST {{base_url}}/api/admin/categories`

### Para criar um curso:
✅ `POST {{base_url}}/api/admin/course`

### Para listar categorias (usuário comum):
✅ `GET {{base_url}}/api/categories`

### Para listar cursos (usuário comum):
✅ `GET {{base_url}}/api/courses`

---

## 🚀 **PRÓXIMOS PASSOS:**
1. ✅ Controllers organizados por nível de acesso
2. 🔄 Atualizar Collection do Postman com estrutura correta
3. 🔄 Testar endpoints com tokens adequados
4. 🔄 Implementar FirstAccess após aplicar migração

**A API agora está organizda seguindo os princípios de segurança e separação de responsabilidades!** 🎉
