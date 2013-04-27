using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Kudu.Core.Deployment;
using Kudu.FunctionalTests.Infrastructure;
using Kudu.TestHarness;
using Xunit;

namespace Kudu.FunctionalTests
{
    public class GitDeploymentTests
    {
        // ASP.NET apps

        [Fact]
        public async Task PushAndDeployAspNetAppOrchard()
        {
            await PushAndDeployApps("Orchard", "master", "Welcome to Orchard", HttpStatusCode.OK, "");
        }

        [Fact]
        public async Task PushAndDeployAspNetAppProjectWithNoSolution()
        {
            await PushAndDeployApps("ProjectWithNoSolution", "master", "Project without solution", HttpStatusCode.OK, "");
        }

        [Fact]
        public async Task PushAndDeployAspNetAppHiddenFoldersAndFiles()
        {
            await PushAndDeployApps("HiddenFoldersAndFiles", "master", "Hello World", HttpStatusCode.OK, "");
        }

        [Fact]
        public async Task PushAndDeployWebApiApp()
        {
            await PushAndDeployApps("Dev11_Net45_Mvc4_WebAPI", "master", "HelloWorld", HttpStatusCode.OK, "", resourcePath: "api/values", httpMethod: "POST", jsonPayload: "\"HelloWorld\"");
        }

        [Fact]
        public async Task PushAndDeployAspNetAppWebSiteInSolution()
        {
            await PushAndDeployApps("WebSiteInSolution", "master", "SomeDummyLibrary.Class1", HttpStatusCode.OK, "");
        }

        [Fact]
        public async Task PushAndDeployAspNetAppKuduGlob()
        {
            await PushAndDeployApps("kuduglob", "master", "ASP.NET MVC", HttpStatusCode.OK, "酷度");
        }

        [Fact]
        public async Task PushAndDeployAspNetAppAppWithPostBuildEvent()
        {
            await PushAndDeployApps("AppWithPostBuildEvent", "master", "Hello Kudu", HttpStatusCode.OK, "Deployment successful");
        }

        // Node apps

        [Fact]
        public async Task PushAndDeployNodeAppExpress()
        {
            // Ensure node is installed.
            Assert.Contains("nodejs", Environment.GetEnvironmentVariable("Path"), StringComparison.OrdinalIgnoreCase);

            await PushAndDeployApps("Express-Template", "master", "Modify this template to jump-start your Node.JS Express Web Pages application", HttpStatusCode.OK, "");
        }

        [Fact]
        public async Task PushAndDeployHtml5WithAppJs()
        {
            await PushAndDeployApps("Html5Test", "master", "html5", HttpStatusCode.OK, String.Empty);
        }

        //Entity Framework 4.5 MVC Project with SQL Compact DB (.sdf file) 
        //and Metadata Artifact Processing set to 'Embed in Assembly'
        [Fact]
        public async Task PushAndDeployEFMVC45AppSqlCompactMAPEIA()
        {
            await PushAndDeployApps("MvcApplicationEFSqlCompact", "master", "Reggae", HttpStatusCode.OK, "");
        }

        // Other apps

