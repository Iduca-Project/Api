# Script para testar as novas funcionalidades de matrícula e progresso
# PowerShell script para API Iduca

$baseUrl = "http://localhost:5284"
$adminCredentials = @{
    "identity" = "admin"
    "password" = "admin123"
}

Write-Host "=== TESTE DAS NOVAS FUNCIONALIDADES DE CURSO ===" -ForegroundColor Cyan

# 1. Login do Admin
Write-Host "`n1. Fazendo login como admin..." -ForegroundColor Yellow
try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -ContentType "application/json" -Body ($adminCredentials | ConvertTo-Json)
    $adminToken = $loginResponse.token
    Write-Host "✅ Login admin realizado com sucesso" -ForegroundColor Green
    Write-Host "Token: $($adminToken.Substring(0, 20))..." -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro no login admin: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

$adminHeaders = @{
    "Authorization" = "Bearer $adminToken"
    "Content-Type" = "application/json"
}

# 2. Listar cursos disponíveis
Write-Host "`n2. Listando cursos disponíveis..." -ForegroundColor Yellow
try {
    $coursesResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/all?Page=1&MaxItems=10" -Method GET -Headers $adminHeaders
    $courses = $coursesResponse.courses
    Write-Host "✅ Encontrados $($courses.Count) cursos" -ForegroundColor Green
    
    if ($courses.Count -gt 0) {
        $courseId = $courses[0].id
        Write-Host "Curso selecionado: $($courses[0].name) (ID: $courseId)" -ForegroundColor Gray
    } else {
        Write-Host "❌ Nenhum curso encontrado. Criando um curso de teste..." -ForegroundColor Yellow
        
        # Criar categoria primeiro (se necessário)
        $categoryRequest = @{
            "name" = "Tecnologia"
            "description" = "Categoria de cursos de tecnologia"
        }
        
        try {
            $categoryResponse = Invoke-RestMethod -Uri "$baseUrl/api/categories" -Method POST -Headers $adminHeaders -Body ($categoryRequest | ConvertTo-Json)
            $categoryId = $categoryResponse.id
            Write-Host "Categoria criada: $categoryId" -ForegroundColor Gray
        } catch {
            Write-Host "Erro ao criar categoria (pode já existir): $($_.Exception.Message)" -ForegroundColor Yellow
            # Assumir uma categoria padrão
            $categoryId = "11111111-1111-1111-1111-111111111111"
        }
        
        # Criar curso de teste
        $courseRequest = @{
            "name" = "Curso de Teste - PowerShell"
            "description" = "Curso criado via script PowerShell para testes"
            "difficulty" = 1
            "image" = "https://example.com/test-course.jpg"
            "totalHours" = 20
            "categories" = @($categoryId)
        }
        
        $courseResponse = Invoke-RestMethod -Uri "$baseUrl/api/course" -Method POST -Headers $adminHeaders -Body ($courseRequest | ConvertTo-Json)
        $courseId = $courseResponse.id
        Write-Host "✅ Curso de teste criado: $courseId" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ Erro ao listar cursos: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 3. Tentar matricular o admin no curso
Write-Host "`n3. Testando matrícula no curso..." -ForegroundColor Yellow
try {
    $enrollRequest = @{
        "courseId" = $courseId
        "userId" = "5374148b-5061-11f0-b52d-0a002700000b"  # ID do admin
    }
    
    $enrollResponse = Invoke-RestMethod -Uri "$baseUrl/api/course/$courseId/enroll" -Method POST -Headers $adminHeaders -Body ($enrollRequest | ConvertTo-Json)
    Write-Host "✅ Matrícula realizada com sucesso!" -ForegroundColor Green
    Write-Host "Mensagem: $($enrollResponse.message)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro na matrícula (pode já estar matriculado): $($_.Exception.Message)" -ForegroundColor Yellow
}

# 4. Verificar cursos do usuário
Write-Host "`n4. Verificando cursos do usuário..." -ForegroundColor Yellow
try {
    $myCoursesResponse = Invoke-RestMethod -Uri "$baseUrl/api/users/my-courses" -Method GET -Headers $adminHeaders
    $myCourses = $myCoursesResponse.courses
    Write-Host "✅ Usuário possui $($myCourses.Count) curso(s) matriculado(s)" -ForegroundColor Green
    
    foreach ($course in $myCourses) {
        Write-Host "- $($course.courseName): $($course.progress.percentageComplete)% completo" -ForegroundColor Gray
        Write-Host "  Módulos: $($course.progress.completedModules)/$($course.progress.totalModules)" -ForegroundColor Gray
        Write-Host "  Lições: $($course.progress.completedLessons)/$($course.progress.totalLessons)" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Erro ao buscar cursos do usuário: $($_.Exception.Message)" -ForegroundColor Red
}

# 5. Buscar módulos do curso para testar lições
Write-Host "`n5. Buscando módulos do curso..." -ForegroundColor Yellow
try {
    $modulesResponse = Invoke-RestMethod -Uri "$baseUrl/api/modules/course/$courseId" -Method GET -Headers $adminHeaders
    $modules = $modulesResponse.modules
    
    if ($modules.Count -gt 0) {
        $moduleId = $modules[0].id
        Write-Host "✅ Encontrados $($modules.Count) módulo(s). Primeiro módulo: $moduleId" -ForegroundColor Green
        
        # Buscar lições do módulo
        try {
            $lessonsResponse = Invoke-RestMethod -Uri "$baseUrl/api/lessons/module/$moduleId" -Method GET -Headers $adminHeaders
            $lessons = $lessonsResponse.lessons
            
            if ($lessons.Count -gt 0) {
                $lessonId = $lessons[0].id
                Write-Host "✅ Encontradas $($lessons.Count) lição(ões). Primeira lição: $lessonId" -ForegroundColor Green
                
                # 6. Testar completar lição
                Write-Host "`n6. Testando completar lição..." -ForegroundColor Yellow
                $completeLessonRequest = @{
                    "lessonId" = $lessonId
                    "userId" = "5374148b-5061-11f0-b52d-0a002700000b"  # ID do admin
                }
                
                try {
                    $completeResponse = Invoke-RestMethod -Uri "$baseUrl/api/lessons/$lessonId/complete" -Method POST -Headers $adminHeaders -Body ($completeLessonRequest | ConvertTo-Json)
                    Write-Host "✅ Lição completada com sucesso!" -ForegroundColor Green
                    Write-Host "Mensagem: $($completeResponse.message)" -ForegroundColor Gray
                } catch {
                    Write-Host "❌ Erro ao completar lição: $($_.Exception.Message)" -ForegroundColor Red
                }
            } else {
                Write-Host "⚠️ Nenhuma lição encontrada no módulo" -ForegroundColor Yellow
            }
        } catch {
            Write-Host "❌ Erro ao buscar lições: $($_.Exception.Message)" -ForegroundColor Red
        }
    } else {
        Write-Host "⚠️ Nenhum módulo encontrado no curso" -ForegroundColor Yellow
    }
} catch {
    Write-Host "❌ Erro ao buscar módulos: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== TESTE CONCLUÍDO ===" -ForegroundColor Cyan
Write-Host "Resumo das funcionalidades testadas:" -ForegroundColor White
Write-Host "- ✅ Login de admin" -ForegroundColor Green
Write-Host "- ✅ Listagem de cursos" -ForegroundColor Green
Write-Host "- ⚠️ Matrícula em curso (implementado, mas pode ter bugs)" -ForegroundColor Yellow
Write-Host "- ⚠️ Listagem de cursos do usuário (implementado, mas progresso não calculado)" -ForegroundColor Yellow
Write-Host "- ⚠️ Completar lição (implementado, mas pode ter problemas de relacionamento)" -ForegroundColor Yellow
