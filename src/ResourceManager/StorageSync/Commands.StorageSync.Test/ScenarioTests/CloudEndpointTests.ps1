﻿# ----------------------------------------------------------------------------------
#
# Copyright Microsoft Corporation
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# http://www.apache.org/licenses/LICENSE-2.0
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ----------------------------------------------------------------------------------

<#
.SYNOPSIS
Test CloudEndpoint
.DESCRIPTION
SmokeTest
#>
function Test-CloudEndpoint
{
    # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        Get-Help New-AzureRmStorageAccount | Out-file New-AzureRmStorageAccount.txt

        $UseStorageModule= $true
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $cloudEndpointName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;
        $StorageAccountShareName = Get-StorageManagementTestResourceName
        $StorageAccountName = Get-StorageManagementTestResourceName
        $StorageAccountTenantId = (Get-AzureRmTenant).Id

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation | Type : ResourceGroup"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation | Type : StorageSyncService"
        New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation | Type : SyncGroup"
        $syncGroup = New-AzureRmStorageSyncGroup -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        if($UseStorageModule)
        {
            Write-Verbose "Resource: $StorageAccountName | Loc: $resourceLocation | Type : StorageAccount"
            $storageAccount = New-AzureRmStorageAccount  -SkuName Standard_LRS -ResourceGroupName $ResourceGroupName -Name $StorageAccountName -Location $resourceLocation
            Write-Verbose "Resource: $StorageAccountShareName | Loc: $resourceLocation | Type : AzureStorageShare"
            $azureFileShare = New-AzureStorageShare -Name $StorageAccountShareName -Context $StorageAccount.Context

            $storageAccountResourceId = $storageAccount.Id
            $azureFileShareName = $azureFileShare.Name 
        }
        else
        {
            $storageAccountResourceId = "/subscriptions/1d16f9b3-bbe3-48d4-930a-27a74dca003b/resourceGroups/sasdkwestcentralus/providers/Microsoft.Storage/storageAccounts/sasdkwestcentralus"
            $azureFileShareName = "afsfileshareps1"
        }

        Write-Verbose "Resource: $cloudEndpointName | Loc: $resourceLocation | Type : CloudEndpoint"
        $cloudEndpoint = New-AzureRmStorageSyncCloudEndpoint -ResourceGroupName $syncGroup.ResourceGroupName  -StorageSyncServiceName $syncGroup.StorageSyncServiceName -SyncGroupName $syncGroup.SyncGroupName -Name $cloudEndpointName -StorageAccountResourceId $storageAccountResourceId -StorageAccountShareName $azureFileShareName -StorageAccountTenantId $StorageAccountTenantId -Verbose

        Write-Verbose "Validating CloudEndpoint Properties"
        Assert-AreEqual $cloudEndpointName $cloudEndpoint.CloudEndpointName
        Assert-AreEqual $storageAccountResourceId $cloudEndpoint.StorageAccountResourceId
        Assert-AreEqual $StorageAccountTenantId $cloudEndpoint.StorageAccountTenantId

        Write-Verbose "Get CloudEndpoint by Name"
        $cloudEndpoint = Get-AzureRMStorageSyncCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -SyncGroupName $syncGroupName -CloudEndpointName $cloudEndpointName 

        Write-Verbose "Validating CloudEndpoint Properties"
        Assert-AreEqual $cloudEndpointName $cloudEndpoint.CloudEndpointName
        Assert-AreEqual $StorageAccount.Id $cloudEndpoint.StorageAccountResourceId
        Assert-AreEqual $StorageAccountTenantId $cloudEndpoint.StorageAccountTenantId

        Write-Verbose "Get CloudEndpoint by ParentObject"
        $cloudEndpoint = Get-AzureRMStorageSyncCloudEndpoint -ParentObject $syncGroup -Name $cloudEndpointName -Verbose
        Write-Verbose "Validating CloudEndpoint Properties"
        Assert-AreEqual $cloudEndpointName $cloudEndpoint.CloudEndpointName
        Assert-AreEqual $StorageAccount.Id $cloudEndpoint.StorageAccountResourceId
        Assert-AreEqual $StorageAccountTenantId $cloudEndpoint.StorageAccountTenantId

        Write-Verbose "Get CloudEndpoint by ParentResourceId"
        $cloudEndpoint = Get-AzureRMStorageSyncCloudEndpoint -ParentResourceId $syncGroup.ResourceId -Name $cloudEndpointName -Verbose
        Write-Verbose "Validating CloudEndpoint Properties"
        Assert-AreEqual $cloudEndpointName $cloudEndpoint.CloudEndpointName
        Assert-AreEqual $StorageAccount.Id $cloudEndpoint.StorageAccountResourceId
        Assert-AreEqual $StorageAccountTenantId $cloudEndpoint.StorageAccountTenantId

        Write-Verbose "Removing CloudEndpoint: $cloudEndpointName"
        Remove-AzureRMStorageSyncCloudEndpoint -Force -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -SyncGroupName $syncGroupName -Name $cloudEndpointName -AsJob | Wait-Job

        Write-Verbose "Executing Piping Scenarios"
        New-AzureRMStorageSyncCloudEndpoint -ParentObject $syncGroup -Name $cloudEndpointName -StorageAccountResourceId $storageAccountResourceId -StorageAccountShareName $azureFileShareName -StorageAccountTenantId $StorageAccountTenantId | Get-AzureRMStorageSyncCloudEndpoint  | Remove-AzureRMStorageSyncCloudEndpoint -Force -AsJob | Wait-Job

        New-AzureRMStorageSyncCloudEndpoint -ParentResourceId $syncGroup.ResourceId -Name $cloudEndpointName -StorageAccountResourceId $storageAccountResourceId -StorageAccountShareName $azureFileShareName -StorageAccountTenantId $StorageAccountTenantId | Remove-AzureRMStorageSyncCloudEndpoint -Force -AsJob | Wait-Job

        Write-Verbose "Removing SyncGroup: $syncGroupName"
        Remove-AzureRmStorageSyncGroup -Force -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName -AsJob | Wait-Job

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName -AsJob | Wait-Job

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
       # Clean-ResourceGroup $resourceGroupName
    }
}

