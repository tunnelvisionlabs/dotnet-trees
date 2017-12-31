# Note: these values may only change during major release

If ($Version.Contains('-')) {

	# Use the development keys
	$Keys = @{
		'netstandard1.0' = '3fadf4ea468d5813'
		'netstandard1.3' = '3fadf4ea468d5813'
		'net45' = '3fadf4ea468d5813'
	}

} Else {

	# Use the final release keys
	$Keys = @{
		'netstandard1.0' = '034b309bdc9ff687'
		'netstandard1.3' = '034b309bdc9ff687'
		'net45' = '034b309bdc9ff687'
	}

}

function Resolve-FullPath() {
	param([string]$Path)
	[System.IO.Path]::GetFullPath((Join-Path (pwd) $Path))
}
