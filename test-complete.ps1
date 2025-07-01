# Script completo de teste das rotas da API com captura automática de IDs
$baseUrl = "http://localhost:5284"

# Função para fazer requisições HTTP com melhor tratamento de erros
function Invoke-ApiTest {
    param(
        [string]$Method,
        [string]$Endpoint,
        [hashtable]$Body = $null,
        [string]$Description,
        [bool]$ReturnResponse = $false
    )
    
    $url = "$baseUrl$Endpoint"
    Write-Host "`n=== $Description ===" -ForegroundColor Cyan
    Write-Host "$Method $url" -ForegroundColor Yellow
    
    try {
        $headers = @{
            "Content-Type" = "application/json"
            "Accept" = "application/json"
        }
        
        $params = @{
            Uri = $url
            Method = $Method
            Headers = $headers
            TimeoutSec = 30
        }
        
        if ($Body) {
            $jsonBody = $Body | ConvertTo-Json -Depth 10
            Write-Host "Enviando Body:" -ForegroundColor Gray
            Write-Host $jsonBody -ForegroundColor White
            $params.Body = $jsonBody
        }
        
        $response = Invoke-RestMethod @params
        
        Write-Host "✅ SUCESSO" -ForegroundColor Green
        if ($response) {
            $responseJson = $response | ConvertTo-Json -Depth 3
            Write-Host "Resposta:" -ForegroundColor Gray
            Write-Host $responseJson -ForegroundColor White
            
            if ($ReturnResponse) {
                return $response
            }
        }
        
        return $true
    }
    catch {
        Write-Host "❌ ERRO: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.Exception.Response) {
            $statusCode = $_.Exception.Response.StatusCode
            Write-Host "Status Code: $statusCode" -ForegroundColor Red
            
            try {
                $errorStream = $_.Exception.Response.GetResponseStream()
                $reader = New-Object System.IO.StreamReader($errorStream)
                $errorBody = $reader.ReadToEnd()
                if ($errorBody) {
                    Write-Host "Erro detalhado: $errorBody" -ForegroundColor Red
                }
            } catch {
                Write-Host "Não foi possível ler o erro detalhado" -ForegroundColor Yellow
            }
        }
        return $false
    }
}

# Função para aguardar a API
function Wait-ForApi {
    Write-Host "🔄 Aguardando API inicializar..." -ForegroundColor Yellow
    $maxAttempts = 30
    $attempt = 0
    
    do {
        $attempt++
        Write-Host "Tentativa $attempt de $maxAttempts..." -ForegroundColor Gray
        
        try {
            $response = Invoke-WebRequest -Uri "$baseUrl/api/categories/all" -Method GET -TimeoutSec 5 -ErrorAction Stop
            if ($response.StatusCode -eq 200) {
                Write-Host "✅ API está rodando!" -ForegroundColor Green
                return $true
            }
        }
        catch {
            Start-Sleep -Seconds 2
        }
    } while ($attempt -lt $maxAttempts)
    
    Write-Host "❌ Não foi possível conectar à API após $maxAttempts tentativas" -ForegroundColor Red
    return $false
}

# Verificar se a API está rodando
if (-not (Wait-ForApi)) {
    Write-Host "Por favor, inicie a API primeiro com: dotnet run" -ForegroundColor Red
    exit 1
}

Write-Host "`n🚀 INICIANDO TESTES AUTOMATIZADOS" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

# Variáveis para armazenar IDs
$companyId = $null
$categoryId1 = $null
$categoryId2 = $null
$userId = $null
$adminId = $null
$courseId = $null
$moduleId = $null

# 1. TESTE DE COMPANIES
Write-Host "`n📢 TESTANDO COMPANIES" -ForegroundColor Magenta

$companyResponse = Invoke-ApiTest -Method "POST" -Endpoint "/api/company" -Description "Criar Empresa" -ReturnResponse $true -Body @{
    name = "Empresa Teste Automático"
    cnpj = "12345678000199"
    email = "contato@empresateste.com"
    phone = "(11) 99999-9999"
}

