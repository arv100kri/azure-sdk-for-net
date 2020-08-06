// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

namespace Microsoft.Azure.Management.Search.Tests
{
    using Microsoft.Azure.Management.Search.Models;
    using Microsoft.Azure.Search.Tests.Utilities;
    using System;
    using Xunit;

    public sealed class SharedPrivateLinkResourceTests : SearchTestBase<SharedPrivateLinkResourceFixture>
    {
        [Fact]
        public void CanPerformSharedPrivateLinkManagement()
        {
            Run(() =>
            {
                SearchManagementClient searchMgmt = GetSearchManagementClient();
                
                string serviceName = SearchTestUtilities.GenerateServiceName();
                SearchService service = DefineServiceWithSku(SkuName.Basic);
                service.Identity = new Identity(IdentityType.SystemAssigned);
                service = searchMgmt.Services.CreateOrUpdate(Data.ResourceGroupName, serviceName, service);

                Data.CreateKeyVault(service.Identity.PrincipalId, Guid.Parse(service.Identity.TenantId));

                SharedPrivateLinkResource resource = searchMgmt.SharedPrivateLinkResources.CreateOrUpdate(
                    Data.ResourceGroupName,
                    service.Name,
                    Data.SharedPrivateLinkResourceName,
                    new SharedPrivateLinkResource(
                        name: Data.SharedPrivateLinkResourceName,
                        properties: new SharedPrivateLinkResourceProperties(
                            privateLinkResourceId: Data.KeyVaultId,
                            groupId: Data.SharedPrivateLinkResourceGroupId,
                            requestMessage: Data.SharedPrivateLinkResourceRequestMessage)));

                searchMgmt.Services.Delete(Data.ResourceGroupName, service.Name);
            });
        }

        private SearchService DefineServiceWithSku(SkuName sku)
        {
            return new SearchService()
            {
                Location = "EastUS",
                Sku = new Sku() { Name = sku },
                ReplicaCount = 1,
                PartitionCount = 1
            };
        }

        private delegate SearchService SearchServiceDefinition(SkuName sku);
    }
}
