using AutoMapper;
using BELMS.Application.DTOs.AccessRequests;
using BELMS.Application.DTOs.Assets;
using BELMS.Application.DTOs.Employees;
using BELMS.Application.DTOs.Notifications;
using BELMS.Application.DTOs.Workflows;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;
using DomainTaskStatus = BELMS.Domain.Enums.TaskStatus;

namespace BELMS.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateEmployeeRequest, Employee>()
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email.Trim().ToLowerInvariant()))
            .ForMember(d => d.Status, o => o.MapFrom(_ => EmployeeStatus.Pending))
            .ForMember(d => d.EmployeeAssets, o => o.Ignore());

        CreateMap<Employee, CreateEmployeeRequest>();
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.WorkflowInstanceId, o => o.Ignore());
        CreateMap<EmployeeDto, Employee>()
            .ForMember(d => d.Status, o => o.MapFrom(s => Enum.Parse<EmployeeStatus>(s.Status)))
            .ForMember(d => d.EmployeeAssets, o => o.Ignore());

        CreateMap<Asset, AssetDto>()
            .ForMember(d => d.AssetType, o => o.MapFrom(s => s.AssetType.ToString()));
        CreateMap<CreateAssetRequest, Asset>()
            .ForMember(d => d.AssetType, o => o.Ignore())
            .ForMember(d => d.EmployeeAssets, o => o.Ignore());

        CreateMap<AccessRequest, AccessRequestDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.EmployeeName, o => o.Ignore())
            .ForMember(d => d.RequestedByName, o => o.Ignore());

        CreateMap<CreateWorkflowStepRequest, WorkflowStepDefinition>()
            .ForMember(d => d.AssignedRole, o => o.MapFrom(s => Enum.Parse<Role>(s.AssignedRole, true)));

        CreateMap<WorkflowStepDefinition, CreateWorkflowStepRequest>()
            .ForMember(d => d.AssignedRole, o => o.MapFrom(s => s.AssignedRole.ToString()));

        CreateMap<CreateWorkflowDefinitionRequest, WorkflowDefinition>()
            .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
            .ForMember(d => d.Steps, o => o.MapFrom(s => s.Steps.OrderBy(x => x.StepOrder)));

        CreateMap<WorkflowDefinition, CreateWorkflowDefinitionRequest>()
            .ForMember(d => d.Steps, o => o.MapFrom(s => s.Steps.OrderBy(x => x.StepOrder)));

        CreateMap<WorkflowDefinition, WorkflowDefinitionDto>()
            .ForMember(d => d.Steps, o => o.MapFrom(s => s.Steps.OrderBy(x => x.StepOrder)));
        CreateMap<WorkflowDefinitionDto, WorkflowDefinition>()
            .ForMember(d => d.Steps, o => o.Ignore());

        CreateMap<WorkflowStepDefinition, WorkflowStepDefinitionDto>()
            .ForMember(d => d.AssignedRole, o => o.MapFrom(s => s.AssignedRole.ToString()));
        CreateMap<WorkflowStepDefinitionDto, WorkflowStepDefinition>()
            .ForMember(d => d.AssignedRole, o => o.MapFrom(s => Enum.Parse<Role>(s.AssignedRole, true)));

        CreateMap<WorkflowInstance, WorkflowInstanceDto>()
            .ForMember(d => d.EntityType, o => o.MapFrom(s => s.EntityType.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Tasks, o => o.MapFrom(s => s.Tasks.OrderBy(x => x.StepOrder)));
        CreateMap<WorkflowInstanceDto, WorkflowInstance>()
            .ForMember(d => d.EntityType, o => o.MapFrom(s => Enum.Parse<WorkflowEntityType>(s.EntityType)))
            .ForMember(d => d.Status, o => o.MapFrom(s => Enum.Parse<WorkflowInstanceStatus>(s.Status)))
            .ForMember(d => d.Tasks, o => o.Ignore());

        CreateMap<WorkflowTask, WorkflowTaskDto>()
            .ForMember(d => d.AssignedRole, o => o.MapFrom(s => s.AssignedRole.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<WorkflowTaskDto, WorkflowTask>()
            .ForMember(d => d.AssignedRole, o => o.MapFrom(s => Enum.Parse<Role>(s.AssignedRole, true)))
            .ForMember(d => d.Status, o => o.MapFrom(s => Enum.Parse<DomainTaskStatus>(s.Status)));

        CreateMap<WorkflowAttachment, WorkflowAttachmentDto>();
        CreateMap<WorkflowAttachmentDto, WorkflowAttachment>();

        CreateMap<Notification, NotificationDto>();
        CreateMap<CreateNotificationRequest, Notification>()
            .ForMember(d => d.IsRead, o => o.MapFrom(_ => false));
    }
}
