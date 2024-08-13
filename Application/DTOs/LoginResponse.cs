

using Domain.Enums;

namespace Application.DTOs
{
    public record LoginResponse(bool Flag, string Message = null!, string Token=null!, RoleEnum roleType= RoleEnum.Basic);

}

