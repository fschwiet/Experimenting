
#addin "Cake.Docker"
var target = Argument("target", "Version");

void Cmd(string line, string workingDirectory = null)
{
  var split = line.Split(new[] {' '}, 2);
  Information($"Running {split[0]} args: {split[1]}");
  var settings = new ProcessSettings();
  settings.Arguments = split[1];

  if (workingDirectory != null)
  {
    settings.WorkingDirectory = workingDirectory;
  }

  var exitCode = StartProcess(split[0], settings);

  if (exitCode != 0)
    throw new Exception("Run failed with exit code " + exitCode);  
}

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

  Task("RunSite")
    .Does(() => {
      Cmd("dotnet restore", workingDirectory:"./NancyApplication");
      Cmd("dotnet build", workingDirectory:"./NancyApplication");
      Cmd("dotnet run", workingDirectory:"./NancyApplication");
    });

RunTarget(target);