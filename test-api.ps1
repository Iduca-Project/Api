# Script de teste das rotas da API
$baseUrl = "http://localhost:5284"

Write-Host "=== TESTANDO ROTAS DA API ===" -ForegroundColor Green

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
        return $null
    }
}

# Aguardar a API iniciar
Write-Host "Aguardando API iniciar..." -ForegroundColor Yellow
Start-Sleep -Seconds 3

# Teste 1: Criar uma empresa
$company = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/company" -Description "Criar Empresa" -Body @{
    name = "Empresa Teste"
    cnpj = "12345678000199"
    email = "contato@empresateste.com"
    phone = "(11) 99999-9999"
}

$companyId = if ($company) { $company.id } else { "00000000-0000-0000-0000-000000000000" }

# Teste 2: Criar categorias
$category1 = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/categories" -Description "Criar Categoria 1" -Body @{
    name = "Tecnologia"
    description = "Categoria sobre tecnologia"
}

$category2 = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/categories" -Description "Criar Categoria 2" -Body @{
    name = "Programação"
    description = "Categoria sobre programação"
}

# Teste 3: Listar todas as categorias
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/categories/all" -Description "Listar Todas as Categorias"

# Teste 4: Buscar categoria por nome
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/categories/search?Name=Tecnologia" -Description "Buscar Categoria por Nome"

# Teste 5: Criar usuário
$user = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/users" -Description "Criar Usuário" -Body @{
    name = "João Silva"
    identity = "12345678901"
    email = "joao.silva@teste.com"
    password = "123456"
    isAdmin = $false
    responsibleId = $null
    companyId = $companyId
    image = "https://example.com/avatar.jpg"
    interests = @()
}

$userId = if ($user) { $user.id } else { "00000000-0000-0000-0000-000000000000" }

# Teste 6: Criar usuário admin
$admin = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/users" -Description "Criar Usuário Admin" -Body @{
    name = "Maria Santos"
    identity = "98765432100"
    email = "maria.santos@teste.com"
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
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/users/all?Name=João&Page=1&MaxItems=10" -Description "Buscar Usuário por Nome"

# Teste 9: Buscar apenas admins
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/users/all?IsAdmin=true&Page=1&MaxItems=10" -Description "Buscar Apenas Admins"

# Teste 10: Buscar usuário por ID
if ($userId -ne "00000000-0000-0000-0000-000000000000") {
    Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/users/$userId" -Description "Buscar Usuário por ID"
}

# Teste 11: Criar curso
$course = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/course" -Description "Criar Curso" -Body @{
    name = "Curso de .NET Core"
    description = "Aprenda desenvolvimento com .NET Core"
    difficulty = 2
    image = "https://example.com/course.jpg"
    totalHours = 40
    categories = @()
}

$courseId = if ($course) { $course.id } else { "00000000-0000-0000-0000-000000000000" }

# Teste 12: Listar cursos
Invoke-ApiRequest -Method "GET" -Url "$baseUrl/api/course/all?Page=1&MaxItens=10" -Description "Listar Cursos"

# Teste 13: Criar módulo
if ($courseId -ne "00000000-0000-0000-0000-000000000000") {
    $module = Invoke-ApiRequest -Method "POST" -Url "$baseUrl/api/modules" -Description "Criar Módulo" -Body @{
        name = "Introdução ao .NET"
        description = "Módulo introdutório sobre .NET Core"
        index = 1
        courseId = $courseId
    }
}

Write-Host "`n=== TESTES CONCLUÍDOS ===" -ForegroundColor Green
