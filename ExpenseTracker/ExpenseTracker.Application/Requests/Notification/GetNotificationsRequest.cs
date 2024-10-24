﻿using ExpenseTracker.Application.Requests.Common;

namespace ExpenseTracker.Application.Requests.Notification;

public sealed record GetNotificationsRequest(Guid UserId)
    : UserRequest(UserId: UserId);
