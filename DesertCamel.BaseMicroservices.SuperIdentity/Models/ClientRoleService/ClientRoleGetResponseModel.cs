﻿namespace DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientRoleService
{
    public class ClientRoleGetResponseModel
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; }

        public string ClientName { get; set; }

        public string ResourceName { get; set; }
    }
}