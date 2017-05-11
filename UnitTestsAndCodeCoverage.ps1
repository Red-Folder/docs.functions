# Create Coverage folder if not exists
New-Item -ItemType Directory -Force -Path TestAndCoverage

# Clear the Coverage folder
Remove-Item TestAndCoverage\* -recurse

# Define the BuildConfiguration
New-Variable -Name BuildConfiguration -Value ($env:BuildConfiguration)
if ([string]::IsNullOrEmpty($BuildConfiguration))
{
		Set-Variable -Name BuildConfiguration -Value "Debug"
}

# Run Unit Tests and Coverage
.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -output:TestAndCoverage\coverage.xml -target:"packages\xunit.runner.console.2.2.0\tools\xunit.console.x86.exe" -targetargs:"tests\DocFunctions.Lib.Unit\bin\$BuildConfiguration\DocFunctions.Lib.Unit.dll -noshadow -xml TestAndCoverage\UnitTestResults.xml" -filter:"+[DocFunctions.Lib*]* -[DocFunctions.Lib.Unit]*" -hideskipped:Attribute

# Convert Coverage Report to Cobertura format
.\packages\OpenCoverToCoberturaConverter.0.2.6.0\tools\OpenCoverToCoberturaConverter.exe -input:TestAndCoverage\coverage.xml -output:TestAndCoverage\cobertura.xml

# Generate Coverage Report
.\packages\ReportGenerator.2.5.7\tools\ReportGenerator.exe TestAndCoverage\coverage.xml TestAndCoverage\coverage