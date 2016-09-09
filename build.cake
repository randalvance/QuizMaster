#addin "Cake.Npm"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var rootDir = Directory("./src/QuizMaster");
var buildDir = rootDir + Directory("bin") + Directory(configuration);
var nodeModulesDir = rootDir + Directory("node_modules");
var typingsDir = rootDir + Directory("typings");
var artifactsDir = Directory("./artifacts");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
	{
		CleanDirectories(new DirectoryPath[] { buildDir, typingsDir });
	});

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
	{
		DotNetCoreRestore();
	});

Task("Npm")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
	{		
		Npm.WithLogLevel(NpmLogLevel.Silent).FromPath(rootDir.ToString())
		.Install()
		.RunScript("runtasks");
	});


Task("Build")
    .IsDependentOn("Npm")
    .Does(() =>
	{
		if(IsRunningOnWindows())
		{
		  // Use MSBuild
		  MSBuild("./QuizMaster.sln", settings =>
			settings.SetConfiguration(configuration));
		}
		else
		{
		  // Use XBuild
		  XBuild("./QuizMaster.sln", settings =>
			settings.SetConfiguration(configuration));
		}
	});
	
Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
 {
     DotNetCoreTest("./test/QuizMaster.Tests");
 });

Task("Publish")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() => 
	{
		DotNetCorePublish("./src/QuizMaster", new DotNetCorePublishSettings
		{
			Framework = "netcoreapp1.0",
			Configuration = "Release",
			OutputDirectory = "./artifacts/"
		});
		
		var version = "Dev";
		
		if (AppVeyor.IsRunningOnAppVeyor)
		{
			version = AppVeyor.Environment.Build.Version;
		}
		
		var outputFile = "./QuizMaster-" + version + ".zip";
		
		Zip(artifactsDir, outputFile, artifactsDir.Path.ToString() + "/**/*.*");
		
		if (AppVeyor.IsRunningOnAppVeyor)
		{
			AppVeyor.UploadArtifact(outputFile, new AppVeyorUploadArtifactsSettings { ArtifactType = AppVeyorUploadArtifactType.WebDeployPackage });
		}
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
