# Script para testar o PATCH de curso com atualização de categorias
$baseUrl = "http://localhost:5284"

Write-Host "=== TESTE DO PATCH DE CURSO COM CATEGORIAS ===" -ForegroundColor Green

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
} catch {
    Write-Host "❌ Erro no login: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

$headers = @{
    "Authorization" = "Bearer $adminToken"
    "Content-Type" = "application/json"
}

# 2. Listar categorias para pegar IDs
Write-Host "`n2. Listando categorias..." -ForegroundColor Yellow
try {
    $categoriesResponse = Invoke-RestMethod -Uri "$baseUrl/api/categories/all" -Method GET -Headers $headers
    if ($categoriesResponse -and $categoriesResponse.Count -gt 0) {
        $categoryId1 = $categoriesResponse[0].id
        Write-Host "✅ Categorias encontradas. Primeira categoria: $categoryId1" -ForegroundColor Green
        
        # Se tiver mais de uma categoria, pegar a segunda também
        $categoryId2 = if ($categoriesResponse.Count -gt 1) { $categoriesResponse[1].id } else { $categoryId1 }
    } else {
        Write-Host "❌ Nenhuma categoria encontrada" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "❌ Erro ao listar categorias: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 3. Criar um curso para testar
Write-Host "`n3. Criando curso para teste..." -ForegroundColor Yellow
$courseData = @{
    name = "Curso de Teste PATCH - $(Get-Date -Format 'HHmmss')"
    description = "Curso criado para testar PATCH de categorias"
    difficulty = 1
    image = "https://example.com/curso-patch-test.jpg"
    totalHours = 30
    categories = @($categoryId1)
} | ConvertTo-Json

try {
    $courseResponse = Invoke-RestMethod -Uri "$baseUrl/api/course" -Method POST -Body $courseData -Headers $headers
    $courseId = $courseResponse.id
    Write-Host "✅ Curso criado com sucesso: $courseId" -ForegroundColor Green
    Write-Host "Categorias iniciais: $($courseData | ConvertFrom-Json | Select-Object -ExpandProperty categories)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro ao criar curso: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 4. Verificar o curso criado
Write-Host "`n4. Verificando curso criado..." -ForegroundColor Yellow
try {
    $courseDetailsResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/$courseId" -Method GET -Headers $headers
    Write-Host "✅ Curso encontrado:" -ForegroundColor Green
    Write-Host "Nome: $($courseDetailsResponse.name)" -ForegroundColor Cyan
    Write-Host "Categorias atuais: $($courseDetailsResponse.categories | ConvertTo-Json)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro ao buscar curso: $($_.Exception.Message)" -ForegroundColor Red
}

# 5. Fazer PATCH do curso atualizando as categorias
Write-Host "`n5. Atualizando curso via PATCH (mudando categorias)..." -ForegroundColor Yellow
$patchData = @{
    id = $courseId
    name = "Curso de Teste PATCH - ATUALIZADO"
    description = "Curso atualizado com novas categorias"
    difficulty = 2
    image = "https://example.com/curso-patch-updated.jpg"
    totalHours = 45
    categories = @($categoryId2)  # Mudando para a segunda categoria
} | ConvertTo-Json

try {
    $patchResponse = Invoke-RestMethod -Uri "$baseUrl/api/course" -Method PATCH -Body $patchData -Headers $headers
    Write-Host "✅ PATCH realizado com sucesso!" -ForegroundColor Green
    Write-Host "Response: $($patchResponse | ConvertTo-Json -Depth 3)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro no PATCH: $($_.Exception.Message)" -ForegroundColor Red
    
    # Tentar obter mais detalhes do erro
    try {
        $errorDetails = $_.ErrorDetails.Message | ConvertFrom-Json
        Write-Host "Detalhes do erro: $($errorDetails | ConvertTo-Json -Depth 3)" -ForegroundColor Red
    } catch {
        Write-Host "Erro adicional ao processar detalhes: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 6. Verificar se as categorias foram atualizadas
Write-Host "`n6. Verificando se as categorias foram atualizadas..." -ForegroundColor Yellow
try {
    $updatedCourseResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/$courseId" -Method GET -Headers $headers
    Write-Host "✅ Curso após atualização:" -ForegroundColor Green
    Write-Host "Nome: $($updatedCourseResponse.name)" -ForegroundColor Cyan
    Write-Host "Dificuldade: $($updatedCourseResponse.difficulty)" -ForegroundColor Cyan
    Write-Host "Total de horas: $($updatedCourseResponse.totalHours)" -ForegroundColor Cyan
    Write-Host "Categorias após PATCH: $($updatedCourseResponse.categories | ConvertTo-Json)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro ao verificar curso atualizado: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== TESTE CONCLUÍDO ===" -ForegroundColor Green
