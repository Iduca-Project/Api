#!/bin/bash

# Script para testar as rotas da API
BASE_URL="http://localhost:5284"

echo "=== TESTANDO ROTAS DA API ==="

# Função para fazer requisições
test_route() {
    echo ""
    echo "--- $1 ---"
    echo "Fazendo requisição: $2 $3"
    if [ -n "$4" ]; then
        echo "Body: $4"
        curl -X "$2" "$BASE_URL$3" \
             -H "Content-Type: application/json" \
             -H "Accept: application/json" \
             -d "$4" \
             -w "\nStatus: %{http_code}\n" \
             -s
    else
        curl -X "$2" "$BASE_URL$3" \
             -H "Accept: application/json" \
             -w "\nStatus: %{http_code}\n" \
             -s
    fi
    echo ""
}

echo "Aguardando API iniciar..."
sleep 3

# Teste das rotas de Categories
test_route "Criar Categoria 1" "POST" "/api/categories" '{"name":"Tecnologia","description":"Categoria sobre tecnologia"}'

test_route "Criar Categoria 2" "POST" "/api/categories" '{"name":"Programação","description":"Categoria sobre programação"}'

test_route "Listar Todas as Categorias" "GET" "/api/categories/all"

test_route "Buscar Categoria por Nome" "GET" "/api/categories/search?Name=Tecnologia"

# Teste das rotas de Company
test_route "Criar Empresa" "POST" "/api/company" '{"name":"Empresa Teste","cnpj":"12345678000199","email":"contato@empresateste.com","phone":"(11) 99999-9999"}'

# Teste básico de Users (precisaria dos IDs reais para funcionar completamente)
test_route "Listar Usuários" "GET" "/api/users/all?Page=1&MaxItems=10"

test_route "Buscar Usuários por Nome" "GET" "/api/users/all?Name=João&Page=1&MaxItems=10"

test_route "Buscar Apenas Admins" "GET" "/api/users/all?IsAdmin=true&Page=1&MaxItems=10"

# Teste das rotas de Courses
test_route "Listar Cursos" "GET" "/api/course/all?Page=1&MaxItens=10"

echo "=== TESTES CONCLUÍDOS ==="
