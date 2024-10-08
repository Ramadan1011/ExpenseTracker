﻿using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal sealed class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
{
    public WalletRepository(ExpenseTrackerDbContext context)
        : base(context)
    {
    }

    public override List<Wallet> GetAll(Guid userId)
    {
        var wallets = _context.Wallets
            .AsNoTracking()
            .Include(x => x.Owner)
            .Where(x => x.OwnerId == userId || x.Shares.Any(s => s.UserId == userId && s.IsAccepted))
            .ToList();

        return wallets;
    }

    public override Wallet GetById(int id)
    {
        var wallet = _context.Wallets
            .AsNoTracking()
            .Include(x => x.Owner)
            .FirstOrDefault(x => x.Id == id);

        if (wallet is null)
        {
            throw new EntityNotFoundException($"Wallet with id: {id} is not found.");
        }

        return wallet;
    }
}
