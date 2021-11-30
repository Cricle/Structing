$version="1.1.1"
$accessToken="oy2n3klc2wrxtmghryrarvgns3pqx2we3bff5pnvdagkty"
$paths=@("Structing","Structing.Core","Structing.Outsize");
for($x=0;$x -lt $paths.length; $x=$x+1)
{
$fp=-join ("src\",$paths[$x],"\bin\Release\",$paths[$x],".",$version,".nupkg");
nuget push $fp -ApiKey $accessToken -Source https://api.nuget.org/v3/index.json
}