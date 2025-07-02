# Teste específico para login do admin
$baseUrl = "http://localhost:5284"

Write-Host "=== TESTE DE LOGIN DO ADMIN ===" -ForegroundColor Green

function Test-AdminLogin {
    param($Email, $Password, $Description)
    
    Write-Host "`n🔄 $Description" -ForegroundColor Yellow
    Write-Host "   Email: $Email" -ForegroundColor Gray
    
    try {
        $body = @{
            email = $Email
            password = $Password
        } | ConvertTo-Json
        
        Write-Host "   JSON enviado: $body" -ForegroundColor Gray
        
        $headers = @{ "Content-Type" = "application/json" }
        
        $response = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $body -Headers $headers
        
        Write-Host "✅ LOGIN SUCESSO!" -ForegroundColor Green
        Write-Host "   Token: $($response.token.Substring(0,20))..." -ForegroundColor Green
        Write-Host "   Primeiro Acesso: $($response.firstAccess)" -ForegroundColor Green
        return $response
    }
    catch {
        Write-Host "❌ ERRO DE LOGIN!" -ForegroundColor Red
        Write-Host "   Código: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
        Write-Host "   Mensagem: $($_.Exception.Message)" -ForegroundColor Red
        
        # Tentar capturar o corpo da resposta de erro
        try {
            $errorStream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($errorStream)
            $errorBody = $reader.ReadToEnd()
            Write-Host "   Corpo do erro: $errorBody" -ForegroundColor Red
        }
        catch {
            Write-Host "   Não foi possível ler o corpo do erro" -ForegroundColor Red
        }
        return $null
    }
}

# Testar primeiro com API em funcionamento
Write-Host "`n1. Verificando se a API está funcionando..." -ForegroundColor Blue
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/company/all" -Method GET
    Write-Host "✅ API está respondendo" -ForegroundColor Green
}
catch {
    Write-Host "❌ API não está respondendo ou endpoint não existe" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
}

# Testar login do admin
Write-Host "`n2. Testando login do admin..." -ForegroundColor Blue
$adminResult = Test-AdminLogin "admin@iduca.com" "123456" "Login do Admin (admin@iduca.com)"

# Testar com outras variações possíveis
if (!$adminResult) {
    Write-Host "`n3. Testando variações do admin..." -ForegroundColor Blue
    Test-AdminLogin "admin@admin.com" "123456" "Teste com admin@admin.com"
    Test-AdminLogin "admin@iduca.com" "admin" "Teste com senha 'admin'"
    Test-AdminLogin "admin@iduca.com" "password" "Teste com senha 'password'"
    Test-AdminLogin "admin@iduca.com" "12345678" "Teste com senha '12345678'"
}

Write-Host "`n=== FIM DO TESTE ===" -ForegroundColor Green
