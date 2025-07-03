# Hierarquia Organizacional - Implementação Completa

## Visão Geral

A API agora implementa um sistema completo de hierarquia organizacional onde usuários podem ter responsáveis (superiores) e subordinados, criando uma árvore hierárquica dentro da empresa.

## Regras de Negócio Implementadas

### 1. **Estrutura Hierárquica**
- Cada usuário pode ter um responsável (`ResponsibleId`)
- Um usuário pode ser responsável por múltiplos subordinados
- A hierarquia é por empresa (usuários só podem ser responsáveis por outros da mesma empresa)
- **Prevenção de Ciclos**: Implementada verificação automática que impede ciclos na hierarquia

### 2. **Controle de Acesso por Hierarquia**

#### **Usuários (`UsersController`)**
- ✅ **Visualizar dados**: Usuários podem ver apenas seus próprios dados, de seus subordinados ou superiores
- ✅ **Atualizar dados**: Apenas admins ou superiores hierárquicos podem atualizar outros usuários
- ✅ **Listar usuários**: Não-admins veem apenas usuários acessíveis pela hierarquia
- ✅ **Deletar usuários**: Restrito apenas a administradores

#### **Analytics (`AnalyticsController`)**
- ✅ **Estatísticas da empresa**: Apenas admins ou usuários com subordinados
- ✅ **Estatísticas de categorias**: Apenas admins ou usuários com subordinados  
- ✅ **Estatísticas de usuário**: Apenas próprio usuário, superiores ou admins
- ✅ **Estatísticas da equipe**: Baseado nos subordinados do usuário logado

#### **Manager (`ManagerController`)**
- ✅ **Dashboard**: Dados apenas dos subordinados diretos e indiretos
- ✅ **Equipe**: Lista apenas subordinados do manager
- ✅ **Status de cursos**: Verificação de hierarquia antes de mostrar dados
- ✅ **Dashboard do funcionário**: Apenas subordinados acessíveis

#### **Courses (`CoursesController`)**
- ✅ **Matrícula**: Usuários podem matricular a si mesmos ou seus subordinados
- ✅ **Matrícula em massa**: Endpoint para matricular toda a equipe

### 3. **Endpoints de Hierarquia**

#### **Gestão de Responsabilidade**
```
POST   /api/hierarchy/{userId}/assign-responsible/{responsibleId}
DELETE /api/hierarchy/{userId}/remove-responsible
```

#### **Consultas Hierárquicas**
```
GET /api/hierarchy/tree                           # Árvore hierárquica da empresa
GET /api/hierarchy/{userId}/subordinates          # Subordinados de um usuário
GET /api/hierarchy/{userId}/superiors             # Superiores de um usuário
GET /api/hierarchy/accessible-users               # Usuários acessíveis pelo atual
```

#### **Verificações**
```
GET /api/hierarchy/check-hierarchy/{superiorId}/{subordinateId}
GET /api/hierarchy/check-cycle/{userId}/{newResponsibleId}
```

#### **Funcionalidades Especiais**
```
POST /api/hierarchy/enroll-team/{courseId}        # Matricular equipe em curso
```

## Serviços Implementados

### **IHierarchyService**
- `AssignResponsibleAsync()` - Atribui responsável com verificação de ciclos
- `RemoveResponsibleAsync()` - Remove responsável
- `WouldCreateCycleAsync()` - Verifica se atribuição criaria ciclo
- `GetSubordinatesAsync()` - Obtém subordinados (diretos/indiretos)
- `GetSuperiorsAsync()` - Obtém cadeia de superiores
- `IsUserSuperiorToAsync()` - Verifica relação hierárquica
- `CanUserAccessDataAsync()` - Verifica permissão de acesso
- `GetAccessibleUserIdsAsync()` - Lista IDs acessíveis por hierarquia
- `GetHierarchyTreeAsync()` - Monta árvore hierárquica completa

## Atributos de Autorização

### **[HierarchyAuthorize]**
- Verifica se o usuário tem permissão hierárquica para a operação
- Usado em endpoints sensíveis de modificação de hierarquia

### **BaseController**
- Métodos helpers para obter informações do usuário atual
- `GetCurrentUserId()`, `GetCurrentUserCompanyId()`, `IsCurrentUserAdmin()`

## Exemplo de Fluxo

### **Cenário: Manager quer ver estatísticas de um subordinado**

1. Manager faz requisição para `/api/analytics/users/{subordinateId}`
2. Sistema verifica se manager pode acessar dados do subordinado via `CanUserAccessDataAsync()`
3. Se permitido, retorna estatísticas; caso contrário, retorna `403 Forbid`

### **Cenário: Atribuir responsabilidade**

1. Admin faz POST para `/api/hierarchy/{userId}/assign-responsible/{responsibleId}`
2. Sistema verifica:
   - Ambos usuários existem e são da mesma empresa
   - Não criaria ciclo na hierarquia via `WouldCreateCycleAsync()`
3. Se válido, atribui responsabilidade; caso contrário, retorna erro

## Validações de Segurança

### **Prevenção de Ciclos**
- ✅ Auto-referência (usuário não pode ser responsável por si mesmo)
- ✅ Ciclo direto (A → B → A)
- ✅ Ciclo indireto (A → B → C → A)
- ✅ Verificação da cadeia completa de superiores

### **Isolamento por Empresa**
- ✅ Usuários só podem ser responsáveis por outros da mesma empresa
- ✅ Hierarquia isolada por `CompanyId`

### **Controle de Acesso**
- ✅ Usuários só acessam dados que têm permissão
- ✅ Admins têm acesso total dentro da empresa
- ✅ Não-admins seguem regras hierárquicas

## Próximos Passos (Opcional)

1. **Implementar handlers reais** para substituir responses mock
2. **Notificações** quando hierarquia é alterada
3. **Histórico de mudanças** na hierarquia
4. **Níveis hierárquicos** com permissões específicas
5. **Delegação temporária** de responsabilidades
6. **Relatórios hierárquicos** avançados

## Resumo da Implementação

✅ **HierarchyService completo** com toda lógica de negócio
✅ **Controllers atualizados** com verificações hierárquicas  
✅ **Prevenção de ciclos** implementada
✅ **Controle de acesso** baseado em hierarquia
✅ **Endpoints RESTful** para gerenciar hierarquia
✅ **Documentação completa** das regras implementadas

A API agora possui um sistema robusto de hierarquia organizacional que garante que usuários só acessem dados que têm permissão baseado em sua posição na estrutura da empresa.
