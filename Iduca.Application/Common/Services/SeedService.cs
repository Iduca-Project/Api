using Iduca.Application.Repository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Domain.Models;
using BC = BCrypt.Net.BCrypt;

namespace Iduca.Application.Common.Services;

public class SeedService : ISeedService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SeedService(
        ICompanyRepository companyRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task EnsureDefaultDataAsync(CancellationToken cancellationToken = default)
    {
        // Verificar se já existe empresa padrão
        var defaultCompany = await _companyRepository.GetCompanyByEqualName("Iduca Admin", cancellationToken);
        
        if (defaultCompany == null)
        {
            // Criar empresa padrão
            defaultCompany = new Company
            {
                Name = "Iduca Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _companyRepository.Create(defaultCompany);
            await _unitOfWork.Save(cancellationToken);
        }

        // Verificar se já existe usuário administrador
        var adminUser = await _userRepository.GetUserByEmail("admin@iduca.com", cancellationToken);
        
        if (adminUser == null)
        {
            // Criar usuário administrador padrão
            adminUser = new User
            {
                Name = "Administrador",
                Identity = "00000000000", // CPF fictício
                Email = "admin@iduca.com",
                Password = BC.HashPassword("admin123"), // Senha padrão
                IsAdmin = true,
                Company = defaultCompany,
                CompanyId = defaultCompany.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _userRepository.Create(adminUser);
            await _unitOfWork.Save(cancellationToken);

            Console.WriteLine("==============================================");
            Console.WriteLine("USUÁRIO ADMINISTRADOR CRIADO COM SUCESSO!");
            Console.WriteLine("==============================================");
            Console.WriteLine($"Email: {adminUser.Email}");
            Console.WriteLine("Senha: admin123");
            Console.WriteLine("IMPORTANTE: Altere a senha após o primeiro login!");
            Console.WriteLine("==============================================");
        }
    }
}
