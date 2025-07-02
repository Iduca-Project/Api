# Script para testar PATCH de company
$baseUrl = "http://localhost:5284"

# 1. Fazer login como admin
Write-Host "=== FAZENDO LOGIN COMO ADMIN ===" -ForegroundColor Yellow
$loginData = @{
    email = "admin@iduca.com"
    password = "admin123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginData -ContentType "application/json"
    $token = $loginResponse.token
    Write-Host "✅ Login realizado com sucesso" -ForegroundColor Green
    Write-Host "Token: $($token.Substring(0, 50))..." -ForegroundColor Cyan
} catch {
    Write-Host "❌ Erro no login: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 2. Listar companies para pegar um ID válido
Write-Host "`n=== LISTANDO COMPANIES ===" -ForegroundColor Yellow
$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

try {
    $companiesResponse = Invoke-RestMethod -Uri "$baseUrl/api/company/all" -Method GET -Headers $headers
    Write-Host "✅ Companies listadas com sucesso" -ForegroundColor Green
    
    if ($companiesResponse.Count -gt 0) {
        $companyId = $companiesResponse[0].id
        Write-Host "ID da primeira company: $companyId" -ForegroundColor Cyan
        
        # 3. Tentar fazer PATCH da company
        Write-Host "`n=== TESTANDO PATCH DE COMPANY ===" -ForegroundColor Yellow
        $patchData = @{
            name = "Company Atualizada - $(Get-Date -Format 'HH:mm:ss')"
            description = "Descrição atualizada via PATCH"
        } | ConvertTo-Json
        
        try {
            $patchResponse = Invoke-RestMethod -Uri "$baseUrl/api/company/$companyId" -Method PATCH -Body $patchData -Headers $headers
            Write-Host "✅ PATCH realizado com sucesso" -ForegroundColor Green
            Write-Host "Response: $($patchResponse | ConvertTo-Json)" -ForegroundColor Cyan
        } catch {
            Write-Host "❌ Erro no PATCH: $($_.Exception.Message)" -ForegroundColor Red
            Write-Host "Status Code: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
            Write-Host "Method: $($_.Exception.Response.Method)" -ForegroundColor Red
            
            # Tentar ler o conteúdo da resposta de erro
            try {
                $errorStream = $_.Exception.Response.GetResponseStream()
                $reader = New-Object System.IO.StreamReader($errorStream)
                $errorContent = $reader.ReadToEnd()
                Write-Host "Error Content: $errorContent" -ForegroundColor Red
            } catch {
                Write-Host "Não foi possível ler o conteúdo do erro" -ForegroundColor Red
            }
        }
    } else {
        Write-Host "❌ Nenhuma company encontrada" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ Erro ao listar companies: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== TESTE CONCLUÍDO ===" -ForegroundColor Yellow
