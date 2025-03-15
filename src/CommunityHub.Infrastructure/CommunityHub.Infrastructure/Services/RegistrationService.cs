using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using Azure.Core;
using CommunityHub.Core.Dtos;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CommunityHub.Infrastructure.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ILogger<IRegistrationService> _logger;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<RegistrationRequest, ApplicationDbContext> _requestRepository;
        private readonly IRepository<UserInfo, ApplicationDbContext> _userInfoRepository;

        public RegistrationService(
            ILogger<IRegistrationService> logger,
            ITransactionManager transactionManager,
            IRepository<RegistrationRequest, ApplicationDbContext> requestRepository,
            IRepository<UserInfo, ApplicationDbContext> userInfoRepository)
        {
            _logger = logger;
            _transactionManager = transactionManager;
            _requestRepository = requestRepository;
            _userInfoRepository = userInfoRepository;
        }

        public async Task<UserInfo> ApproveRequestAsync(int id)
        {
            using (_transactionManager.BeginTransactionAsync())
            {
                try
                {
                    var matchingRequest = await _requestRepository.GetAsync(x => x.Id == id);
                    if (matchingRequest == null) return null;

                    var registrationInfo = JsonConvert.DeserializeObject<RegistrationInfo>(matchingRequest.RegistrationInfo);
                    var newUser = await _userInfoRepository.AddAsync(registrationInfo.UserInfo);
                    if (newUser == null) return null;

                    matchingRequest.ReviewedAt = DateTime.UtcNow;
                    matchingRequest.RegistrationStatus = RegistrationStatus.Approved.GetEnumMemberValue();
                    await _requestRepository.UpdateAsync(matchingRequest);

                    await _transactionManager.CommitTransactionAsync();
                    return newUser;
                }
                catch (Exception ex)
                {
                    await _transactionManager.RollbackTransactionAsync();
                    _logger.LogError(ex, "Error approving request {Id}", id);
                    throw;
                }
            }
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

                string registrationInfo = JsonConvert.SerializeObject(registrationData);
                RegistrationRequest registrationRequest = new RegistrationRequest()
                {
                    RegistrationInfo = registrationInfo,
                    CreatedAt = DateTime.UtcNow,
                    RegistrationStatus = "pending",
                    Review = null,
                    ReviewedAt = null
                };

                return await _requestRepository.AddAsync(registrationRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating registration request: {ex.Message}", ex);
                throw;
            }        
        }

        public async Task<RegistrationRequest> GetRequestAsync(int id)
        {
            return await _requestRepository.GetAsync(x => x.Id == id);
        }

        public async Task<List<RegistrationRequest>> GetRequestsAsync(RegistrationStatus status = RegistrationStatus.Pending)
        {
            if(status == RegistrationStatus.All)
            {
                return await _requestRepository.GetAllAsync();
            }
               
            return await _requestRepository.GetAllAsync(x => x.RegistrationStatus.ToLower() == status.GetEnumMemberValue().ToLower());
        }

        public async Task<RegistrationRequest> RejectRequestAsync(int id, string reviewComment)
        {
            var matchingRequest = await _requestRepository.GetAsync(x => x.Id == id);
            if (matchingRequest == null) return null;

            RegistrationRequest request = await GetRequestAsync(id);
            request.Review = reviewComment;
            request.RegistrationStatus = RegistrationStatus.Rejected.GetEnumMemberValue();
            request.ReviewedAt = DateTime.UtcNow;

            return await _requestRepository.UpdateAsync(request);
        }
    }
}