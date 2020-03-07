using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.Azure.Core;
using Pulumi.Azure.KeyVault;

internal class Program
{
    private const string KeyVaultId =
        "/subscriptions/47a9c0f7-9e3a-4f74-8803-a4c966fedff6/resourceGroups/DevTestKeyVaultGroup/providers/Microsoft.KeyVault/vaults/PulumiVault";

    private static Task<int> Main()
    {
        return Deployment.RunAsync(Logic);
    }

    private static IDictionary<string, object> Logic()
    {
        var resourceGroup = new ResourceGroup("DevTestKeyVaultGroup",
                                              new ResourceGroupArgs
                                              {
                                                  Location = "WestEurope", Name = "DevTestKeyVaultGroup", Tags = new InputMap<string>()
                                              },
                                              new CustomResourceOptions
                                              {
                                                  ImportId = "/subscriptions/47a9c0f7-9e3a-4f74-8803-a4c966fedff6/resourceGroups/DevTestKeyVaultGroup"
                                              });

        var keyVault = new KeyVault("PulumiVault",
                                    new KeyVaultArgs
                                    {
                                        Name = "PulumiVault",
                                        ResourceGroupName = resourceGroup.Name,
                                        TenantId = "407da715-6eac-44a1-ac21-3f0b4e84877c",
                                        EnabledForDeployment = true,
                                        EnabledForDiskEncryption = true,
                                        EnabledForTemplateDeployment = true
                                    },
                                    new CustomResourceOptions {ImportId = KeyVaultId});
        return new Dictionary<string, object?> {{"VaultLocation", keyVault.Location}};
    }
}
