﻿using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.WalletShare;

public sealed record GetWalletSharesRequest(Guid UserId, string? Search)
    : UserRequestId(UserId: UserId);
