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
        return Deployment.RunAsync(() =>
                                   {
                                       // Create an Azure Resource Group
                                       var resourceGroup = new ResourceGroup("DevTestKeyVaultGroup",
                                                                             new ResourceGroupArgs
                                                                             {
                                                                                 Location = "WestEurope",
                                                                                 Name = "DevTestKeyVaultGroup",
                                                                                 Tags = new InputMap<string>()
                                                                             },
                                                                             new CustomResourceOptions
                                                                             {
                                                                                 ImportId =
                                                                                     "/subscriptions/47a9c0f7-9e3a-4f74-8803-a4c966fedff6/resourceGroups/DevTestKeyVaultGroup"
                                                                             });

                                       var keyVault = new KeyVault("PulumiVault",
                                                                   new KeyVaultArgs {ResourceGroupName = resourceGroup.Name},
                                                                   new CustomResourceOptions {ImportId = KeyVaultId});
                                       return new Dictionary<string, object?> {{"VaultLocation", keyVault.Location}};
                                   });
    }
}
