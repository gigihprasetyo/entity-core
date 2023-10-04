using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.Infrastructure;
using System.Collections.Generic;

namespace qcs_product.API.EventHandlers
{
    public class FacilityEventHandler : IIntegrationEventHandler<FacilityIntegrationEvent>
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<FacilityEventHandler> _logger;
        private readonly IOrganizationDataProvider _organizationDataProvider;
        private readonly IFacilityDataProvider _facilityDataProvider;
        private readonly IRoomDataProvider _roomDataProvider;
        private readonly IFacilityRoomDataProvider _facilityRoomDataProvider;
        private readonly RoomEventHandler _roomEventHandler;

        public FacilityEventHandler(QcsProductContext context, ILogger<FacilityEventHandler> logger,
            IOrganizationDataProvider organizationDataProvider, IFacilityDataProvider facilityDataProvider, IRoomDataProvider roomDataProvider, IFacilityRoomDataProvider facilityRoomDataProvider,
            RoomEventHandler roomEventHandler)
        {
            _context = context;
            _logger = logger;
            _facilityDataProvider = facilityDataProvider;
            _organizationDataProvider = organizationDataProvider;
            _roomDataProvider = roomDataProvider;
            _facilityRoomDataProvider = facilityRoomDataProvider;
            _roomEventHandler = roomEventHandler;
        }

        public async Task Handle(FacilityIntegrationEvent @event)
        {
            _logger.LogInformation("sync facility from google pub/sub");
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(@event));

                #region insert organization if not exists

                _logger.LogInformation("insert or update organization");

                Organization organization = null;

                if (!string.IsNullOrEmpty(@event.OrganizationCode))
                {
                    organization = await _organizationDataProvider.GetDetailByCode(@event.OrganizationCode, true);
                    if (organization == null)
                    {
                        organization = new Organization();
                        organization.OrgCode = @event.OrganizationCode;
                        organization.Name = @event.OrganizationName;
                        organization.BIOHROrganizationId = @event.BIOHROrganizationId;
                        organization.UpdatedAt = DateTime.Now;
                        organization.CreatedBy = @event.CreatedBy;
                        organization.UpdatedBy = @event.UpdatedBy;
                        organization.RowStatus = @event.RowStatus;
                        organization = await _organizationDataProvider.Insert(organization);
                    }
                }

                #endregion

                #region insert or update facility

                _logger.LogInformation("insert or update facility");

                var isNew = false;
                var facility = await _facilityDataProvider.GetByCode(@event.Code);
                if (facility == null)
                {
                    if (organization == null)
                    {
                        throw new Exception("Can not insert facility. Organization is empty");
                    }

                    facility = new Facility();
                    facility.Code = @event.Code;
                    facility.OrganizationId = organization.Id;
                    facility.CreatedAt = DateTime.Now;
                    facility.CreatedBy = @event.CreatedBy;
                    isNew = true;
                }
                else
                {
                    if (facility.OrganizationId == 0 && organization != null)
                    {
                        facility.OrganizationId = organization.Id;
                    }
                }

                facility.Name = @event.Name;
                facility.UpdatedAt = DateTime.Now;
                facility.UpdatedBy = @event.UpdatedBy;
                facility.RowStatus = @event.RowStatus;
                facility.ObjectStatus = @event.ObjectStatus;

                if (isNew)
                {
                    await _facilityDataProvider.Insert(facility);
                }
                else
                {
                    await _facilityDataProvider.Update(facility);
                }

                await Task.Delay(2000);

                #endregion

                #region insert or update room
                var facilityRooms = await _facilityRoomDataProvider.GetByFacilityId(facility.Id);
                if (facilityRooms != null)
                {
                    _context.RoomFacilities.RemoveRange(facilityRooms);
                    await _context.SaveChangesAsync();
                }

                if (@event.Rooms != null)
                {
                    _logger.LogInformation("insert or update room");
                    foreach (var roomEvent in @event.Rooms)
                    {
                        var room = await _roomDataProvider.GetByCode(roomEvent.Code);
                        if (room == null)
                        {
                            await _roomEventHandler.Handle(roomEvent);
                        }

                        #region insert or update facility room

                        room = await _roomDataProvider.GetByCode(roomEvent.Code);

                        var facilityRoom = new RoomFacility();
                        facilityRoom.FacilityId = facility.Id;
                        facilityRoom.RoomId = room.Id;
                        facilityRoom.CreatedAt = DateTime.Now;
                        facilityRoom.CreatedBy = @event.CreatedBy;
                        facilityRoom.UpdatedAt = DateTime.Now;
                        facilityRoom.UpdatedBy = @event.UpdatedBy;
                        facilityRoom.RowStatus = @event.RowStatus;

                        await _facilityRoomDataProvider.Insert(facilityRoom);

                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
        }
    }
}