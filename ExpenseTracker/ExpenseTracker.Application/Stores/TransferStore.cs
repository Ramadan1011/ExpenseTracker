﻿using ExpenseTracker.Application.Requests.Transfer;
using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.Transfer;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Stores;

public class TransferStore : ITransferStore
{
    private readonly ICommonRepository _repository;

    public TransferStore(ICommonRepository repository)
    {
        _repository = repository;
    }

    public List<TransferViewModel> GetAll(GetTransfersRequest request)
    {
        var transfers = _repository.Transfers.GetAll(request.CategoryId, request.Search, request.UserId);
        var viewModels = transfers
            .Select(x => x.ToViewModel()).ToList();

        return viewModels;
    }

    public TransferViewModel GetById(TransferRequest request)
    {
        var transfer = _repository.Transfers.GetById(request.Id);
        transfer.Images = _repository.ImageFiles.GetByTransferId(request.Id);
        var viewModel = transfer.ToViewModel();

        return viewModel;
    }

    public TransferViewModel Create(CreateTransferRequest request, IEnumerable<IFormFile> attachments)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = request.ToEntity();

        var createdTransfer = _repository.Transfers.Create(entity);

        foreach (var image in attachments)
        {
            using var stream = new MemoryStream();
            image.CopyTo(stream);

            var imageFile = new ImageFile
            {
                Name = image.FileName,
                Type = image.ContentType,
                Data = stream.ToArray(),
                Transfer = createdTransfer,
            };

            _repository.ImageFiles.Create(imageFile);
        }

        _repository.SaveChanges(); // ACID -> Atomicity

        createdTransfer.Category = _repository.Categories.GetById(request.CategoryId);
        var viewModel = createdTransfer.ToViewModel();

        return viewModel;
    }

    public void Update(UpdateTransferRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = request.ToEntity();

        try
        {
            _repository.Transfers.Update(entity);
            _repository.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_repository.Transfers.Exists(request.Id))
            {
                throw new EntityNotFoundException($"Transfer with id: {request.Id} is not found.");
            }
        }
    }

    public void Delete(TransferRequest request)
    {
        _repository.Transfers.Delete(request.Id);
        _repository.SaveChanges();
    }
}
