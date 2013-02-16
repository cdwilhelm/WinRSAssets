using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildTasks.RightScaleAutomation;

namespace testHarnessApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GetRSOAuthToken oauthtoken = new GetRSOAuthToken();
            oauthtoken.OAuthRefreshToken = "tokengoeshere";
            oauthtoken.Execute();

            LaunchRSServer lrss = new LaunchRSServer();

            lrss.ServerID = "703265001";
            lrss.OAuth2Token = "tokengoeshere";
            lrss.Execute();
        }
    }
}
