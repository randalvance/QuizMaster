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
		.Install(settings => settings.Package("gulp").Globally())
		.Install(settings => settings.Package("typings").Globally())
		.Install(settings => settings.ForProduction())
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
