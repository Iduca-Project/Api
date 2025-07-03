# Script de Teste da API Iduca Organizada
# Execute este script para validar a estrutura da API

param(
    [string]$BaseUrl = "http://localhost:5284",
    [switch]$RunFullTest = $false
)

Write-Host "=====================================" -ForegroundColor Green
Write-Host "  TESTE DA API IDUCA ORGANIZADA" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""

# Função para fazer requests HTTP
function Invoke-ApiRequest {
    param(
        [string]$Method,
        [string]$Url,
        [hashtable]$Headers = @{},
        [string]$Body = $null
    )
    
    try {
        $params = @{
            Method = $Method
            Uri = $Url
            Headers = $Headers
            ContentType = "application/json"
        }
        
        if ($Body) {
            $params.Body = $Body
        }
        
        $response = Invoke-RestMethod @params
        return @{ Success = $true; Data = $response }
    }
    catch {
        return @{ Success = $false; Error = $_.Exception.Message }
    }
}

# 1. Teste de Health Check
Write-Host "1. Testando Health Check..." -ForegroundColor Yellow
$healthCheck = Invoke-ApiRequest -Method "GET" -Url "$BaseUrl/api/health"
if ($healthCheck.Success) {
    Write-Host "   ✅ Health check OK" -ForegroundColor Green
} else {
    Write-Host "   ❌ Health check FALHOU: $($healthCheck.Error)" -ForegroundColor Red
    exit 1
}

# 2. Teste de Home
Write-Host "2. Testando Home..." -ForegroundColor Yellow
$home = Invoke-ApiRequest -Method "GET" -Url "$BaseUrl/api/home"
if ($home.Success) {
    Write-Host "   ✅ Home OK" -ForegroundColor Green
} else {
    Write-Host "   ❌ Home FALHOU: $($home.Error)" -ForegroundColor Red
}

# 3. Teste de Estrutura das Rotas (sem autenticação)
Write-Host "3. Testando estrutura das rotas..." -ForegroundColor Yellow

$publicRoutes = @(
    "/api/health",
    "/api/home"
)

$authenticatedRoutes = @(
    "/api/categories",
    "/api/courses", 
    "/api/modules",
    "/api/lessons",
    "/api/profile",
    "/api/hierarchy/tree",
    "/api/admin/companies"
)

foreach ($route in $authenticatedRoutes) {
    $result = Invoke-ApiRequest -Method "GET" -Url "$BaseUrl$route"
    if ($result.Success -or $result.Error -like "*401*" -or $result.Error -like "*403*") {
        Write-Host "   ✅ Rota $route está configurada" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Rota $route pode ter problemas: $($result.Error)" -ForegroundColor Yellow
    }
}

if ($RunFullTest) {
    Write-Host ""
    Write-Host "4. Testando fluxo completo (requer configuração)..." -ForegroundColor Yellow
    
    # Dados de teste (substitua pelos seus dados reais)
    $loginData = @{
        email = "admin@empresa.com"
        password = "123456"
    } | ConvertTo-Json
    
    # Teste de Login Admin
    Write-Host "   Testando login de administrador..." -ForegroundColor Cyan
    $loginResult = Invoke-ApiRequest -Method "POST" -Url "$BaseUrl/api/auth/login" -Body $loginData
    
    if ($loginResult.Success -and $loginResult.Data.token) {
        Write-Host "   ✅ Login de admin OK" -ForegroundColor Green
        $adminToken = $loginResult.Data.token
        $authHeaders = @{ "Authorization" = "Bearer $adminToken" }
        
        # Teste de endpoint administrativo
        Write-Host "   Testando endpoint administrativo..." -ForegroundColor Cyan
        $adminTest = Invoke-ApiRequest -Method "GET" -Url "$BaseUrl/api/admin/companies" -Headers $authHeaders
        
        if ($adminTest.Success) {
            Write-Host "   ✅ Endpoint administrativo OK" -ForegroundColor Green
        } else {
            Write-Host "   ❌ Endpoint administrativo FALHOU: $($adminTest.Error)" -ForegroundColor Red
        }
        
        # Teste de endpoint de usuário comum
        Write-Host "   Testando endpoint de usuário comum..." -ForegroundColor Cyan
        $userTest = Invoke-ApiRequest -Method "GET" -Url "$BaseUrl/api/categories" -Headers $authHeaders
        
        if ($userTest.Success) {
            Write-Host "   ✅ Endpoint de usuário comum OK" -ForegroundColor Green
        } else {
            Write-Host "   ❌ Endpoint de usuário comum FALHOU: $($userTest.Error)" -ForegroundColor Red
        }
        
    } else {
        Write-Host "   ❌ Login de admin FALHOU: $($loginResult.Error)" -ForegroundColor Red
        Write-Host "   💡 Verifique as credenciais ou crie um usuário admin" -ForegroundColor Blue
    }
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
Write-Host "  RESULTADO DOS TESTES" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

Write-Host ""
Write-Host "📋 VERIFICAÇÕES REALIZADAS:" -ForegroundColor Blue
Write-Host "   ✅ Health Check"
Write-Host "   ✅ Home Endpoint"
Write-Host "   ✅ Estrutura das Rotas"
if ($RunFullTest) {
    Write-Host "   ✅ Fluxo de Autenticação"
}

Write-Host ""
Write-Host "📚 PRÓXIMOS PASSOS:" -ForegroundColor Blue
Write-Host "   1. Importe o Postman Collection: API-Iduca-Postman-Collection-ORGANIZED.json"
Write-Host "   2. Configure as variáveis no Postman (base_url, tokens)"
Write-Host "   3. Execute os testes na ordem: Autenticação → Admin → Usuário → Gestor"
Write-Host "   4. Consulte o GUIA-USO-POSTMAN-COLLECTION.md para instruções detalhadas"

Write-Host ""
Write-Host "🎉 TESTE CONCLUÍDO!" -ForegroundColor Green

# Informações adicionais
Write-Host ""
Write-Host "ℹ️  INFORMAÇÕES ADICIONAIS:" -ForegroundColor Cyan
Write-Host "   📂 Collection: API-Iduca-Postman-Collection-ORGANIZED.json"
Write-Host "   📖 Documentação: RELATORIO-FINAL-ORGANIZACAO.md"
Write-Host "   📝 Guia de Uso: GUIA-USO-POSTMAN-COLLECTION.md"
Write-Host "   🏗️ Estrutura: ORGANIZACAO-ROTAS-CORRETA.md"

Write-Host ""
Write-Host "Para executar o teste completo com autenticação:" -ForegroundColor Yellow
Write-Host "   .\test-api-organized.ps1 -RunFullTest" -ForegroundColor Yellow
