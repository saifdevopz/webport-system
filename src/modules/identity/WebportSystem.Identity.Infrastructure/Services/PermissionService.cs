#pragma warning disable S125 // Sections of code should not be commented out
                            //using Webport.ERP.Common.Application.Authorization;
#pragma warning restore S125 // Sections of code should not be commented out
                            //using Webport.ERP.Common.Application.Messaging;
                            //using Webport.ERP.Common.Domain.Results;
                            //using Webport.ERP.Identity.Application.Features.Permissions;
                            //using WebportSystem.Common.Application.Abstractions;

//namespace WebportSystem.Identity.Infrastructure.Services;


//internal sealed class PermissionService(
//    IQueryHandler<GetPermissionsByUserIdQuery, GetPermissionsByUserIdQueryResult> handler)
//    : IPermissionService
//{
//    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(int userId)
//    {
//        var response = await handler
//            .Handle(new GetPermissionsByUserIdQuery(userId), default);

//        return Result.Success(response.Data.Permissions);
//    }
//}