param (
	[string] $LiteralPath = ".\BrightcoveVideoCloudIntegration.wsp",
	[string] $WebSiteUrl,
	[string] $BCPublisherId = "",
	[string] $BCReadToken = "",
	[string] $BCWriteToken = "",
	[string] $BCVideoPlayerId = "",
	[string] $BCPlaylistPlayerId = "",
	[string] $BCReadUrl = "http://api.brightcove.com/services/library",
	[string] $BCWriteUrl = "http://api.brightcove.com/services/post"
)

[Int] $currentStep = 0
[String] $siteUrl = $null
$SPWeb = $null

Write-Host ""
Write-Host "-----------------------------------------------------"
Write-Host "installation for 'Video Cloud SharePoint 2010 Integration'"
Write-Host "http://opensource.brightcove.com/project/video-cloud-sharepoint-2010-integration"
Write-Host ""
Write-Host "Starting $([DateTime]::Now.ToString())"
Write-Host ""

try {

    if ($LiteralPath -eq $null -or $LiteralPath -eq "" -or $WebSiteUrl -eq $null -or $WebSiteUrl -eq "")
    {
        Write-Host "Supply values for the following parameters"
    }

    if ($LiteralPath -eq $null -or $LiteralPath -eq "")
    {
        $LiteralPath = Read-Host 'LiteralPath'
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
	# check that files exist is current directory
	#------------------------------------------------------------------------------------------------------------------------------	


    $currentStep  = 2
	Write-Host "$currentStep) Validating existenance of solution package ."
 	$LiteralPath = Resolve-path $LiteralPath -ea Stop

	#------------------------------------------------------------------------------------------------------------------------------
	# check that assembly exist is current directory
	#------------------------------------------------------------------------------------------------------------------------------	


    $currentStep = 3
	Write-Host "$currentStep) Validating existenance of Brightcove SDK assembly."
 	[string] $dllpath = Resolve-path .\BrightcoveSDK.dll -ea Stop

	
	#------------------------------------------------------------------------------------------------------------------------------
	# validate existance of site collection
	#------------------------------------------------------------------------------------------------------------------------------	

    $currentStep = 4

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
	# install needed assembly in cache
	#------------------------------------------------------------------------------------------------------------------------------

    $currentStep = 5

    
    [boolean] $sdkExists = $false
    dir C:\Windows\Assembly\GAC_MSIL -Recurse -Filter "BrightcoveSDK.dll" | foreach { $sdkExists = $true }
	if ( $sdkExists )
    {
         Write-Host "$currentStep) Assembly $dllpath already found in the Global Assembly Cache."
    }
    else
    {

	    [Reflection.Assembly]::LoadWithPartialName("System.EnterpriseServices") > $null
	    [System.EnterpriseServices.Internal.Publish] $publish = new-object System.EnterpriseServices.Internal.Publish

	    Write-Host "$currentStep) Installing $dllpath into the Global Assembly Cache."
	
	    if ( -not (Test-Path $dllpath -type Leaf) ) 
	    {
		    throw "The assembly '$dllpath' does not exist."
	    }

	    $LoadedAssembly = [System.Reflection.Assembly]::LoadFile($dllpath)

	    if ($LoadedAssembly.GetName().GetPublicKey().Length -eq 0) 
	    {
		    throw "The assembly '$dllpath' must be strongly signed."
	    }
	  
	    $publish.GacInstall($dllpath)	
    }
	
	#------------------------------------------------------------------------------------------------------------------------------
	# add solution into sharepoint
	#------------------------------------------------------------------------------------------------------------------------------

    $currentStep = 6
	$sleepseconds = 15

	$identity = split-path -leaf $LiteralPath

	if ((Get-SPSolution -Identity $identity -ea SilentlyContinue))
	{
		Write-Host "$currentStep) SharePoint solution package '$identity' already exists in farm."
	}
	else
	{
		Write-Host "$currentStep) Adding SharePoint solution package '$identity' to farm."
	
		Add-SPSolution $LiteralPath -ea Stop
		
		while (!(Get-SPSolution -Identity $identity -ea SilentlyContinue)) {
			"Sleeping $sleepseconds seconds on solution add..."
			Start-Sleep $sleepseconds
		}
		Write-Host ""
	}
	 

	#------------------------------------------------------------------------------------------------------------------------------
	# deploy solution into sharepoint
	#------------------------------------------------------------------------------------------------------------------------------	

    $currentStep = 7
    $iisResetNeeded = $true

	if ($($sln = (Get-SPSolution -Identity $identity -ea SilentlyContinue); ($sln.Deployed)))
	{
		Write-Host "$currentStep) SharePoint solution package has already been deployed to $siteUrl."
        $iisResetNeeded = $false
	}
	else
	{
		Write-Host "$currentStep) Deploying SharePoint solution package to $siteUrl."

		Install-SPSolution -Identity $identity -CASPolicies -GACDeployment -WebApplication $siteUrl -ea Stop

		Start-Sleep $sleepseconds
		while ($($sln = (Get-SPSolution -Identity $identity -ea SilentlyContinue); !($sln.Deployed) -or $sln.JobExists)) {
			"   Sleeping another $sleepseconds seconds to let solution deploy..."
			Start-Sleep $sleepseconds
		}
	}	

	#------------------------------------------------------------------------------------------------------------------------------
	# reset iis after deploying solution
	#------------------------------------------------------------------------------------------------------------------------------	

    $currentStep = 8

    if (-not ($iisResetNeeded))
    {
		Write-Host "$currentStep) IIS Reset not required. No solution deployed."
    }
    else
    {
		Write-Host "$currentStep) Resetting IIS (iisreset /noforce)"
		iisreset /noforce
		Write-Host ""	
    }
	
	#------------------------------------------------------------------------------------------------------------------------------
	# create document library called demo into the web application
	#------------------------------------------------------------------------------------------------------------------------------	

    $currentStep = 9
    $activeDemo = $true
    $demoListCreated = $false
    $SPWeb = $null

    try
    {
	    $SPWeb = Get-SPWeb -Identity $WebSiteUrl

	    $ListUrl          = "Demo"
	    $Description      = "This Document Library is populated with style sheets, scripts, images and examples pages that show the web parts in action for the Brightcove Integration."
	    $Template         = "Document Library"
	    $ListTitle        = "Demo"

        $demoBaseType     = $SPWeb.Lists.TryGetList($ListUrl).BaseType
        $demoFolderExists = $SPWeb.GetFolder($ListUrl).Exists
        $demoFileExists   = $SPWeb.GetFile($ListUrl).Exists

	    if ($demoBaseType -eq "DocumentLibrary")
	    {
		    Write-Host "$currentStep) A Document Library called 'Demo' already exist, skipping library creation."
	    }
        elseif ($demoBaseType -ne $null)
        {
		    Write-Host "$currentStep) A list called 'Demo' already exist, it needs to be a Document Library."
            $activeDemo = $false
        }
	    elseif ($demoFolderExists -or $demoFileExists)
	    {
	        Write-Host "$currentStep) A folder or file called 'Demo' already exist, it needs to be a Document Library."
            $activeDemo = $false
	    }
	    else
	    {
		    Write-Host "$currentStep) Creating required Document Library list called 'Demo'."
			
		    $listTemplate = $SPWeb.ListTemplates[$Template]
		    $listobj = $SPWeb.Lists.Add($ListUrl,$Description,$listTemplate)
		    $list = $SPWeb.Lists[$ListUrl]
		    $list.Title = $ListTitle
		    $list.Update()
            $demoListCreated = $true
	    }
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
	# activate site feature 'Brightcove Video Cloud Integration'
	#------------------------------------------------------------------------------------------------------------------------------	

    $currentStep = 10
	$iisResetNeeded = $false
	
	# if (Get-SPSolution -id BrightcoveVideoCloudIntegration.wsp).Deployed == "True"
		 
	if ( Get-SPFeature -Site $siteUrl | where { $_.SolutionID -eq (Get-SPSolution -id "$identity").ID } )
	{
		Write-Host "$currentStep) Site feature 'Brightcove Video Cloud Integration' to $siteUrl already activated."
	}
	else
	{
		Write-Host "$currentStep) Enabling site feature 'Brightcove Video Cloud Integration' to $siteUrl."

		Get-SPFeature |  
		where { $_.Scope -eq "Site" -and $_.SolutionID -eq (Get-SPSolution -id "$identity").ID } |
		% { Enable-SPFeature -identity $_.ID -Url $siteUrl }
		
		$iisResetNeeded = $true
	}

	#------------------------------------------------------------------------------------------------------------------------------
	# update web site properites
	#------------------------------------------------------------------------------------------------------------------------------	


    $currentStep = 11
    Write-Host "$currentStep) Creating site properties needed for Web Parts on $WebSiteUrl."

	$KeyPublisherId = "Brightcove_PublisherId"
	$KeyReadToken = "Brightcove_ReadToken"
	$KeyWriteToken = "Brightcove_WriteToken"
	$KeyVideoPlayerId = "Brightcove_DefaultVideoPlayerId"
	$KeyPlaylistPlayerId = "Brightcove_DefaultPlaylistPlayerId"
	$KeyReadUrl = "Brightcove_ReadUrl"
	$KeyWriteUrl = "Brightcove_WriteUrl"

	try
    {
	    $SPWeb = Get-SPWeb -Identity $WebSiteUrl
		$madeChanges = $false

		if ($SPWeb.AllProperties[$KeyPublisherId] -eq $null)
		{
			$SPWeb.AllProperties[$KeyPublisherId] = $BCPublisherId
			$madeChanges = $true
            Write-Host "  - Adding property '$KeyPublisherId'"
		}

		if ($SPWeb.AllProperties[$KeyReadToken] -eq $null)
		{
			$SPWeb.AllProperties[$KeyReadToken] = $BCReadToken
			$madeChanges = $true
            Write-Host "  - Adding property '$KeyReadToken'"
		}

		if ($SPWeb.AllProperties[$KeyWriteToken] -eq $null)
		{
			$SPWeb.AllProperties[$KeyWriteToken] = $BCWriteToken
			$madeChanges = $true
            Write-Host "  - Adding property '$KeyWriteToken'"
		}

		if ($SPWeb.AllProperties[$KeyVideoPlayerId] -eq $null)
		{
			$SPWeb.AllProperties[$KeyVideoPlayerId] = $BCVideoPlayerId
			$madeChanges = $true
            Write-Host "  - Adding property '$KeyVideoPlayerId'"
		}

		if ($SPWeb.AllProperties[$KeyPlaylistPlayerId] -eq $null)
		{
			$SPWeb.AllProperties[$KeyPlaylistPlayerId] = $BCPlaylistPlayerId
			$madeChanges = $true
            Write-Host "  - Adding property '$KeyPlaylistPlayerId'"
		}
		
		if ($SPWeb.AllProperties[$KeyReadUrl] -eq $null)
		{
			$SPWeb.AllProperties[$KeyReadUrl] = $BCReadUrl
			$madeChanges = $true
            Write-Host "  - Adding property '$KeyReadUrl'"
		}

		if ($SPWeb.AllProperties[$KeyWriteUrl] -eq $null)
		{
			$SPWeb.AllProperties[$KeyWriteUrl] = $BCWriteUrl
			$madeChanges = $true
            Write-Host "  - Adding property '$KeyWriteUrl'"
		}
		
		if ($madeChanges)
		{
			$SPWeb.Update()
            Write-Host "  - Update properites manually using SharePoint Designer"
		}
        else
        {
            Write-Host "  - All properites already exist on site"
        }
	
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
	# activate web feature 'Brightcove Video Cloud Integration Demo'
	#------------------------------------------------------------------------------------------------------------------------------	

    $currentStep = 12
    $demoFeatureActive = ( Get-SPFeature -Web $WebSiteUrl | where { $_.SolutionID -eq (Get-SPSolution -id "$identity").ID } )

    if (-not ($activeDemo)) 
    {
        Write-Host "$currentStep) Web feature 'Brightcove Video Cloud Integration Demo' cannot be activated at this time."
    }
	elseif ( $demoFeatureActive -ne $null -and -not $demoListCreated )
	{
		Write-Host "$currentStep) Web feature 'Brightcove Video Cloud Integration Demo' to $WebSiteUrl already activated."
	}
	else
	{
		Write-Host "$currentStep) Enabling web feature 'Brightcove Video Cloud Integration Demo' to $WebSiteUrl."
			
		Get-SPFeature | 
		where { $_.Scope -eq "Web" -and $_.SolutionID -eq (Get-SPSolution -id "$identity").ID } |
		% { 
                # if feature was already active then it needs to be deactivated in order to get 
                # the files added during activation
                $id = $_.ID

                if ($demoFeatureActive)
                {
                     Disable-SPFeature -identity $id -Url $WebSiteUrl -Force -Confirm:$false
                }
                Enable-SPFeature -identity $id -Url $WebSiteUrl -Force -Confirm:$false
          }
	}


	
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

