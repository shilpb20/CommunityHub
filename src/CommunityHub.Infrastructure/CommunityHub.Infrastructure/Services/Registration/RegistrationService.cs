using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

public class RegistrationService : IRegistrationService
{
    private readonly ILogger<IRegistrationService> _logger;
    private readonly IRepository<RegistrationRequest, ApplicationDbContext> _repository;

    public RegistrationService(
        ILogger<IRegistrationService> logger,
        IRepository<RegistrationRequest, ApplicationDbContext> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<RegistrationRequest> CreateRequestAsync(RegistrationInfo registrationData)
    {
        try
        {
            if (registrationData == null)
            {
                _logger.LogDebug("Registration data null. Skipping request creation");
                return null;
            }

            _logger.LogDebug($"Creating registration request for user - {registrationData?.UserInfo.FullName}.");

            var registrationInfo = JsonConvert.SerializeObject(registrationData);
            var registrationRequest = new RegistrationRequest(registrationInfo);

            return await _repository.AddAsync(registrationRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while creating registration request: {ex.Message}", ex);
            throw;
        }
    }

    public async Task<RegistrationRequest> GetRequestByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning($"Invalid ID: {id}. Cannot retrieve registration request.");
            return null;
        }

        return await _repository.GetAsync(x => x.Id == id);
    }

    public async Task<List<RegistrationRequest>> GetRequestsAsync(eRegistrationStatus status)
    {
        if (status == eRegistrationStatus.All)
        {
            return await _repository.GetAllAsync();
        }

        return await _repository.GetAllAsync(x => x.RegistrationStatus.ToString() == status.GetEnumMemberValue());
    }

    public async Task<RegistrationRequest> ApproveRequestAsync(int id)
    {
        var request = await GetRequestByIdAsync(id);
        if (request == null) return null;

        request.Approve();
        return await _repository.UpdateAsync(request);
    }

    public async Task<RegistrationRequest> RejectRequestAsync(int id, string comment)
    {
        var request = await GetRequestByIdAsync(id);
        if (request == null) return null;

        request.Reject(comment);
        return await _repository.UpdateAsync(request);
    }

}
