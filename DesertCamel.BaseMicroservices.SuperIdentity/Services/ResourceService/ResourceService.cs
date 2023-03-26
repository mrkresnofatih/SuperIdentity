using DesertCamel.BaseMicroservices.SuperIdentity.Entity;
using DesertCamel.BaseMicroservices.SuperIdentity.EntityFramework;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ResourceService;
using DesertCamel.BaseMicroservices.SuperIdentity.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperIdentity.Services.ResourceService
{
    public class ResourceService : IResourceService
    {
        private readonly SuperCognitoDbContext _superCognitoDbContext;
        private readonly ILogger<ResourceService> _logger;

        public ResourceService(
            SuperCognitoDbContext superCognitoDbContext,
            ILogger<ResourceService> logger)
        {
            _superCognitoDbContext = superCognitoDbContext;
            _logger = logger;
        }

        public async Task<FuncResponse<ResourceCreateResponseModel>> Create(ResourceCreateRequestModel createRequest)
        {
            try
            {
                _logger.LogInformation($"Start CreateResource w. data: {createRequest.ToJson()}");
                var newResource = new ResourceEntity
                {
                    Id = Guid.NewGuid(),
                    Name = createRequest.Name,
                    Description = createRequest.Description
                };

                _superCognitoDbContext.Resources.Add(newResource);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("success create resource");
                return new FuncResponse<ResourceCreateResponseModel>
                {
                    Data = new ResourceCreateResponseModel
                    {
                        Id = newResource.Id,
                        Name = newResource.Name,
                        Description = newResource.Description
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to create resource");
                return new FuncResponse<ResourceCreateResponseModel>
                {
                    ErrorMessage = "failed to create resource"
                };
            }
        }

        public async Task<FuncResponse<ResourceDeleteResponseModel>> Delete(ResourceDeleteRequestModel deleteRequest)
        {
            try
            {
                _logger.LogInformation($"Start ResourceDelete w. data: {deleteRequest.ToJson()}");
                var foundResource = await _superCognitoDbContext
                    .Resources
                    .Where(x => x.Name.Equals(deleteRequest.Name))
                    .FirstOrDefaultAsync();
                if (foundResource == null)
                {
                    throw new Exception("Resource for delete op is not found");
                }

                _superCognitoDbContext.Resources.Remove(foundResource);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("success deletePermission");
                return new FuncResponse<ResourceDeleteResponseModel>
                {
                    Data = new ResourceDeleteResponseModel()
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to delete resource");
                return new FuncResponse<ResourceDeleteResponseModel>
                {
                    ErrorMessage = "failed to delete resource"
                };
            }
        }

        public async Task<FuncResponse<ResourceGetResponseModel>> Get(ResourceGetRequestModel getRequest)
        {
            try
            {
                _logger.LogInformation($"Start GetResource w. data: {getRequest.ToJson()}");
                var foundResource = await _superCognitoDbContext
                    .Resources
                    .Where(x => x.Name.Equals(getRequest.Name))
                    .FirstOrDefaultAsync();
                if (foundResource == null)
                {
                    throw new Exception("resource for get op is not found");
                }

                _logger.LogInformation("success get resource");
                return new FuncResponse<ResourceGetResponseModel>
                {
                    Data = new ResourceGetResponseModel
                    {
                        Id = foundResource.Id,
                        Name = foundResource.Name,
                        Description = foundResource.Description,
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to get permission");
                return new FuncResponse<ResourceGetResponseModel>
                {
                    ErrorMessage = "failed to get resource"
                };
            }
        }

        public async Task<FuncListResponse<ResourceGetResponseModel>> List(ResourceListRequestModel listRequest)
        {
            try
            {
                _logger.LogInformation($"Start listResources w. data: {listRequest.ToJson()}");
                var query = _superCognitoDbContext.Resources.AsQueryable();

                if (!String.IsNullOrWhiteSpace(listRequest.Name))
                {
                    query = query.Where(x => x.Name.Contains(listRequest.Name));
                }

                var total = await query.CountAsync();

                query = query.OrderBy(x => x.Name);

                var foundResources = await query
                    .Skip((int)listRequest.PageSize * ((int)listRequest.Page - 1))
                    .Take((int)listRequest.PageSize)
                    .Select(x => new ResourceGetResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToListAsync();

                _logger.LogInformation("success list resources");
                return new FuncListResponse<ResourceGetResponseModel>
                {
                    Total = total,
                    Data = foundResources,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to list resources");
                return new FuncListResponse<ResourceGetResponseModel>
                {
                    ErrorMessage = "failed to list resources"
                };
            }
        }

        public async Task<FuncResponse<ResourceUpdateResponseModel>> Update(ResourceUpdateRequestModel updateRequest)
        {
            try
            {
                _logger.LogInformation($"Start UpdateResource w. data: {updateRequest.ToJson()}");
                var foundResource = await _superCognitoDbContext
                    .Resources
                    .Where(x => x.Name.Equals(updateRequest.Name))
                    .FirstOrDefaultAsync();
                if (foundResource == null)
                {
                    throw new Exception("resources for update op not found");
                }

                foundResource.Description = updateRequest.Description;
                _superCognitoDbContext.Resources.Update(foundResource);
                await _superCognitoDbContext.SaveChangesAsync();

                _logger.LogInformation("success update permission");
                return new FuncResponse<ResourceUpdateResponseModel>
                {
                    Data = new ResourceUpdateResponseModel
                    {
                        Id = foundResource.Id,
                        Name = foundResource.Name,
                        Description = updateRequest.Description
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to update resource");
                return new FuncResponse<ResourceUpdateResponseModel>
                {
                    ErrorMessage = "failed to update resource"
                };
            }
        }
    }
}
