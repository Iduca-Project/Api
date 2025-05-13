param(
    [string]$EntityName
)

$EntityNameLower = $EntityName.toLower()

New-Item -Path ".\Domain\Entities\$($EntityName)\Models\$($EntityName).cs" -ItemType File -Force
New-Item -Path ".\Domain\Entities\$($EntityName)\Repositories\I$($EntityName)Repository.cs" -ItemType File -Force
New-Item -Path ".\Domain\Entities\$($EntityName)\Services\I$($EntityName)Service.cs" -ItemType File -Force
New-Item -Path ".\Core\Entities\$($EntityName)\Mapping\$($EntityName)ClassMap.cs" -ItemType File -Force
New-Item -Path ".\Core\Entities\$($EntityName)\Repositories\$($EntityName)Repository.cs" -ItemType File -Force
New-Item -Path ".\Core\Entities\$($EntityName)\Services\$($EntityName)Service.cs" -ItemType File -Force

Write-Output @"
namespace Api.Domain.Models;

public class $($EntityName) : IEntity
{

}
"@ | Out-File -FilePath ".\Domain\Entities\$($EntityName)\Models\$($EntityName).cs"

Write-Output @"
using Api.Domain.Models;

namespace Api.Domain.Repositories;

public interface I$($EntityName)Repository : IRepository<$($EntityName)>
{

}
"@ | Out-File -FilePath ".\Domain\Entities\$($EntityName)\Repositories\I$($EntityName)Repository.cs"

Write-Output @"
using Api.Domain.Models;

namespace Api.Domain.Services;

public interface I$($EntityName)Service : IService<$($EntityName)>
{
    
}
"@ | Out-File -FilePath ".\Domain\Entities\$($EntityName)\Services\I$($EntityName)Service.cs"

Write-Output @"
using Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Core.Mapping;

public class $($EntityName)ClassMap : IEntityTypeConfiguration<$($EntityName)>
{
    public void Configure(EntityTypeBuilder<$($EntityName)> builder)
    {
        builder.HasKey($($EntityNameLower) => $($EntityNameLower).Id)
            .HasName("PK_____$($EntityName)");

        builder.ToTable("tb_$($EntityNameLower)");
    }
}
"@ | Out-File -FilePath ".\Core\Entities\$($EntityName)\Mapping\$($EntityName)ClassMap.cs"

Write-Output @"
using Api.Domain.Models;
using Api.Domain.Repositories;

namespace Api.Core.Repositories;

public class $($EntityName)Repository(Context context)
    : BaseRepository<$($EntityName)>(context), I$($EntityName)Repository
{
    
}
"@ | Out-File -FilePath ".\Core\Entities\$($EntityName)\Repositories\$($EntityName)Repository.cs"

Write-Output @"
using Api.Core.Repositories;
using Api.Domain.Models;
using Api.Domain.Services;

namespace Api.Core.Services;

public class $($EntityName)Service($($EntityName)Repository repository)
    : BaseService<$($EntityName)>(repository), I$($EntityName)Service
{

}
"@ | Out-File -FilePath ".\Core\Entities\$($EntityName)\Services\$($EntityName)Service.cs"