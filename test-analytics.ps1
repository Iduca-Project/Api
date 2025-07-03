# Script para testar as rotas de Analytics da API Iduca
# Requer que o servidor esteja rodando e que existam dados de teste

$baseUrl = "http://localhost:5284"
$adminEmail = "admin@iduca.com"
$adminPassword = "123456"

Write-Host "=== TESTE DAS ROTAS DE ANALYTICS ===" -ForegroundColor Cyan
Write-Host ""

# 1. Login do administrador
Write-Host "1. Fazendo login como administrador..." -ForegroundColor Yellow

$loginPayload = @{
    email = $adminEmail
    password = $adminPassword
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginPayload -ContentType "application/json"
    $adminToken = $loginResponse.token
    Write-Host "✅ Login do admin realizado com sucesso!" -ForegroundColor Green
    Write-Host "Token: $($adminToken.Substring(0, 50))..." -ForegroundColor Gray
} catch {
    Write-Host "❌ Falha no login do admin: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

$headers = @{
    "Authorization" = "Bearer $adminToken"
    "Content-Type" = "application/json"
}

Write-Host ""

# 2. Listar empresas para obter IDs
Write-Host "2. Obtendo lista de empresas..." -ForegroundColor Yellow

try {
    $companiesResponse = Invoke-RestMethod -Uri "$baseUrl/api/companies/all?page=1&maxItems=5" -Method GET -Headers $headers
    
    if ($companiesResponse.companies -and $companiesResponse.companies.Count -gt 0) {
        $companyId = $companiesResponse.companies[0].id
        $companyName = $companiesResponse.companies[0].name
        Write-Host "✅ Empresa encontrada: $companyName (ID: $companyId)" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Nenhuma empresa encontrada. Criando empresa de teste..." -ForegroundColor Yellow
        
        $companyPayload = @{
            name = "Empresa Teste Analytics"
            description = "Empresa criada para teste de analytics"
            image = "https://example.com/logo.png"
            address = "Rua Teste, 123"
        } | ConvertTo-Json
        
        $createCompanyResponse = Invoke-RestMethod -Uri "$baseUrl/api/companies" -Method POST -Body $companyPayload -Headers $headers
        $companyId = $createCompanyResponse.id
        $companyName = $createCompanyResponse.name
        Write-Host "✅ Empresa criada: $companyName (ID: $companyId)" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ Falha ao obter empresas: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""

# 3. Listar categorias para obter IDs
Write-Host "3. Obtendo lista de categorias..." -ForegroundColor Yellow

try {
    $categoriesResponse = Invoke-RestMethod -Uri "$baseUrl/api/categories/all?page=1&maxItems=5" -Method GET -Headers $headers
    
    if ($categoriesResponse.categories -and $categoriesResponse.categories.Count -gt 0) {
        $categoryId = $categoriesResponse.categories[0].id
        $categoryName = $categoriesResponse.categories[0].name
        Write-Host "✅ Categoria encontrada: $categoryName (ID: $categoryId)" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Nenhuma categoria encontrada. Pulando teste de analytics de categoria." -ForegroundColor Yellow
        $categoryId = $null
    }
} catch {
    Write-Host "❌ Falha ao obter categorias: $($_.Exception.Message)" -ForegroundColor Red
    $categoryId = $null
}

Write-Host ""

# 4. Testar estatísticas da empresa
Write-Host "4. Testando estatísticas da empresa..." -ForegroundColor Yellow

try {
    $companyStatsResponse = Invoke-RestMethod -Uri "$baseUrl/api/analytics/companies/$companyId" -Method GET -Headers $headers
    
    Write-Host "✅ Estatísticas da empresa obtidas com sucesso!" -ForegroundColor Green
    Write-Host "   📊 Empresa: $($companyStatsResponse.companyName)" -ForegroundColor Cyan
    Write-Host "   👥 Total de usuários: $($companyStatsResponse.statistics.totalUsers)" -ForegroundColor Cyan
    Write-Host "   📚 Total de cursos: $($companyStatsResponse.statistics.totalCourses)" -ForegroundColor Cyan
    Write-Host "   🎓 Total de matrículas: $($companyStatsResponse.statistics.totalEnrollments)" -ForegroundColor Cyan
    Write-Host "   ✅ Cursos completados: $($companyStatsResponse.statistics.completedCourses)" -ForegroundColor Cyan
    Write-Host "   📈 Taxa média de conclusão: $($companyStatsResponse.statistics.averageCompletionRate)%" -ForegroundColor Cyan
    Write-Host "   ⏱️  Horas totais de aprendizado: $($companyStatsResponse.statistics.totalHoursLearned)h" -ForegroundColor Cyan
    Write-Host "   🟢 Usuários ativos: $($companyStatsResponse.statistics.activeUsers)" -ForegroundColor Cyan
    
    if ($companyStatsResponse.courseBreakdown -and $companyStatsResponse.courseBreakdown.Count -gt 0) {
        Write-Host "   📋 Breakdown de cursos: $($companyStatsResponse.courseBreakdown.Count) cursos" -ForegroundColor Cyan
    }
    
    if ($companyStatsResponse.categoryBreakdown -and $companyStatsResponse.categoryBreakdown.Count -gt 0) {
        Write-Host "   🏷️  Breakdown de categorias: $($companyStatsResponse.categoryBreakdown.Count) categorias" -ForegroundColor Cyan
    }
    
} catch {
    Write-Host "❌ Falha ao obter estatísticas da empresa: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Resposta: $($_.Exception.Response.StatusCode) - $($_.Exception.Response.StatusDescription)" -ForegroundColor Red
}

Write-Host ""

# 5. Testar estatísticas da categoria (se disponível)
if ($categoryId) {
    Write-Host "5. Testando estatísticas da categoria..." -ForegroundColor Yellow
    
    try {
        $categoryStatsResponse = Invoke-RestMethod -Uri "$baseUrl/api/analytics/categories/$categoryId" -Method GET -Headers $headers
        
        Write-Host "✅ Estatísticas da categoria obtidas com sucesso!" -ForegroundColor Green
        Write-Host "   🏷️  Categoria: $($categoryStatsResponse.categoryName)" -ForegroundColor Cyan
        Write-Host "   📚 Total de cursos: $($categoryStatsResponse.analytics.totalCourses)" -ForegroundColor Cyan
        Write-Host "   🎓 Total de matrículas: $($categoryStatsResponse.analytics.totalEnrollments)" -ForegroundColor Cyan
        Write-Host "   ✅ Matrículas completadas: $($categoryStatsResponse.analytics.completedEnrollments)" -ForegroundColor Cyan
        Write-Host "   📈 Taxa média de conclusão: $($categoryStatsResponse.analytics.averageCompletionRate)%" -ForegroundColor Cyan
        Write-Host "   ⏱️  Horas totais de conteúdo: $($categoryStatsResponse.analytics.totalHoursContent)h" -ForegroundColor Cyan
        Write-Host "   👥 Usuários únicos: $($categoryStatsResponse.analytics.uniqueUsers)" -ForegroundColor Cyan
        Write-Host "   📊 Rank de popularidade: $($categoryStatsResponse.analytics.popularityRank)%" -ForegroundColor Cyan
        
        if ($categoryStatsResponse.topCourses -and $categoryStatsResponse.topCourses.Count -gt 0) {
            Write-Host "   🏆 Top cursos: $($categoryStatsResponse.topCourses.Count) cursos" -ForegroundColor Cyan
        }
        
        if ($categoryStatsResponse.companyBreakdown -and $categoryStatsResponse.companyBreakdown.Count -gt 0) {
            Write-Host "   🏢 Breakdown de empresas: $($categoryStatsResponse.companyBreakdown.Count) empresas" -ForegroundColor Cyan
        }
        
    } catch {
        Write-Host "❌ Falha ao obter estatísticas da categoria: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "   Resposta: $($_.Exception.Response.StatusCode) - $($_.Exception.Response.StatusDescription)" -ForegroundColor Red
    }
} else {
    Write-Host "5. ⏭️  Pulando teste de categoria (não disponível)" -ForegroundColor Yellow
}

Write-Host ""

# 6. Testar estatísticas com filtro de data
Write-Host "6. Testando estatísticas com filtro de data..." -ForegroundColor Yellow

$startDate = "2024-01-01"
$endDate = "2024-12-31"

try {
    $filteredStatsResponse = Invoke-RestMethod -Uri "$baseUrl/api/analytics/companies/$companyId" -Method GET -Headers $headers -Body @{
        startDate = $startDate
        endDate = $endDate
    }
    
    Write-Host "✅ Estatísticas com filtro de data obtidas com sucesso!" -ForegroundColor Green
    Write-Host "   📅 Período: $startDate até $endDate" -ForegroundColor Cyan
    Write-Host "   🎓 Matrículas no período: $($filteredStatsResponse.statistics.totalEnrollments)" -ForegroundColor Cyan
    
} catch {
    Write-Host "❌ Falha ao obter estatísticas com filtro: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== TESTE DE ANALYTICS CONCLUÍDO ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Resumo dos testes realizados:" -ForegroundColor Yellow
Write-Host "• Login de administrador" -ForegroundColor White
Write-Host "• Listagem de empresas e categorias" -ForegroundColor White  
Write-Host "• Estatísticas detalhadas da empresa" -ForegroundColor White
Write-Host "• Estatísticas detalhadas da categoria" -ForegroundColor White
Write-Host "• Filtros por data nas estatísticas" -ForegroundColor White
Write-Host ""
Write-Host "💡 Para obter dados mais ricos, certifique-se de que existem:" -ForegroundColor Yellow
Write-Host "   - Usuários matriculados em cursos" -ForegroundColor Gray
Write-Host "   - Lições completadas pelos usuários" -ForegroundColor Gray
Write-Host "   - Múltiplas empresas e categorias" -ForegroundColor Gray
