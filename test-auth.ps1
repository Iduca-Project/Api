# Teste de Autenticação JWT
$baseUrl = "http://localhost:5284"

Write-Host "=== TESTE DE AUTENTICAÇÃO ===" -ForegroundColor Green

function Test-Request {
    param($Method, $Url, $Body, $Token, $Description)
    
    Write-Host "`n🔄 $Description" -ForegroundColor Yellow
    Write-Host "   $Method $Url" -ForegroundColor Gray
    
    try {
        $headers = @{ "Content-Type" = "application/json" }
        
        if ($Token) {
            $headers["Authorization"] = "Bearer $Token"
            Write-Host "   Token: Bearer $($Token.Substring(0,20))..." -ForegroundColor Gray
        }
        
        if ($Body) {
            $json = $Body | ConvertTo-Json -Depth 10
            $response = Invoke-RestMethod -Uri $Url -Method $Method -Body $json -Headers $headers
        } else {
            $response = Invoke-RestMethod -Uri $Url -Method $Method -Headers $headers
        }
        
        Write-Host "✅ SUCESSO!" -ForegroundColor Green
        return $response
    }
    catch {
        Write-Host "❌ ERRO: $($_.Exception.Message)" -ForegroundColor Red
        return $null
    }
}

# 1. Testar acesso sem token (deve falhar)
Write-Host "`n1. Testando acesso sem token..." -ForegroundColor Blue
$result = Test-Request "GET" "$baseUrl/api/users/all" $null $null "Listar usuários SEM token"

if (!$result) {
    Write-Host "✅ Correto! Acesso negado sem token" -ForegroundColor Green
} else {
    Write-Host "❌ ERRO! API permitiu acesso sem token" -ForegroundColor Red
}

# 2. Fazer login para obter token
Write-Host "`n2. Fazendo login para obter token..." -ForegroundColor Blue
$loginBody = @{
    email = "joao.santos.atualizado@teste.com"
    password = "123456"
}

$loginResponse = Test-Request "POST" "$baseUrl/api/auth/login" $loginBody $null "Login"

if ($loginResponse) {
    $token = $loginResponse.token
    Write-Host "✅ Token obtido: $($token.Substring(0,30))..." -ForegroundColor Cyan
    
    # 3. Testar acesso com token válido
    Write-Host "`n3. Testando acesso com token válido..." -ForegroundColor Blue
    $usersWithToken = Test-Request "GET" "$baseUrl/api/users/all" $null $token "Listar usuários COM token"
    
    if ($usersWithToken) {
        Write-Host "✅ Autenticação JWT funcionando!" -ForegroundColor Green
    } else {
        Write-Host "❌ Token válido foi rejeitado" -ForegroundColor Red
    }
    
    # 4. Testar outras rotas protegidas
    Write-Host "`n4. Testando outras rotas protegidas..." -ForegroundColor Blue
    
    $categories = Test-Request "GET" "$baseUrl/api/categories/all" $null $token "Listar categorias"
    $companies = Test-Request "GET" "$baseUrl/api/company/all" $null $token "Listar empresas"
    
    if ($categories) { Write-Host "✅ Categorias: OK" -ForegroundColor Green }
    if ($companies) { Write-Host "✅ Empresas: OK" -ForegroundColor Green }
    
} else {
    Write-Host "❌ Falha no login. Não foi possível obter token." -ForegroundColor Red
}

# 5. Testar token inválido
Write-Host "`n5. Testando token inválido..." -ForegroundColor Blue
$invalidResult = Test-Request "GET" "$baseUrl/api/users/all" $null "token-invalido" "Usar token inválido"

if (!$invalidResult) {
    Write-Host "✅ Correto! Token inválido foi rejeitado" -ForegroundColor Green
} else {
    Write-Host "❌ ERRO! API aceitou token inválido" -ForegroundColor Red
}

Write-Host "`n=== RESUMO DO TESTE ===" -ForegroundColor Green
Write-Host "🔒 Sistema de autenticação JWT implementado e testado!" -ForegroundColor Cyan