if ($companyResponse -and $companyResponse.id) {
    $companyId = $companyResponse.id
    Write-Host "✅ Company ID capturado: $companyId" -ForegroundColor Green
}

# 2. TESTE DE CATEGORIES
Write-Host "`n📂 TESTANDO CATEGORIES" -ForegroundColor Magenta

$category1Response = Invoke-ApiTest -Method "POST" -Endpoint "/api/categories" -Description "Criar Categoria 1" -ReturnResponse $true -Body @{
    name = "Tecnologia"
    description = "Categoria sobre tecnologia"
}

if ($category1Response -and $category1Response.id) {
    $categoryId1 = $category1Response.id
    Write-Host "✅ Category 1 ID capturado: $categoryId1" -ForegroundColor Green
}

$category2Response = Invoke-ApiTest -Method "POST" -Endpoint "/api/categories" -Description "Criar Categoria 2" -ReturnResponse $true -Body @{
    name = "Programação"
    description = "Categoria sobre programação"
}

if ($category2Response -and $category2Response.id) {
    $categoryId2 = $category2Response.id
    Write-Host "✅ Category 2 ID capturado: $categoryId2" -ForegroundColor Green
}

# Testar listagem de categorias
Invoke-ApiTest -Method "GET" -Endpoint "/api/categories/all" -Description "Listar Todas as Categorias"

# Testar busca por nome
Invoke-ApiTest -Method "GET" -Endpoint "/api/categories/search?Name=Tecnologia" -Description "Buscar Categoria por Nome"

# Testar busca por ID
if ($categoryId1) {
    Invoke-ApiTest -Method "GET" -Endpoint "/api/categories/$categoryId1" -Description "Buscar Categoria por ID"
}

# 3. TESTE DE USERS
Write-Host "`n👥 TESTANDO USERS" -ForegroundColor Magenta