        [Fact]
        public void CustomDeploymentScriptShouldHaveDeploymentSetting()
        {
<<<<<<< HEAD
            // use a fresh guid so its impossible to accidently see the right output just by chance.
            var guidtext = Guid.NewGuid().ToString();
            var unicodeText = "酷度酷度";
            var normalVar = "TESTED_VAR";
            var normalVarText = "Settings Were Set Properly" + guidtext;
            var kuduSetVar = "KUDU_SYNC_CMD";
            var kuduSetVarText = "Fake Kudu Sync " + guidtext;
            var exepctedLogFeedback = 
                String.Format("Using custom deployment setting for {0} custom value is '{1}'.", 
                    kuduSetVar, kuduSetVarText);
  
            string randomTestName = "CustomDeploymentScriptShouldHaveDeploymentSetting";
            ApplicationManager.Run(randomTestName, appManager =>
            {
                appManager.SettingsManager.SetValue(normalVar, normalVarText).Wait();
                appManager.SettingsManager.SetValue(kuduSetVar, kuduSetVarText).Wait();
=======
            var guidtext = Guid.NewGuid().ToString();
            var verificationLogText = "Settings Were Set Properly" + guidtext;
            var unicodeText = "酷度酷度";
            var testVar = "TESTED_VAR";
            string randomTestName = "CustomDeploymentScriptShouldHaveDeploymentSetting";
            ApplicationManager.Run(randomTestName, appManager =>
            {
                appManager.SettingsManager.SetValue(testVar, verificationLogText).Wait();

                // Act
                using (TestRepository testRepository = Git.Clone("CustomDeploymentSettingsTest"))
                {
                    appManager.GitDeploy(testRepository.PhysicalPath, "master");
                }
                var results = appManager.DeploymentManager.GetResultsAsync().Result.ToList();

                // Assert
                Assert.Equal(1, results.Count);
                Assert.Equal(DeployStatus.Success, results[0].Status);

                // Also validate custom script output supports unicode
                KuduAssert.VerifyLogOutput(appManager, results[0].Id, new string[] { testVar+"="+verificationLogText, unicodeText });
            });
        }

        [Fact]
        public void CanOverrideStandardKuduSyncCmdSettings()
        {
   
            var guidtext = Guid.NewGuid().ToString();
            var verificationLogText = String.Format("Fake Kudu Sync {0}", guidtext);
            var testVar = "KUDU_SYNC_CMD";
            var expectedLogText = String.Format("Using custom deployment setting for {0} custom value is '{1}'.", testVar, verificationLogText);
  
            string randomTestName = "CanOverrideStandardKuduSettings";
            ApplicationManager.Run(randomTestName, appManager =>
            {
                appManager.SettingsManager.SetValue(testVar, verificationLogText).Wait();
    
>>>>>>> 692d8b9b55f536f36857b1b6c3d1c065ae404241

                // Act
                using (TestRepository testRepository = Git.Clone("CustomDeploymentSettingsTest"))
                {
                    appManager.GitDeploy(testRepository.PhysicalPath, "master");
                }
                var results = appManager.DeploymentManager.GetResultsAsync().Result.ToList();

                // Assert
                Assert.Equal(1, results.Count);
                Assert.Equal(DeployStatus.Success, results[0].Status);

                // Also validate custom script output supports unicode
<<<<<<< HEAD
                KuduAssert.VerifyLogOutput(appManager, results[0].Id, 
                    new string[] { 
                        unicodeText,
                        normalVar+"="+normalVarText,
                        kuduSetVar+"="+kuduSetVarText, 
                        exepctedLogFeedback 
                    });
=======
                KuduAssert.VerifyLogOutput(appManager, results[0].Id, new string[] { testVar+"="+verificationLogText, expectedLogText });
>>>>>>> 692d8b9b55f536f36857b1b6c3d1c065ae404241
            });
        }

        [Fact]
        public async Task UpdatedTargetPathShouldChangeDeploymentDestination()
        {
            await PushAndDeployApps("TargetPathTest", "master", "Target Path Test", HttpStatusCode.OK, verificationLogText: null, resourcePath: "myTarget/index.html");
        }

        [Fact]
        public async Task PushAndDeployMVCAppWithLatestNuget()
        {
            await PushAndDeployApps("MVCAppWithLatestNuget", "master", "MVCAppWithLatestNuget", HttpStatusCode.OK, "Deployment successful");
        }

        //Common code
        private static async Task PushAndDeployApps(string repoCloneUrl, string defaultBranchName,
                                              string verificationText, HttpStatusCode expectedResponseCode, string verificationLogText,
                                              string resourcePath = "", string httpMethod = "GET", string jsonPayload = "")
        {
            using (new LatencyLogger("await PushAndDeployApps - " + repoCloneUrl))
            {
                Uri uri;
                if (!Uri.TryCreate(repoCloneUrl, UriKind.Absolute, out uri))
                {
                    uri = null;
                }

                string randomTestName = uri != null ? Path.GetFileNameWithoutExtension(repoCloneUrl) : repoCloneUrl;
                await ApplicationManager.RunAsync(randomTestName, async appManager =>
                {
                    // Act
                    using (TestRepository testRepository = Git.Clone(randomTestName, uri != null ? repoCloneUrl : null))
                    {
                        using (new LatencyLogger("GitDeploy"))
                        {
                            appManager.GitDeploy(testRepository.PhysicalPath, defaultBranchName);
                        }
                    }
                    var resultsTask = appManager.DeploymentManager.GetResultsAsync();
                    var url = new Uri(new Uri(appManager.SiteUrl), resourcePath);
                    Task verifyUrlTask = KuduAssert.VerifyUrlAsync(url.ToString(), verificationText, expectedResponseCode, httpMethod, jsonPayload);

                    await Task.WhenAll(resultsTask, verifyUrlTask);
                    var results = resultsTask.Result.ToList();

                    // Assert
                    Assert.Equal(1, results.Count);
                    Assert.Equal(DeployStatus.Success, results[0].Status);
                    
                    if (!String.IsNullOrEmpty(verificationLogText))
                    {
                        KuduAssert.VerifyLogOutput(appManager, results[0].Id, verificationLogText.Trim());
                    }
                });
            }
        }
    }
}