<#
.SYNOPSIS
Test NewCloudEndpoint
.DESCRIPTION
SmokeTest
#>
function Test-NewCloudEndpoint
{
   # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation"
        New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation"
        $syncGroup = New-AzureRmStorageSyncGroup -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

         Write-Verbose "Validating SyncGroup Properties"
        Assert-AreEqual $syncGroupName $syncGroup.SyncGroupName

        Write-Verbose "Removing SyncGroup: $syncGroupName"
        Remove-AzureRmStorageSyncGroup -Force -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
        Clean-ResourceGroup $resourceGroupName
    }
}

<#
.SYNOPSIS
Test GetCloudEndpoint
.DESCRIPTION
SmokeTest
#>
function Test-GetCloudEndpoint
{
    # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation"
        New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation"
        New-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Get CloudEndpoint by Name"
        $syncGroup = Get-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName -Verbose
         Write-Verbose "Validating CloudEndpoint Properties"
        Assert-AreEqual $syncGroupName $syncGroup.CloudEndpointName

        Write-Verbose "Removing CloudEndpoint: $syncGroupName"
        Remove-AzureRmStorageCloudEndpoint -Force -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
        Clean-ResourceGroup $resourceGroupName
    }
}

<#
.SYNOPSIS
Test GetCloudEndpoints
.DESCRIPTION
SmokeTest
#>
function Test-GetCloudEndpoints
{
    # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation"
        New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation"
        New-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Get CloudEndpoint by Name"
        $syncGroups = Get-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Verbose

        Assert-AreEqual $syncGroups.Length 1
        $syncGroup = $syncGroups[0]

         Write-Verbose "Validating CloudEndpoint Properties"
        Assert-AreEqual $syncGroupName $syncGroup.CloudEndpointName

        Write-Verbose "Removing CloudEndpoint: $syncGroupName"
        Remove-AzureRmStorageCloudEndpoint -Force -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
        Clean-ResourceGroup $resourceGroupName
    }
}

