// https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/deploy-github-actions?tabs=CLI%2Cuserlevel

param location string = resourceGroup().location

resource webApplication 'Microsoft.Web/staticSites@2022-09-01' = {
  name: 'ss-site-reflectionnaire'
  location: location
  properties: {}
  sku: {
    name: 'Free'
  }
  kind: 'linux'
}

resource webApplication_appsettings 'Microsoft.Web/staticSites/config@2021-01-15' = {
  parent: webApplication
  name: 'appsettings'
  properties: {
    APPLICATIONINSIGHTS_CONNECTION_STRING: applicationInsights.properties.ConnectionString
    ReflectionnaireOptions__TableStorageConnectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageaccount.name};AccountKey=${listKeys(storageaccount.id, storageaccount.apiVersion).keys[0].value};EndpointSuffix=${environment().suffixes.storage}'
  }
}

resource customDomain 'Microsoft.Web/staticSites/customDomains@2021-02-01' = {
  parent: webApplication
  name: 'www.reflectionnaire.com'
  properties: {}
}

resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: 'stdatareflectionnaire'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

resource tableservices 'Microsoft.Storage/storageAccounts/tableServices@2023-01-01' = {
  name: 'default'
  parent: storageaccount
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'ai-reflectionnaire'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}
