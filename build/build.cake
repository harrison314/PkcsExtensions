var target = Argument("target", "Default");
var configuration = Argument("Configuration", "Release");

//*****************************************************************************
// Constants

string artefacts = "./Artifacts";

// ****************************************************************************

Task("Clean")
     .Does(()=>
     {
          foreach(var projFile in GetFiles("../src/Src/*/*.csproj"))
          {
              var projDirectory = projFile.GetDirectory();
              Information($"Clear {projDirectory}");
              CleanDirectory(projDirectory + Directory("/obj"));
              CleanDirectory(projDirectory + Directory("/bin"));
          }
     
          foreach(var projFile in GetFiles("../src/Test/*/*.csproj"))
          {
              var projDirectory = projFile.GetDirectory();
              Information($"Clear {projDirectory}");
              CleanDirectory(projDirectory + Directory("/obj"));
              CleanDirectory(projDirectory + Directory("/bin"));
          }
     
          CleanDirectory(artefacts);
     });

Task("Build-PKCSExtensuions")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCorePackSettings settings = new  DotNetCorePackSettings()
        {
            Configuration = configuration,
            OutputDirectory = artefacts,
            IncludeSource = false,
            IncludeSymbols = false,
            NoBuild = false
        };

        DotNetCorePack("../src/src/PkcsExtenions/PkcsExtenions.csproj", settings);
    });

Task("Build-PKCSExtensuionsBlazor")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCorePackSettings settings = new  DotNetCorePackSettings()
        {
            Configuration = configuration,
            OutputDirectory = artefacts,
            IncludeSource = false,
            IncludeSymbols = false,
            NoBuild = false
        };

        DotNetCorePack("../src/src/PkcsExtenions.Blazor/PkcsExtenions.Blazor.csproj", settings);
    });

// ****************************************************************************

Task("Test-PKCSExtensuions")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreTestSettings settings = new  DotNetCoreTestSettings()
        {
            Configuration = configuration
        };

        DotNetCoreTest("../src/test/PkcsExtenions.Tests/PkcsExtenions.Tests.csproj", settings);
    });

Task("Test-PKCSExtensuionsBlazor")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreTestSettings settings = new  DotNetCoreTestSettings()
        {
            Configuration = configuration
        };

        DotNetCoreTest("../src/test/PkcsExtenions.Blazor.Tests/PkcsExtenions.Blazor.Tests.csproj", settings);
    });

Task("Test-Usage")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreTestSettings settings = new  DotNetCoreTestSettings()
        {
            Configuration = configuration
        };

        DotNetCoreTest("../src/test/PkcsExtenions.UsageTests/PkcsExtenions.UsageTests.csproj", settings);
    });

// ****************************************************************************

Task("Test")
    .IsDependentOn("Test-PKCSExtensuions")
    .IsDependentOn("Test-PKCSExtensuionsBlazor")
    .IsDependentOn("Test-Usage");

Task("Build")
    .IsDependentOn("Build-PKCSExtensuions")
    .IsDependentOn("Build-PKCSExtensuionsBlazor");

Task("Default")
    .IsDependentOn("Build");

//*****************************************************************************

RunTarget(target);