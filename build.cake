
#addin "Cake.Docker"
var target = Argument("target", "Version");
Action<string> Cmd = (line) => {
  var split = line.Split(new[] {' '}, 2);
  Information($"Running {split[0]} args: {split[1]}");
  var exitCode = StartProcess(split[0], split[1]);
  if (exitCode != 0)
    throw new Exception("Run failed with exit code " + exitCode);
}; 

Task("Version")
  .Does(() =>
{
  Cmd("docker --version");
  Cmd("docker-machine --version");
});

Task("RecreateRaven")
  .Does(() => {
    Cmd("open /Applications/Docker.app/");
    Cmd("docker pull ravendb/ravendb");
    Cmd("docker-machine restart");
    Cmd("docker ps");
    Cmd("docker run -d -p 8080:8080 -e UNSECURED_ACCESS_ALLOWED=PublicNetwork --network=bridge ravendb/ravendb:ubuntu-latest");
  });

RunTarget(target);