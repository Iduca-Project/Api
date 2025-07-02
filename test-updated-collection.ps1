# Script para testar a coleção atualizada e a listagem de cursos
$baseUrl = "http://localhost:5284"

Write-Host "=== TESTE DA COLEÇÃO ATUALIZADA - API IDUCA ===" -ForegroundColor Green

# 1. Fazer login como admin
Write-Host "`n1. Fazendo login como admin..." -ForegroundColor Yellow
$loginData = @{
    email = "admin@iduca.com"
    password = "admin123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginData -ContentType "application/json"
    $adminToken = $loginResponse.token
    Write-Host "✅ Login realizado com sucesso" -ForegroundColor Green
    Write-Host "Token: $($adminToken.Substring(0, 50))..." -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro no login: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 2. Testar listagem de cursos (problema reportado)
Write-Host "`n2. Testando listagem de cursos com paginação..." -ForegroundColor Yellow
$headers = @{
    "Authorization" = "Bearer $adminToken"
    "Content-Type" = "application/json"
}

try {
    $coursesResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/all?Page=1&MaxItems=10" -Method GET -Headers $headers
    Write-Host "✅ Listagem de cursos funcionando!" -ForegroundColor Green
    Write-Host "Response: $($coursesResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro na listagem de cursos: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Status Code: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

# 3. Testar criação de categoria para ter dados para os cursos
Write-Host "`n3. Criando categoria de teste..." -ForegroundColor Yellow
$categoryData = @{
    name = "Tecnologia - Teste $(Get-Date -Format 'HHmmss')"
} | ConvertTo-Json

try {
    $categoryResponse = Invoke-RestMethod -Uri "$baseUrl/api/categories" -Method POST -Body $categoryData -Headers $headers
    $categoryId = $categoryResponse.id
    Write-Host "✅ Categoria criada com sucesso: $categoryId" -ForegroundColor Green
} catch {
    Write-Host "❌ Erro ao criar categoria: $($_.Exception.Message)" -ForegroundColor Red
    $categoryId = $null
}

# 4. Testar criação de curso
if ($categoryId) {
    Write-Host "`n4. Criando curso de teste..." -ForegroundColor Yellow
    $courseData = @{
        name = "Curso de Teste - $(Get-Date -Format 'HHmmss')"
        description = "Curso criado automaticamente para teste"
        difficulty = 1
        image = "https://example.com/curso-teste.jpg"
        totalHours = 40
        categories = @($categoryId)
    } | ConvertTo-Json

    try {
        $courseResponse = Invoke-RestMethod -Uri "$baseUrl/api/course" -Method POST -Body $courseData -Headers $headers
        $courseId = $courseResponse.id
        Write-Host "✅ Curso criado com sucesso: $courseId" -ForegroundColor Green
    } catch {
        Write-Host "❌ Erro ao criar curso: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 5. Testar listagem de cursos novamente
Write-Host "`n5. Testando listagem de cursos novamente..." -ForegroundColor Yellow
try {
    $coursesResponse2 = Invoke-RestMethod -Uri "$baseUrl/api/course/all?Page=1&MaxItems=10" -Method GET -Headers $headers
    Write-Host "✅ Segunda listagem de cursos funcionando!" -ForegroundColor Green
    if ($coursesResponse2.courses -and $coursesResponse2.courses.Count -gt 0) {
        Write-Host "Total de cursos encontrados: $($coursesResponse2.courses.Count)" -ForegroundColor Cyan
    } else {
        Write-Host "⚠️  Nenhum curso encontrado na listagem" -ForegroundColor Yellow
    }
} catch {
    Write-Host "❌ Erro na segunda listagem de cursos: $($_.Exception.Message)" -ForegroundColor Red
}

# 6. Testar PATCH de empresa (problema reportado anteriormente)
Write-Host "`n6. Testando PATCH de empresa..." -ForegroundColor Yellow
try {
    $companiesResponse = Invoke-RestMethod -Uri "$baseUrl/api/company/all" -Method GET -Headers $headers
    if ($companiesResponse -and $companiesResponse.Count -gt 0) {
        $companyId = $companiesResponse[0].id
        $patchData = @{
            name = "Empresa Atualizada - $(Get-Date -Format 'HHmmss')"
            description = "Descrição atualizada via PATCH"
        } | ConvertTo-Json
        
        $patchResponse = Invoke-RestMethod -Uri "$baseUrl/api/company/$companyId" -Method PATCH -Body $patchData -Headers $headers
        Write-Host "✅ PATCH de empresa funcionando!" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Nenhuma empresa encontrada para testar PATCH" -ForegroundColor Yellow
    }
} catch {
    Write-Host "❌ Erro no PATCH de empresa: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== TESTE CONCLUÍDO ===" -ForegroundColor Green
Write-Host "📝 Coleção Postman atualizada salva em: API-Iduca-Postman-Collection-Updated.json" -ForegroundColor Cyan
