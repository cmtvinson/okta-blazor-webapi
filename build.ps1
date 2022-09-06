. .\BuildFunctions.ps1
$startTime = 
$projectName = "okta"
$base_dir = resolve-path .\
$source_dir = "$base_dir\src"
$unitTestProjectPath = "$source_dir\UnitTests"
$integrationTestProjectPath = "$source_dir\IntegrationTests"
$acceptanceTestProjectPath = "$source_dir\AcceptanceTests"
$projectConfig = $env:BuildConfiguration
$framework = "net6.0"
$version = $env:Version
$verbosity = "m"

$build_dir = "$base_dir\build"
$test_dir = "$build_dir\test"

if ([string]::IsNullOrEmpty($version)) { $version = "1.0.0.0"}
if ([string]::IsNullOrEmpty($projectConfig)) {$projectConfig = "Release"}
 
Function Init {
    rd $build_dir -recurse -force  -ErrorAction Ignore
	md $build_dir > $null

	exec {
		& dotnet clean $source_dir\$projectName.sln -nologo -v $verbosity
		}
	exec {
		& dotnet restore $source_dir\$projectName.sln -nologo --interactive -v $verbosity  
		}
    

    Write-Host $projectConfig
    Write-Host $version
}

Function Compile{
	Write-Host 
	exec {
		& dotnet build $source_dir\$projectName.sln -nologo --no-restore -v $verbosity -maxcpucount --configuration $projectConfig --no-incremental /p:Version=$version /p:Authors="Programming with Palermo" /p:Product="Church Bulletin"
	}
}

Function UnitTests{
	Push-Location -Path $unitTestProjectPath

	Write-Host 
	try {
		exec {
			& dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura -nologo -v $verbosity --logger:trx `
			--results-directory $test_dir --no-build `
			--no-restore --configuration $projectConfig `
			--collect:"Code Coverage" 
		}
	}
	finally {
		Pop-Location
	}
}

Function IntegrationTest{
	Push-Location -Path $integrationTestProjectPath

	Write-Host 
	try {
		exec {
			& dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura -nologo -v $verbosity --logger:trx `
			--results-directory $test_dir --no-build `
			--no-restore --configuration $projectConfig `
			--collect:"Code Coverage" 
		}
	}
	finally {
		Pop-Location
	}
}

Function AcceptanceTest{
	Push-Location -Path $acceptanceTestProjectPath

	Write-Host 
	try {
		exec {
			& dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura -nologo -v $verbosity --logger:trx `
			--results-directory $test_dir --no-build `
			--no-restore --configuration $projectConfig `
			--collect:"Code Coverage" 
		}
	}
	finally {
		Pop-Location
	}
}

Function PrivateBuild{
	$projectConfig = "Debug"
	$sw = [Diagnostics.Stopwatch]::StartNew()
	Init
	Compile
#	UnitTests
#	IntegrationTest
	$sw.Stop()
	write-host "Build time: " $sw.Elapsed.ToString()
}

Function CIBuild{
	$sw = [Diagnostics.Stopwatch]::StartNew()
	Init
	Compile
#	UnitTests
#	IntegrationTest
	$sw.Stop()
	write-host "Build time: " $sw.Elapsed.ToString()
}
