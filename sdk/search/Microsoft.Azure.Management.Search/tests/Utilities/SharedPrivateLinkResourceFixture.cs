// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

namespace Microsoft.Azure.Search.Tests.Utilities
{
    using Microsoft.Azure.Management.KeyVault;
    using Microsoft.Azure.Management.KeyVault.Models;
    using Microsoft.Azure.Management.ResourceManager;
    using Microsoft.Azure.Management.ResourceManager.Models;
    using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class SharedPrivateLinkResourceFixture : ResourceGroupFixture
    {
        public string SharedPrivateLinkResourceName => $"pe-{KeyVaultName}";

        public string SharedPrivateLinkResourceGroupId => "vault";

        public string SharedPrivateLinkResourceRequestMessage => "Approve me";

        public string KeyVaultName { get; private set; }

        public string KeyVaultId { get; private set; }

        public MockContext MockContext { get; private set; }

        public override void Initialize(MockContext context)
        {
            base.Initialize(context);

            MockContext = context;

            ResourceManagementClient rmClient = context.GetServiceClient<ResourceManagementClient>();

            // Register subscription for storageAccounts
            Provider provider = rmClient.Providers.Register("Microsoft.KeyVault");
            Assert.NotNull(provider);

            KeyVaultName = SearchTestUtilities.GenerateVaultName();
        }

        public void CreateKeyVault(string principalId, Guid tenantId)
        {
            KeyVaultManagementClient client = MockContext.GetServiceClient<KeyVaultManagementClient>();

            var accessPolicies = new AccessPolicyEntry()
            {
                ObjectId = principalId,
                TenantId = tenantId,
                Permissions = new Microsoft.Azure.Management.KeyVault.Models.Permissions()
                {
                    Keys = new List<string> { "get", "wrapKey", "unwrapKey" }
                }
            };

            Vault vault = client.Vaults.CreateOrUpdate(
                ResourceGroupName,
                KeyVaultName, new VaultCreateOrUpdateParameters(
                    "eastus2",
                    new VaultProperties
                    {
                        TenantId = tenantId,
                        AccessPolicies = new List<AccessPolicyEntry>
                        {
                            accessPolicies
                        }
                    }));

            Assert.NotNull(vault);

            KeyVaultId = vault.Id;
        }

        public override void Cleanup()
        {
            if (ResourceGroupName != null && KeyVaultName != null)
            {
                KeyVaultManagementClient client = MockContext.GetServiceClient<KeyVaultManagementClient>();
                client.Vaults.Delete(ResourceGroupName, KeyVaultName);
            }

            base.Cleanup();
        }
    }
}
