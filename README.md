### [ Modelagem ](https://drive.google.com/file/d/1eU9bKm7yb8rmziR_P50M0-yaoEkZrAoN/view?usp=drive_link)
### [ Caso de uso ](https://excalidraw.com/#room=e304a1b783b96bd97dea,zUE57N0YvdqGjWg7izMN3w)

--

# Endpoints:

#🔐 Autenticação (Login e Recuperação de Senha)
📌 POST /auth/login
Realiza o login do usuário com e-mail corporativo e senha.

**Exemplo**
📥 Request Body

```
{
  "email": "usuario@empresa.com",
  "password": "senha123"
}
```

📤 Response - Login comum
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "João da Silva",
    "email": "usuario@empresa.com",
    "admin": false,
    "image": "caminho/da/imagem",
  },
  "firstAccess": false
}
```

📤 Response - Primeiro acesso
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "João da Silva",
    "email": "usuario@empresa.com",
    "admin": "false"
  },
  "firstAccess": true
}
```

#📌 POST /auth/forgotPass
Envia um código de 5 dígitos para o e-mail corporativo do usuário para recuperação de senha.

📥 Request Body
```
{
  "email": "usuario@empresa.com"
}
```

📤 Response
```
{
  "response": true
}
```

#📌 POST /auth/checkCode
Verifica se o código enviado ao e-mail está correto.

📥 Request Body
```
{
  "email": "usuario@empresa.com",
  "code": "12345"
}
```

📤 Response - Código válido
```
{
  "response": true
}
```

📤 Response - Código inválido
```
{
  "response": false
}
```

#📌 POST /auth/reenviar-codigo
Reenvia o código de 5 dígitos para o e-mail corporativo do usuário.

📥 Request Body
```
{
  "email": "usuario@empresa.com"
}
```
📤 Response
```
{
  "response": true
}
```

#📌 POST /auth/redefinir-senha
Redefine a senha do usuário após a verificação do código.

📥Request Body
```
{
  "email": "usuario@empresa.com",
  "newPass": "senhaNova123"
}
```

📥Response
```
{
  "response": true
}
```

