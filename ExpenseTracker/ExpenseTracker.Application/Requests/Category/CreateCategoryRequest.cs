﻿using ExpenseTracker.Application.Requests.Common;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Requests.Category;

public record CreateCategoryRequest(
    Guid UserId,
    string Name,
    string? Description,
    CategoryType Type)
    : UserRequestId(UserId: UserId);
