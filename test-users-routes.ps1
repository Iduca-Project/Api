# Teste das Rotas de Usuários
$baseUrl = "http://localhost:5284"

Write-Host "=== TESTE DAS ROTAS DE USUÁRIOS ===" -ForegroundColor Green

# Primeiro fazer login como admin para obter token
Write-Host "`n1. Fazendo login como admin..." -ForegroundColor Blue
try {
    $loginBody = @{
        email = "admin@iduca.com"
        password = "admin123"
    } | ConvertTo-Json
    
    $headers = @{ "Content-Type" = "application/json" }
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginBody -Headers $headers
    $token = $loginResponse.token
    Write-Host "✅ Login realizado com sucesso!" -ForegroundColor Green
    Write-Host "   Token: $($token.Substring(0,30))..." -ForegroundColor Gray
}
catch {
    Write-Host "❌ Erro no login: $($_.Exception.Message)" -ForegroundColor Red
    exit
}

# Headers com autenticação
$authHeaders = @{ 
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

Write-Host "`n2. Testando rotas de usuários..." -ForegroundColor Blue

# Testar rota que não existe (GET /api/users)
Write-Host "`n🔄 Testando GET /api/users (rota que NÃO existe)" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/users" -Method GET -Headers $authHeaders
    Write-Host "✅ Resposta recebida: $response" -ForegroundColor Green
}
catch {
    Write-Host "❌ Erro (esperado): $($_.Exception.Response.StatusCode) - $($_.Exception.Message)" -ForegroundColor Red
}

# Testar rota que existe (GET /api/users/all)
Write-Host "`n🔄 Testando GET /api/users/all (rota que EXISTE)" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/users/all?Page=1&MaxItems=5" -Method GET -Headers $authHeaders
    Write-Host "✅ Sucesso! Usuários encontrados: $($response.Count)" -ForegroundColor Green
    if ($response.Count -gt 0) {
        Write-Host "   Primeiro usuário: $($response[0].name) - $($response[0].email)" -ForegroundColor Gray
    }
}
catch {
    Write-Host "❌ Erro: $($_.Exception.Response.StatusCode) - $($_.Exception.Message)" -ForegroundColor Red
}

# Listar todas as rotas disponíveis
Write-Host "`n3. Rotas disponíveis para usuários:" -ForegroundColor Blue
Write-Host "   POST   /api/users              - Criar usuário (admin)" -ForegroundColor Cyan
Write-Host "   GET    /api/users/{id}         - Buscar usuário por ID" -ForegroundColor Cyan
Write-Host "   GET    /api/users/all          - Listar todos os usuários (admin)" -ForegroundColor Cyan
Write-Host "   PUT    /api/users              - Atualizar usuário (admin)" -ForegroundColor Cyan
Write-Host "   DELETE /api/users/{id}         - Deletar usuário (admin)" -ForegroundColor Cyan

Write-Host "`n4. Exemplo de uso correto no Postman:" -ForegroundColor Blue
Write-Host "   1. Primeiro faça login: POST {{base_url}}/api/auth/login" -ForegroundColor White
Write-Host "   2. Use o token no header: Authorization: Bearer {token}" -ForegroundColor White
Write-Host "   3. Acesse: GET {{base_url}}/api/users/all?Page=1&MaxItems=10" -ForegroundColor White

Write-Host "`n=== FIM DO TESTE ===" -ForegroundColor Green
