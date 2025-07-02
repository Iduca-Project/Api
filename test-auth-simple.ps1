# Teste simples de autenticação
$baseUrl = "http://localhost:5284"

Write-Host "=== TESTE SIMPLES DE AUTENTICAÇÃO ===" -ForegroundColor Green

# Testar sem token
Write-Host "`n1. Testando SEM token..." -ForegroundColor Blue
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/users/all" -Method GET
    Write-Host "❌ ERRO! API permitiu acesso sem token" -ForegroundColor Red
    $response | ConvertTo-Json | Write-Host -ForegroundColor Red
} catch {
    if ($_.Exception.Response.StatusCode -eq 401) {
        Write-Host "✅ Correto! Token requerido (401 Unauthorized)" -ForegroundColor Green
    } else {
        Write-Host "❌ Erro inesperado: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Fazer login
Write-Host "`n2. Fazendo login..." -ForegroundColor Blue
try {
    $loginBody = @{
        email = "joao.santos.atualizado@teste.com"
        password = "123456"
    } | ConvertTo-Json

    $headers = @{ "Content-Type" = "application/json" }
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginBody -Headers $headers
    
    $token = $loginResponse.token
    Write-Host "✅ Login OK! Token: $($token.Substring(0,20))..." -ForegroundColor Green
    
    # Testar com token
    Write-Host "`n3. Testando COM token..." -ForegroundColor Blue
    $authHeaders = @{
        "Authorization" = "Bearer $token"
        "Content-Type" = "application/json"
    }
    
    try {
        $usersResponse = Invoke-RestMethod -Uri "$baseUrl/api/users/all" -Method GET -Headers $authHeaders
        Write-Host "✅ Sucesso! Autenticação funcionando!" -ForegroundColor Green
        Write-Host "   Usuários encontrados: $($usersResponse.Count)" -ForegroundColor Cyan
    } catch {
        Write-Host "❌ Falha ao acessar com token válido: $($_.Exception.Message)" -ForegroundColor Red
    }
    
} catch {
    Write-Host "❌ Falha no login: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== FIM DO TESTE ===" -ForegroundColor Green
