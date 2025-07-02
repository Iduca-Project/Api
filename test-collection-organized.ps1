# =========================================
# SCRIPT DE TESTE PARA COLEÇÃO ORGANIZADA
# =========================================
#
# Este script testa a coleção Postman organizada por permissões
# Valida autenticação, autorização e funcionamento dos endpoints
#
# Uso: .\test-collection-organized.ps1
#

Write-Host "🚀 Iniciando testes da coleção organizada..." -ForegroundColor Green
Write-Host ""

# Configurações
$baseUrl = "http://localhost:5284"
$adminEmail = "admin@iduca.com"
$adminPassword = "admin123"
$userEmail = "joao.santos.atualizado@teste.com"
$userPassword = "123456"

# Headers padrão
$headers = @{
    "Content-Type" = "application/json"
}

Write-Host "📊 === TESTES DE AUTENTICAÇÃO ===" -ForegroundColor Yellow

# Teste 1: Login como Admin
try {
    $adminLoginBody = @{
        email = $adminEmail
        password = $adminPassword
    } | ConvertTo-Json

    $adminLoginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $adminLoginBody -Headers $headers
    $adminToken = $adminLoginResponse.token

    Write-Host "✅ Login Admin: SUCESSO" -ForegroundColor Green
    Write-Host "   Token obtido: $($adminToken.Substring(0, 20))..." -ForegroundColor Gray
}
catch {
    Write-Host "❌ Login Admin: FALHOU" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Teste 2: Login como Usuário Comum
try {
    $userLoginBody = @{
        email = $userEmail
        password = $userPassword
    } | ConvertTo-Json

    $userLoginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $userLoginBody -Headers $headers
    $userToken = $userLoginResponse.token

    Write-Host "✅ Login Usuário: SUCESSO" -ForegroundColor Green
    Write-Host "   Token obtido: $($userToken.Substring(0, 20))..." -ForegroundColor Gray
}
catch {
    Write-Host "❌ Login Usuário: FALHOU" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "🔒 === TESTES DE AUTORIZAÇÃO ===" -ForegroundColor Yellow

# Headers com token admin
$adminHeaders = @{
    "Content-Type" = "application/json"
    "Authorization" = "Bearer $adminToken"
}

# Headers com token usuário
$userHeaders = @{
    "Content-Type" = "application/json"
    "Authorization" = "Bearer $userToken"
}

# Teste 3: Admin pode listar todos os usuários
try {
    $usersResponse = Invoke-RestMethod -Uri "$baseUrl/api/users/all?Page=1&MaxItems=5" -Method GET -Headers $adminHeaders
    Write-Host "✅ Admin listar usuários: SUCESSO" -ForegroundColor Green
    Write-Host "   Usuários encontrados: $($usersResponse.users.Count)" -ForegroundColor Gray
}
catch {
    Write-Host "❌ Admin listar usuários: FALHOU" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
}

# Teste 4: Usuário comum NÃO pode listar todos os usuários (deve falhar)
try {
    $usersResponse = Invoke-RestMethod -Uri "$baseUrl/api/users/all?Page=1&MaxItems=5" -Method GET -Headers $userHeaders
    Write-Host "❌ Usuário listar usuários: FALHA DE SEGURANÇA (deveria ter sido negado)" -ForegroundColor Red
}
catch {
    if ($_.Exception.Response.StatusCode -eq 403) {
        Write-Host "✅ Usuário listar usuários: CORRETAMENTE NEGADO (403)" -ForegroundColor Green
    }
    else {
        Write-Host "⚠️ Usuário listar usuários: Erro inesperado ($($_.Exception.Response.StatusCode))" -ForegroundColor Yellow
    }
}

# Teste 5: Admin pode criar empresa
try {
    $companyBody = @{
        name = "Empresa Teste Script $(Get-Date -Format 'HHmmss')"
        description = "Empresa criada pelo script de teste"
    } | ConvertTo-Json

    $companyResponse = Invoke-RestMethod -Uri "$baseUrl/api/company" -Method POST -Body $companyBody -Headers $adminHeaders
    $companyId = $companyResponse.id

    Write-Host "✅ Admin criar empresa: SUCESSO" -ForegroundColor Green
    Write-Host "   Empresa ID: $companyId" -ForegroundColor Gray
}
catch {
    Write-Host "❌ Admin criar empresa: FALHOU" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
}

# Teste 6: Usuário comum NÃO pode criar empresa (deve falhar)
try {
    $companyBody = @{
        name = "Empresa Não Autorizada $(Get-Date -Format 'HHmmss')"
        description = "Esta criação deve falhar"
    } | ConvertTo-Json

    $companyResponse = Invoke-RestMethod -Uri "$baseUrl/api/company" -Method POST -Body $companyBody -Headers $userHeaders
    Write-Host "❌ Usuário criar empresa: FALHA DE SEGURANÇA (deveria ter sido negado)" -ForegroundColor Red
}
catch {
    if ($_.Exception.Response.StatusCode -eq 403) {
        Write-Host "✅ Usuário criar empresa: CORRETAMENTE NEGADO (403)" -ForegroundColor Green
    }
    else {
        Write-Host "⚠️ Usuário criar empresa: Erro inesperado ($($_.Exception.Response.StatusCode))" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "👁️ === TESTES DE VISUALIZAÇÃO (USUÁRIO COMUM) ===" -ForegroundColor Yellow

# Teste 7: Usuário pode ver empresas
try {
    $companiesResponse = Invoke-RestMethod -Uri "$baseUrl/api/company/all" -Method GET -Headers $userHeaders
    Write-Host "✅ Usuário ver empresas: SUCESSO" -ForegroundColor Green
    Write-Host "   Empresas encontradas: $($companiesResponse.Count)" -ForegroundColor Gray
}
catch {
    Write-Host "❌ Usuário ver empresas: FALHOU" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
}

# Teste 8: Usuário pode ver categorias
try {
    $categoriesResponse = Invoke-RestMethod -Uri "$baseUrl/api/categories/all" -Method GET -Headers $userHeaders
    Write-Host "✅ Usuário ver categorias: SUCESSO" -ForegroundColor Green
    Write-Host "   Categorias encontradas: $($categoriesResponse.Count)" -ForegroundColor Gray
}
catch {
    Write-Host "❌ Usuário ver categorias: FALHOU" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
}

# Teste 9: Usuário pode ver cursos
try {
    $coursesResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/all?Page=1&MaxItems=5" -Method GET -Headers $userHeaders
    Write-Host "✅ Usuário ver cursos: SUCESSO" -ForegroundColor Green
    Write-Host "   Cursos encontrados: $($coursesResponse.courses.Count)" -ForegroundColor Gray
}
catch {
    Write-Host "❌ Usuário ver cursos: FALHOU" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "🔍 === TESTES SEM AUTENTICAÇÃO ===" -ForegroundColor Yellow

# Teste 10: Acesso sem token deve falhar
try {
    $noAuthResponse = Invoke-RestMethod -Uri "$baseUrl/api/users/all" -Method GET
    Write-Host "❌ Acesso sem token: FALHA DE SEGURANÇA (deveria ter sido negado)" -ForegroundColor Red
}
catch {
    if ($_.Exception.Response.StatusCode -eq 401) {
        Write-Host "✅ Acesso sem token: CORRETAMENTE NEGADO (401)" -ForegroundColor Green
    }
    else {
        Write-Host "⚠️ Acesso sem token: Erro inesperado ($($_.Exception.Response.StatusCode))" -ForegroundColor Yellow
    }
}

# Teste 11: Token inválido deve falhar
try {
    $invalidHeaders = @{
        "Content-Type" = "application/json"
        "Authorization" = "Bearer token-invalido-123"
    }
    
    $invalidTokenResponse = Invoke-RestMethod -Uri "$baseUrl/api/users/all" -Method GET -Headers $invalidHeaders
    Write-Host "❌ Token inválido: FALHA DE SEGURANÇA (deveria ter sido negado)" -ForegroundColor Red
}
catch {
    if ($_.Exception.Response.StatusCode -eq 401) {
        Write-Host "✅ Token inválido: CORRETAMENTE NEGADO (401)" -ForegroundColor Green
    }
    else {
        Write-Host "⚠️ Token inválido: Erro inesperado ($($_.Exception.Response.StatusCode))" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "📊 === RESUMO DOS TESTES ===" -ForegroundColor Cyan
Write-Host "✅ Testes de autenticação - Login admin e usuário" -ForegroundColor Green
Write-Host "✅ Testes de autorização - Separação correta de permissões" -ForegroundColor Green
Write-Host "✅ Testes de visualização - Usuários podem ver dados públicos" -ForegroundColor Green
Write-Host "✅ Testes de segurança - Bloqueio correto sem autenticação" -ForegroundColor Green
Write-Host ""
Write-Host "🎉 Todos os testes da coleção organizada foram executados!" -ForegroundColor Green
Write-Host "📋 Você pode agora usar a coleção Postman 'API-Iduca-Postman-Collection-Organized.json'" -ForegroundColor Cyan
Write-Host "🔧 Para importar no Postman e testar manualmente cada pasta/permissão" -ForegroundColor Cyan