<#
.SYNOPSIS
Test GetCloudEndpointParentObject
.DESCRIPTION
SmokeTest
#>
function Test-GetCloudEndpointParentObject
{
    # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation"
        $storageSyncService = New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation"
        New-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Get CloudEndpoint by ParentObject"
        $syncGroup = Get-AzureRmStorageCloudEndpoint -ParentObject $storageSyncService -Name $syncGroupName -Verbose
        Write-Verbose "Validating CloudEndpoint Properties"
        Assert-AreEqual $syncGroupName $syncGroup.CloudEndpointName

        Write-Verbose "Removing CloudEndpoint: $syncGroupName"
        Remove-AzureRmStorageCloudEndpoint -Force -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
        Clean-ResourceGroup $resourceGroupName
    }
}

<#
.SYNOPSIS
Test GetCloudEndpointParentResourceId
.DESCRIPTION
SmokeTest
#>
function Test-GetCloudEndpointParentResourceId
{
    # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation"
        $storageSyncService = New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation"
        New-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Get CloudEndpoint by ParentResourceId"
        $syncGroup = Get-AzureRmStorageCloudEndpoint -ParentResourceId $storageSyncService.ResourceId -Name $syncGroupName -Verbose
        Write-Verbose "Validating CloudEndpoint Properties"
        Assert-AreEqual $syncGroupName $syncGroup.CloudEndpointName

        Write-Verbose "Removing CloudEndpoint: $syncGroupName"
        Remove-AzureRmStorageCloudEndpoint -Force -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
        Clean-ResourceGroup $resourceGroupName
    }
}

<#
.SYNOPSIS
Test RemoveCloudEndpoint
.DESCRIPTION
SmokeTest
#>
function Test-RemoveCloudEndpoint
{
     # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation"
        New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation"
        New-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Removing CloudEndpoint: $syncGroupName"
        Remove-AzureRmStorageCloudEndpoint -Force -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName 

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
        Clean-ResourceGroup $resourceGroupName
    }
}

<#
.SYNOPSIS
Test RemoveCloudEndpointInputObject
.DESCRIPTION
SmokeTest
#>
function Test-RemoveCloudEndpointInputObject
{
     # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation"
        New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation"
        $syncGroup = New-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Removing CloudEndpoint: $syncGroupName"
        Remove-AzureRmStorageCloudEndpoint -InputObject $syncGroup -Force

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
        Clean-ResourceGroup $resourceGroupName
    }
}


<#
.SYNOPSIS
Test RemoveCloudEndpointInputObject
.DESCRIPTION
SmokeTest
#>
function Test-RemoveCloudEndpointResourceId
{
     # Setup
    $resourceGroupName = Get-StorageManagementTestResourceName;

    try
    {
        # Test
        $storageSyncServiceName = Get-StorageManagementTestResourceName
        $syncGroupName = Get-StorageManagementTestResourceName
        $resourceLocation = Get-ProviderLocation ResourceManagement;

        Write-Verbose "RGName: $resourceGroupName | Loc: $resourceLocation"
        New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceLocation;

        Write-Verbose "Resource: $storageSyncServiceName | Loc: $resourceLocation"
        New-AzureRmStorageSyncService -ResourceGroupName $resourceGroupName -Location $resourceLocation -StorageSyncServiceName $storageSyncServiceName

        Write-Verbose "Resource: $syncGroupName | Loc: $resourceLocation"
        $syncGroup = New-AzureRmStorageCloudEndpoint -ResourceGroupName $resourceGroupName -StorageSyncServiceName $storageSyncServiceName -Name $syncGroupName

        Write-Verbose "Removing CloudEndpoint: $syncGroupName"
        Remove-AzureRmStorageCloudEndpoint -ResourceId $syncGroup.ResourceId -Force

        Write-Verbose "Removing StorageSyncService: $storageSyncServiceName"
        Remove-AzureRmStorageSyncService -Force -ResourceGroupName $resourceGroupName -Name $storageSyncServiceName

    }
    finally
    {
        # Cleanup
        Write-Verbose "Removing ResourceGroup : $resourceGroupName"
        Clean-ResourceGroup $resourceGroupName
    }
}