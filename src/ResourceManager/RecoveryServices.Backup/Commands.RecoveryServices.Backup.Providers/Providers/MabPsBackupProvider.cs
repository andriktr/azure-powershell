﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.Models;
using Microsoft.Azure.Management.RecoveryServices.Backup.Models;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Helpers;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ProviderModel
{
    public class MabPsBackupProvider : IPsBackupProvider
    {
        ProviderData ProviderData { get; set; }
        HydraAdapter.HydraAdapter HydraAdapter { get; set; }

        public void Initialize(ProviderData providerData, HydraAdapter.HydraAdapter hydraAdapter)
        {
            this.ProviderData = providerData;
            this.HydraAdapter = hydraAdapter;
        }       

        public Management.RecoveryServices.Backup.Models.BaseRecoveryServicesJobResponse EnableProtection()
        {
            throw new NotImplementedException();
        }

        public Management.RecoveryServices.Backup.Models.BaseRecoveryServicesJobResponse DisableProtection()
        {
            throw new NotImplementedException();
        }

        public Management.RecoveryServices.Backup.Models.BaseRecoveryServicesJobResponse TriggerBackup()
        {
            throw new NotImplementedException();
        }

        public Management.RecoveryServices.Backup.Models.BaseRecoveryServicesJobResponse TriggerRestore()
        {
            throw new NotImplementedException();
        }

        public Management.RecoveryServices.Backup.Models.ProtectedItemResponse GetProtectedItem()
        {
            throw new NotImplementedException();
        }

        public AzureRmRecoveryServicesRecoveryPointBase GetRecoveryPointDetails()
        {
            throw new NotImplementedException();
        }

        public List<AzureRmRecoveryServicesRecoveryPointBase> ListRecoveryPoints()
        {
            throw new NotImplementedException();
        }

        public Management.RecoveryServices.Backup.Models.ProtectionPolicyResponse CreatePolicy()
        {
            throw new NotImplementedException();
        }

        public List<AzureRmRecoveryServicesJobBase> ModifyPolicy()
        {
            throw new NotImplementedException();
        }

        public List<Models.AzureRmRecoveryServicesContainerBase> ListProtectionContainers()
        {
            string name = (string)this.ProviderData.ProviderParameters[ContainerParams.Name];
            ContainerRegistrationStatus status = (ContainerRegistrationStatus)this.ProviderData.ProviderParameters[ContainerParams.Status];
            string resourceGroupName = (string)this.ProviderData.ProviderParameters[ContainerParams.ResourceGroupName];

            ProtectionContainerListQueryParams queryParams = new ProtectionContainerListQueryParams();

            // 1. Filter by Name
            queryParams.FriendlyName = name;

            // 2. Filter by ContainerType
            queryParams.ProviderType = ProviderType.AzureIaasVM.ToString();

            // 3. Filter by Status
            queryParams.RegistrationStatus = status.ToString();

            var listResponse = HydraAdapter.ListContainers(queryParams);

            List<AzureRmRecoveryServicesContainerBase> containerModels = ConversionHelpers.GetContainerModelList(listResponse);

            // 4. Filter by RG Name
            if (!string.IsNullOrEmpty(resourceGroupName))
            {
                containerModels = containerModels.Where(containerModel =>
                    (containerModel as AzureRmRecoveryServicesDpmContainer).ResourceGroupName == resourceGroupName).ToList();
            }

            return containerModels;
        }

        public AzureOperationResponse UnregisterContainer()
        {
            AzureOperationResponse response = null;

            AzureRmRecoveryServicesContainerBase container = ProviderData.ProviderParameters[ContainerParams.Container]
                as AzureRmRecoveryServicesContainerBase;

            if (container == null)
            {
                throw new InvalidCastException("Cant convert input to AzureRmRecoveryServicesItemBase");
            }

            if (container.ContainerType != Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.Models.ContainerType.Mab)
            {
                throw new InvalidOperationException("Please provide mab container in the input. Provided container is of type " + container.ContainerType);
            }
            AzureRmRecoveryServicesMabContainer mabContainer = container as AzureRmRecoveryServicesMabContainer;
            //string ResourceGroupName = mabContainer.ResourceGroupName;
            //string ResourceName = mabContainer.ResourceName;
            string containerName = mabContainer.Name;

            response = HydraAdapter.UnregisterContainers(containerName); 
            return response;
        }

        public Management.RecoveryServices.Backup.Models.ProtectionPolicyResponse GetPolicy()
        {
            throw new NotImplementedException();
        }

        public void DeletePolicy()
        {
            throw new NotImplementedException();
        }

        public AzureRmRecoveryServicesSchedulePolicyBase GetDefaultSchedulePolicyObject()
        {
            throw new NotImplementedException();
        }

        public AzureRmRecoveryServicesRetentionPolicyBase GetDefaultRetentionPolicyObject()
        {
            throw new NotImplementedException();
        }

        public List<Models.AzureRmRecoveryServicesItemBase> ListProtectedItems()
        {
            throw new NotImplementedException();
        }
    }
}
