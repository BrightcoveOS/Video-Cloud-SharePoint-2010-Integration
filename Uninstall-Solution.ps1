param (
	[string] $Identity = "BrightcoveVideoCloudIntegration.wsp",
	[string] $WebSiteUrl
)

[Int] $currentStep = 0
[Int] $sleepseconds = 15
[string] $siteUrl
$SPWeb = $null

Write-Host ""
Write-Host "-----------------------------------------------------"
Write-Host "de-installation for 'Video Cloud SharePoint 2010 Integration'"
Write-Host "http://opensource.brightcove.com/project/video-cloud-sharepoint-2010-integration"
Write-Host ""
Write-Host "Starting $([DateTime]::Now.ToString())"
Write-Host ""

try {


    if ($Identity -eq $null -or $Identity -eq "" -or $WebSiteUrl -eq $null -or $WebSiteUrl -eq "")
    {
        Write-Host "Supply values for the following parameters"
    }

    if ($Identity -eq $null -or $Identity -eq "")
    {
        $Identity = Read-Host 'Identity'
    }

    if ($WebSiteUrl -eq $null -or $WebSiteUrl -eq "")
    {
        $WebSiteUrl = Read-Host 'WebSiteUrl'
    }



	#------------------------------------------------------------------------------------------------------------------------------
	# check for SP addin, stop if not active
	#------------------------------------------------------------------------------------------------------------------------------

    $currentStep = 1
	Write-Host "$currentStep) Checking for Microsoft.SharePoint.PowerShell snap-in."
	[void](get-pssnapin Microsoft.SharePoint.PowerShell -ea stop)


	#------------------------------------------------------------------------------------------------------------------------------
	# validate existance of site collection
	#------------------------------------------------------------------------------------------------------------------------------	


    $currentStep = 2

    try
    {
        Write-Host "$currentStep) Checking for existance of site collection at '$WebSiteUrl'."
	    $SPWeb = Get-SPWeb -Identity $WebSiteUrl -ea Stop

        if ( -not ($SPWeb.Provisioned))
        {
            throw "Site collection at '$WebSiteUrl' needs to be provisioned before continueing."
        }

        $siteUrl = $SPWeb.Site.Url
    }
    finally
    {
        if ($SPWeb -ne $null)
        {
	        $SPWeb.Dispose()
            $SPWeb = $null
        }
    }


	#------------------------------------------------------------------------------------------------------------------------------
	# check for solution
	#------------------------------------------------------------------------------------------------------------------------------	

    $currentStep = 3
	Write-Host "$currentStep) Checking for existance of solution '$Identity'."

    if (( Get-SPSolution | where { $_.Name -eq $Identity } ) -eq $null)
    {
        Write-Host "Solution does not exist on farm. Skipping steps 4 thru 7."
    }
    else
    {


        $solution = Get-SPSolution -id "$Identity" -ea Stop


	    #------------------------------------------------------------------------------------------------------------------------------
	    # disable web site features
	    #------------------------------------------------------------------------------------------------------------------------------	

        $currentStep = 4

        [boolean] $demoFeatureActive = ( Get-SPFeature -Web $WebSiteUrl | where { $_.SolutionID -eq $solution.ID } ) -ne $null

        if (-not $demoFeatureActive)
        {
	        Write-Host "$currentStep) Web site feature for '$WebSiteUrl' has already been disabled."
        }
        else
        {
	        Write-Host "$currentStep) Disabling web site feature in '$WebSiteUrl'."


    

	        Get-SPFeature | where { $_.Scope -eq "Web" -and $_.SolutionID -eq $solution.ID } |
	        % { 
                    $id = $_.ID
                    Disable-SPFeature -Identity $id -Url $WebSiteUrl -Force -Confirm:$false 
              }

        }

	    #------------------------------------------------------------------------------------------------------------------------------
	    # disable site collection features
	    #------------------------------------------------------------------------------------------------------------------------------	

        $currentStep = 5

        [boolean] $siteFeatureActive = ( Get-SPFeature -Site $siteUrl | where { $_.SolutionID -eq $solution.ID } ) -ne $null

        if (-not $siteFeatureActive)
        {
	        Write-Host "$currentStep) Site collection feature for '$siteUrl' has already been disabled."
        }
        else
        {

	        Write-Host "$currentStep) Disabling site collection feature in '$siteUrl'."


	        Get-SPFeature | where { $_.Scope -ne "Web" -and $_.SolutionID -eq $solution.ID } |
	        % { 
                    $id = $_.ID

                    Disable-SPFeature -Identity $id -Url $siteUrl -Force -Confirm:$false 
               }

        }


	    #------------------------------------------------------------------------------------------------------------------------------
	    # retracting solution
	    #------------------------------------------------------------------------------------------------------------------------------	

        $currentStep = 6
        if (-not $solution.Deployed)
        {
            Write-Host "$currentStep) Solution '$Identity' is not deployed, no need to retract."
        }
        else
        {
            Write-Host "$currentStep) Retracting solution '$Identity' from farm."
 
            Uninstall-SPSolution -Identity $Identity -AllWebApplications -Confirm:$false -ea Stop

            while ($($sln = (Get-SPSolution -Identity $Identity -ea SilentlyContinue); $sln.Deployed -or $sln.JobExists)) 
            {
	            "Sleeping $sleepseconds seconds on solution uninstall..."
	            Start-Sleep $sleepseconds
            }
        }

	    #------------------------------------------------------------------------------------------------------------------------------
	    # removing solution
	    #------------------------------------------------------------------------------------------------------------------------------	


        $currentStep = 7
        Write-Host "$currentStep) Removing solution '$Identity' from farm."

        Remove-SPSolution -Confirm:$False -Identity $Identity -ea Stop
        while ((Get-SPSolution -Identity $Identity -ea SilentlyContinue)) 
        {
	        "Sleeping $sleepseconds seconds on solution remove..."
	        Start-Sleep $sleepseconds
        }
    }

	#------------------------------------------------------------------------------------------------------------------------------
	# remove assembly in cache
	#------------------------------------------------------------------------------------------------------------------------------
 

    [boolean] $sdkExists = $false
    dir C:\Windows\Assembly\GAC_MSIL -Recurse -Filter "BrightcoveSDK.dll" | foreach { $sdkExists = $true }
	if ( $sdkExists )
    {
        Write-Host ""
        Write-Host "The assembly BrightcoveSDK.dll will need to be manually removed from the Global Assembly Cache (GAC).  Refer to http://msdn.microsoft.com/en-us/library/aa559881(v=bts.20).aspx"
    }

    Write-Host ""
    Write-Host "The 'Demo' document library and site properties at $WebSiteUrl must be removed manually."

    Write-Host ""
    Write-Host "Completed $([DateTime]::Now.ToString())"
    Write-Host ""
}
catch [Exception]
{
	Write-Host " "
    Write-Host ">>> ERROR <<<"
    Write-Host $_    

    # uncomment the following line in order to get a more details error message
    # $_ | Select *

    Write-Host ""
    Write-Host "Terminated $([DateTime]::Now.ToString())"
    Write-Host ""
}
finally
{
    if ($SPWeb -ne $null)
    {
	    $SPWeb.Dispose()
        $SPWeb = $null
    }
}