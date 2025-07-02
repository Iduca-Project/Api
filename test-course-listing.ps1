# Test script para validar listagem de cursos
$baseUrl = "http://localhost:5001"

# Primeiro, fazer login como admin
$loginData = @{
    email = "admin@iduca.com"
    password = "admin123"
}

$loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method Post -Body ($loginData | ConvertTo-Json) -ContentType "application/json"

if ($loginResponse.success -eq $true) {
    Write-Host "Login realizado com sucesso!" -ForegroundColor Green
    $token = $loginResponse.data.token
    
    # Testar listagem de cursos sem filtros
    Write-Host "`n=== Testando listagem de cursos ===" -ForegroundColor Yellow
    
    $headers = @{
        "Authorization" = "Bearer $token"
    }
    
    try {
        $coursesResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/all?Page=1&MaxItems=10" -Method Get -Headers $headers
        
        if ($coursesResponse.success -eq $true) {
            Write-Host "Sucesso na listagem de cursos!" -ForegroundColor Green
            Write-Host "Dados retornados:" -ForegroundColor Cyan
            $coursesResponse.data | ConvertTo-Json -Depth 3
        } else {
            Write-Host "Erro na listagem de cursos:" -ForegroundColor Red
            $coursesResponse | ConvertTo-Json -Depth 3
        }
    } catch {
        Write-Host "Erro na requisição de listagem de cursos:" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
    }
    
    # Testar listagem de cursos com filtro por nome
    Write-Host "`n=== Testando listagem com filtro por nome ===" -ForegroundColor Yellow
    
    try {
        $coursesWithFilterResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/all?Name=Curso&Page=1&MaxItems=10" -Method Get -Headers $headers
        
        if ($coursesWithFilterResponse.success -eq $true) {
            Write-Host "Sucesso na listagem com filtro!" -ForegroundColor Green
            Write-Host "Dados retornados:" -ForegroundColor Cyan
            $coursesWithFilterResponse.data | ConvertTo-Json -Depth 3
        } else {
            Write-Host "Erro na listagem com filtro:" -ForegroundColor Red
            $coursesWithFilterResponse | ConvertTo-Json -Depth 3
        }
    } catch {
        Write-Host "Erro na requisição de listagem com filtro:" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
    }
    
    # Criar um curso básico para testar
    Write-Host "`n=== Criando um curso para testar ===" -ForegroundColor Yellow
    
    # Primeiro criar uma categoria
    $categoryData = @{
        name = "Categoria Teste"
        description = "Uma categoria para teste"
    }
    
    try {
        $categoryResponse = Invoke-RestMethod -Uri "$baseUrl/api/category" -Method Post -Body ($categoryData | ConvertTo-Json) -ContentType "application/json" -Headers $headers
        
        if ($categoryResponse.success -eq $true) {
            Write-Host "Categoria criada com sucesso!" -ForegroundColor Green
            $categoryId = $categoryResponse.data.id
            
            # Agora criar um curso
            $courseData = @{
                name = "Curso de Teste"
                description = "Um curso para testar a listagem"
                difficulty = 1
                image = "https://example.com/image.jpg"
                totalHours = 40
                categories = @($categoryId)
            }
            
            $courseResponse = Invoke-RestMethod -Uri "$baseUrl/api/course" -Method Post -Body ($courseData | ConvertTo-Json) -ContentType "application/json" -Headers $headers
            
            if ($courseResponse.success -eq $true) {
                Write-Host "Curso criado com sucesso!" -ForegroundColor Green
                
                # Testar listagem novamente
                Write-Host "`n=== Testando listagem após criação ===" -ForegroundColor Yellow
                
                $newCoursesResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/all?Page=1&MaxItems=10" -Method Get -Headers $headers
                
                if ($newCoursesResponse.success -eq $true) {
                    Write-Host "Sucesso na nova listagem!" -ForegroundColor Green
                    Write-Host "Dados retornados:" -ForegroundColor Cyan
                    $newCoursesResponse.data | ConvertTo-Json -Depth 3
                } else {
                    Write-Host "Erro na nova listagem:" -ForegroundColor Red
                    $newCoursesResponse | ConvertTo-Json -Depth 3
                }
            } else {
                Write-Host "Erro ao criar curso:" -ForegroundColor Red
                $courseResponse | ConvertTo-Json -Depth 3
            }
        } else {
            Write-Host "Erro ao criar categoria:" -ForegroundColor Red
            $categoryResponse | ConvertTo-Json -Depth 3
        }
    } catch {
        Write-Host "Erro na criação de categoria/curso:" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
    }
    
} else {
    Write-Host "Erro no login:" -ForegroundColor Red
    $loginResponse | ConvertTo-Json -Depth 3
}
