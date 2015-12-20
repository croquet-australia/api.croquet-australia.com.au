# Adds application settings to Azure.
#
# This script needs to be run before first publishing of the website to Azure or
# if any of the settings change.

param(
    [CmdletBinding(SupportsShouldProcess=$true)]
         
    [Parameter(Mandatory = $true)] 
    [string] $ConnectionStringAzureStorage,
    [Parameter(Mandatory = $false)] 
    [string] $WebSiteName = "croquet-australia-api",
    [Parameter(Mandatory = $false)] 
    [string] $WebAppBaseUri = "https://croquet-australia-tournaments.azurewebsites.net/"
)

$appSettings = @{ ` 
    "ConnectionString:AzureStorage" = $ConnectionStringAzureStorage;
    "WebApp:BaseUri" = $WebAppBaseUri;
}

Set-AzureWebsite -Name "$WebSiteName" -AppSettings $appSettings 