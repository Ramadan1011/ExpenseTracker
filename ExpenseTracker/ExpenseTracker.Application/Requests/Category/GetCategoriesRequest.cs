using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Category;

public sealed record GetCategoriesRequest(Guid UserId,string? Search)
    : UserRequestId(UserId: UserId);
