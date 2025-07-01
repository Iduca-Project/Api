# Teste básico da API

# Aguardar a API inicializar
Start-Sleep -Seconds 5

# Teste básico de conectividade
try {
    Write-Host "Testando conectividade com a API..." -ForegroundColor Yellow
    
    # Teste simples para verificar se a API está rodando
    $response = Invoke-WebRequest -Uri "http://localhost:5284/api/categories/all" -Method GET -TimeoutSec 10
    
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ API está funcionando! Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host "Resposta: $($response.Content)" -ForegroundColor White
    }
}
catch {
    Write-Host "❌ Erro ao conectar com a API: $($_.Exception.Message)" -ForegroundColor Red
    
    # Verificar se a porta está em uso
    try {
        $connections = Get-NetTCPConnection -LocalPort 5284 -ErrorAction SilentlyContinue
        if ($connections) {
            Write-Host "✅ Porta 5284 está em uso - API provavelmente está rodando" -ForegroundColor Yellow
        } else {
            Write-Host "❌ Porta 5284 não está em uso - API não está rodando" -ForegroundColor Red
        }
    }
    catch {
        Write-Host "Não foi possível verificar a porta" -ForegroundColor Gray
    }
}

Write-Host "`nPara testar as rotas manualmente, use:" -ForegroundColor Cyan
Write-Host "- Arquivo test-routes.http com REST Client extension" -ForegroundColor White
Write-Host "- Ou execute: .\test-api.ps1" -ForegroundColor White
