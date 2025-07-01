# Script de teste das rotas da API - VERSÃO CORRIGIDA
$baseUrl = "http://localhost:5284"

Write-Host "=== TESTANDO ROTAS DA API (VERSÃO CORRIGIDA) ===" -ForegroundColor Green

# Função para fazer requisições HTTP
function Invoke-ApiRequest {
    param(
        [string]$Method,
        [string]$Url,
        [hashtable]$Body = $null,
        [string]$Description
    )
    
    Write-Host "`n--- $Description ---" -ForegroundColor Yellow
    Write-Host "$Method $Url" -ForegroundColor Cyan
    
    try {
        $headers = @{
            "Content-Type" = "application/json"
            "Accept" = "application/json"
        }
        
        if ($Body) {
            $jsonBody = $Body | ConvertTo-Json -Depth 10
            Write-Host "Body: $jsonBody" -ForegroundColor Gray
            $response = Invoke-RestMethod -Uri $Url -Method $Method -Body $jsonBody -Headers $headers
        } else {
            $response = Invoke-RestMethod -Uri $Url -Method $Method -Headers $headers
        }
        
        Write-Host "Status: SUCCESS" -ForegroundColor Green
        Write-Host "Response: $($response | ConvertTo-Json -Depth 3)" -ForegroundColor White
        return $response
    }
    catch {
        Write-Host "Status: ERROR" -ForegroundColor Red
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.ErrorDetails) {
            Write-Host "Details: $($_.ErrorDetails.Message)" -ForegroundColor Red
        }
        return $null
    }
}

# Aguardar a API iniciar
Write-Host "Aguardando API iniciar..." -ForegroundColor Yellow
Start-Sleep -Seconds 3

# Teste 1: Criar uma empresa
$company = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/company" -Description "Criar Empresa" -Body @{
    name = "Empresa Teste Corrigida"
    cnpj = "12345678000188"
    email = "contato@empresatestecorrigida.com"
    phone = "(11) 98888-8888"
}

$companyId = if ($company) { $company.id } else { "00000000-0000-0000-0000-000000000000" }

# Teste 2: Criar categorias (CORRIGIDO - removido description)
$category1 = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/categories" -Description "Criar Categoria 1 (CORRIGIDO)" -Body @{
    name = "Tecnologia Corrigida"
}

$category2 = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/categories" -Description "Criar Categoria 2 (CORRIGIDO)" -Body @{
    name = "Programacao Corrigida"
}

$categoryId1 = if ($category1) { $category1.id } else { $null }
$categoryId2 = if ($category2) { $category2.id } else { $null }

# Teste 3: Listar todas as categorias (CORRIGIDO)
$allCategories = Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/categories/all" -Description "Listar Todas as Categorias (CORRIGIDO)"

# Teste 4: Buscar categoria por nome
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/categories/search?Name=Tecnologia" -Description "Buscar Categoria por Nome"

# Teste 5: Criar usuário (CORRIGIDO - usando IDs reais)
$interests = @()
if ($categoryId1) { $interests += $categoryId1 }
if ($categoryId2) { $interests += $categoryId2 }

$user = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/users" -Description "Criar Usuário (CORRIGIDO)" -Body @{
    name = "Joao Silva Corrigido"
    identity = "12345678902"
    email = "joao.silva.corrigido@teste.com"
    password = "123456"
    isAdmin = $false
    responsibleId = $null
    companyId = $companyId
    image = "https://example.com/avatar2.jpg"
    interests = $interests
}

$userId = if ($user) { $user.id } else { "00000000-0000-0000-0000-000000000000" }

# Teste 6: Criar usuário admin
$admin = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/users" -Description "Criar Usuário Admin" -Body @{
    name = "Maria Santos Admin"
    identity = "98765432101"
    email = "maria.santos.admin@teste.com"
    password = "admin123"
    isAdmin = $true
    responsibleId = $null
    companyId = $companyId
    image = $null
    interests = @()
}

# Teste 7: Listar usuários
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/users/all?Page=1&MaxItems=10" -Description "Listar Usuários"

# Teste 8: Buscar usuário por nome
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/users/all?Name=Joao&Page=1&MaxItems=10" -Description "Buscar Usuário por Nome"

# Teste 9: Buscar apenas admins
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/users/all?IsAdmin=true&Page=1&MaxItems=10" -Description "Buscar Apenas Admins"

# Teste 10: Buscar usuário por ID
if ($userId -ne "00000000-0000-0000-0000-000000000000") {
    Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/users/$userId" -Description "Buscar Usuário por ID"
}

# Teste 11: Criar curso
$course = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/course" -Description "Criar Curso" -Body @{
    name = "Curso de .NET Core Corrigido"
    description = "Aprenda desenvolvimento com .NET Core - versao corrigida"
    difficulty = 2
    image = "https://example.com/course2.jpg"
    totalHours = 40
    categories = @()
}

$courseId = if ($course) { $course.id } else { "00000000-0000-0000-0000-000000000000" }

# Teste 12: Listar cursos
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/course/all?Page=1&MaxItens=10" -Description "Listar Cursos"

# Teste 13: Criar módulo (CORRIGIDO - com index)
if ($courseId -ne "00000000-0000-0000-0000-000000000000") {
    $module = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/modules" -Description "Criar Módulo (CORRIGIDO)" -Body @{
        name = "Introducao ao .NET Corrigido"
        description = "Modulo introdutorio sobre .NET Core - versao corrigida"
        index = 1
        courseId = $courseId
    }
    
    $moduleId = if ($module) { $module.id } else { $null }
    
    # Teste 14: Buscar módulos por curso
    if ($moduleId) {
        Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/modules/course/$courseId" -Description "Buscar Módulos por Curso"
        
        # Teste 15: Buscar módulo por ID
        Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/modules/$moduleId" -Description "Buscar Módulo por ID"
        
        # Teste 16: Atualizar módulo
        Invoke-ApiRequest -Method "PUT" -Url "$baseUrl/api/modules" -Description "Atualizar Módulo" -Body @{
            id = $moduleId
            name = "Introducao Avancada ao .NET"
            description = "Modulo introdutorio avancado sobre .NET Core"
            index = 1
            courseId = $courseId
        }
    }
}

# Teste 17: Atualizar usuário
if ($userId -ne "00000000-0000-0000-0000-000000000000") {
    Invoke-ApiRequest -Method "PUT" -Url "$baseUrl/api/users" -Description "Atualizar Usuário" -Body @{
        id = $userId
        name = "Joao Silva Santos Atualizado"
        identity = "12345678902"
        email = "joao.santos.atualizado@teste.com"
        isAdmin = $false
        responsibleId = $null
        companyId = $companyId
        image = "https://example.com/new-avatar.jpg"
        interests = $interests
    }
}

Write-Host "`n=== TESTES CORRIGIDOS CONCLUÍDOS ===" -ForegroundColor Green
Write-Host "Problemas corrigidos:" -ForegroundColor Cyan
Write-Host "✅ Removido 'description' das categorias" -ForegroundColor White
Write-Host "✅ Adicionado 'index' obrigatório nos módulos" -ForegroundColor White
Write-Host "✅ Corrigido mapeamento de responses" -ForegroundColor White
Write-Host "✅ Usado IDs reais em todas as requisições" -ForegroundColor White
