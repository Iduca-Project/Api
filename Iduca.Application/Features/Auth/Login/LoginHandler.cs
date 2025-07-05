using MediatR;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Contracts;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Common.Services;

namespace Iduca.Application.Features.Auth.Login;

public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticator _authenticator;
    private readonly ILogService _logService;

    public LoginHandler(IUserRepository userRepository, IAuthenticator authenticator, ILogService logService)
    {
        _userRepository = userRepository;
        _authenticator = authenticator;
        _logService = logService;
    }

    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        // Buscar usuário por email
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user == null)
        {
            // Não fazer log quando usuário não existe para evitar problemas de referência
            throw new NotFoundException("Usuário não encontrado ou credenciais inválidas");
        }

        // Verificar senha (assumindo que a senha está em hash)
        if (!VerifyPassword(request.Password, user.Password))
        {
            // Log de tentativa de login falhada (senha incorreta)
            await _logService.LogLoginAsync(user.Id, false, cancellationToken);
            throw new UnauthorizedException("Usuário não encontrado ou credenciais inválidas");
        }

        // Gerar token JWT
        var token = _authenticator.GenerateUserToken(user);

        // Determinar se é primeiro acesso (antes de fazer log de login)
        var firstAccess = await _userRepository.IsFirstAccessAsync(user.Id, cancellationToken);

        var admin = user.IsAdmin;

        var manager = await _userRepository.IsManager(user.Id, cancellationToken);

        // Log de login bem-sucedido
        await _logService.LogLoginAsync(user.Id, true, cancellationToken);

        // Se é o primeiro acesso, marcar como completo após o login bem-sucedido
        if (firstAccess)
        {
            await _userRepository.MarkFirstAccessCompleteAsync(user.Id, cancellationToken);
        }

        return new LoginResponse(token, admin, manager, firstAccess);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        // Usar BCrypt para verificar a senha
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
