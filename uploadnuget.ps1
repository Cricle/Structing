$version="1.2.0"
$accessToken="oy2oved6ok7dmek4boka73v7ahlqyu3u7a5scjrcavx7g4"
$paths=@("Structing","Structing.Core","Structing.Outsize","Structing.NetCore");
for($x=0;$x -lt $paths.length; $x=$x+1)
{
$fp=-join ("src\",$paths[$x],"\bin\Release\",$paths[$x],".",$version,".nupkg");
nuget push $fp -ApiKey $accessToken -Source https://api.nuget.org/v3/index.json
}