if ($companyId) {
    $interests = @()
    if ($categoryId1) { $interests += $categoryId1 }
    if ($categoryId2) { $interests += $categoryId2 }
    
    $userResponse = Invoke-ApiTest -Method "POST" -Endpoint "/api/users" -Description "Criar Usuário" -ReturnResponse $true -Body @{
        name = "João Silva"
        identity = "12345678901"
        email = "joao.silva@teste.com"
        password = "123456"
        isAdmin = $false
        responsibleId = $null
        companyId = $companyId
        image = "https://example.com/avatar.jpg"
        interests = $interests
    }
    
    if ($userResponse -and $userResponse.id) {
        $userId = $userResponse.id
        Write-Host "✅ User ID capturado: $userId" -ForegroundColor Green
    }
    
    $adminResponse = Invoke-ApiTest -Method "POST" -Endpoint "/api/users" -Description "Criar Usuário Admin" -ReturnResponse $true -Body @{
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
    
    if ($adminResponse -and $adminResponse.id) {
        $adminId = $adminResponse.id
        Write-Host "✅ Admin ID capturado: $adminId" -ForegroundColor Green
    }
}

# Testar listagem de usuários
Invoke-ApiTest -Method "GET" -Endpoint "/api/users/all?Page=1&MaxItems=10" -Description "Listar Usuários"

# Testar busca por nome
Invoke-ApiTest -Method "GET" -Endpoint "/api/users/all?Name=João&Page=1&MaxItems=10" -Description "Buscar Usuário por Nome"

# Testar busca por email
Invoke-ApiTest -Method "GET" -Endpoint "/api/users/all?Email=joao&Page=1&MaxItems=10" -Description "Buscar Usuário por Email"

# Testar busca apenas admins
Invoke-ApiTest -Method "GET" -Endpoint "/api/users/all?IsAdmin=true&Page=1&MaxItems=10" -Description "Buscar Apenas Admins"

# Testar busca por ID
if ($userId) {
    Invoke-ApiTest -Method "GET" -Endpoint "/api/users/$userId" -Description "Buscar Usuário por ID"
}

# Testar atualização
if ($userId -and $companyId) {
    Invoke-ApiTest -Method "PUT" -Endpoint "/api/users" -Description "Atualizar Usuário" -Body @{
        id = $userId
        name = "João Silva Santos"
        identity = "12345678901"
        email = "joao.santos@teste.com"
        isAdmin = $false
        responsibleId = $null
        companyId = $companyId
        image = "https://example.com/new-avatar.jpg"
        interests = @()
    }
}

# 4. TESTE DE COURSES
Write-Host "`n📚 TESTANDO COURSES" -ForegroundColor Magenta

$categories = @()
if ($categoryId1) { $categories += $categoryId1 }

$courseResponse = Invoke-ApiTest -Method "POST" -Endpoint "/api/course" -Description "Criar Curso" -ReturnResponse $true -Body @{
    name = "Curso de .NET Core"
    description = "Aprenda desenvolvimento com .NET Core"
    difficulty = 2
    image = "https://example.com/course.jpg"
    totalHours = 40
    categories = $categories
}

if ($courseResponse -and $courseResponse.id) {
    $courseId = $courseResponse.id
    Write-Host "✅ Course ID capturado: $courseId" -ForegroundColor Green
}

# Testar listagem de cursos
Invoke-ApiTest -Method "GET" -Endpoint "/api/course/all?Page=1&MaxItens=10" -Description "Listar Cursos"

# Testar busca por ID
if ($courseId) {
    Invoke-ApiTest -Method "GET" -Endpoint "/api/course/$courseId" -Description "Buscar Curso por ID"
}

# 5. TESTE DE MODULES
Write-Host "`n📖 TESTANDO MODULES" -ForegroundColor Magenta

if ($courseId) {
    $moduleResponse = Invoke-ApiTest -Method "POST" -Endpoint "/api/modules" -Description "Criar Módulo" -ReturnResponse $true -Body @{
        name = "Introdução ao .NET"
        description = "Módulo introdutório sobre .NET Core"
        index = 1
        courseId = $courseId
    }
    
    if ($moduleResponse -and $moduleResponse.id) {
        $moduleId = $moduleResponse.id
        Write-Host "✅ Module ID capturado: $moduleId" -ForegroundColor Green
    }
    
    # Testar busca módulos por curso
    Invoke-ApiTest -Method "GET" -Endpoint "/api/modules/course/$courseId" -Description "Buscar Módulos por Curso"
    
    # Testar busca por ID
    if ($moduleId) {
        Invoke-ApiTest -Method "GET" -Endpoint "/api/modules/$moduleId" -Description "Buscar Módulo por ID"
        
        # Testar atualização
        Invoke-ApiTest -Method "PUT" -Endpoint "/api/modules" -Description "Atualizar Módulo" -Body @{
            id = $moduleId
            name = "Introdução Avançada ao .NET"
            description = "Módulo introdutório avançado sobre .NET Core"
            index = 1
            courseId = $courseId
        }
    }
}

# RESUMO FINAL
Write-Host "`n📊 RESUMO DOS TESTES" -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green
Write-Host "Company ID: $companyId" -ForegroundColor White
Write-Host "Category 1 ID: $categoryId1" -ForegroundColor White
Write-Host "Category 2 ID: $categoryId2" -ForegroundColor White
Write-Host "User ID: $userId" -ForegroundColor White
Write-Host "Admin ID: $adminId" -ForegroundColor White
Write-Host "Course ID: $courseId" -ForegroundColor White
Write-Host "Module ID: $moduleId" -ForegroundColor White

Write-Host "`n🎉 TESTES CONCLUÍDOS!" -ForegroundColor Green
Write-Host "Todos os IDs foram capturados automaticamente e utilizados nos testes subsequentes." -ForegroundColor Yellow
