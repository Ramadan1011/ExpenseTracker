using ExpenseTracker.Application.Requests.User;
using ExpenseTracker.Application.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Application.Mappings;

public static class UserMappings
{
    public static UserViewModel ToViewModel(this IdentityUser<Guid> user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber
        };
    }

    public static IdentityUser<Guid> ToEntity(this CreateUserRequest request)
    {
        return new IdentityUser<Guid>
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
    }

    public static IdentityUser<Guid> ToEntity(this UpdateUserRequest request)
    {
        return new IdentityUser<Guid>
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
    }
}