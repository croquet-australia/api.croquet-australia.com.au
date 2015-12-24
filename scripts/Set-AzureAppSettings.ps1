# Adds application settings to Azure.
#
# This script needs to be run before first publishing of the website to Azure or
# if any of the settings change.

param(
    [CmdletBinding(SupportsShouldProcess=$true)]
         
    [Parameter(Mandatory = $true)] 
    [string] $ConnectionStringAzureStorage,
    [Parameter(Mandatory = $true)]
    [string] $MailInBlueAccessId,
    [Parameter(Mandatory = $false)] 
    [string] $WebSiteName = "croquet-australia-api",
    [Parameter(Mandatory = $false)] 
    [string] $WebAppBaseUri = "https://croquet-australia.com.au/"
)

$appSettings = @{ ` 
    "ConnectionString:AzureStorage" = $ConnectionStringAzureStorage; `
    "WebApp:BaseUri" = $WebAppBaseUri;
}

# todo: Set connection string WebJobDashboardxxxxx
Set-AzureWebsite -Name "$WebSiteName" -AppSettings $appSettings 