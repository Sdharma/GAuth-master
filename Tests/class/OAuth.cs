using Google.Apis.Auth.OAuth2;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace Tests
{
    class OAuth
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/tasks-dotnet-quickstart.json
        static string[] Scopes = { TasksService.Scope.Tasks };
        static string ApplicationName = "Google Tasks API .NET Quickstart";
        private static String tasklistid = "";
        private static String tasktomove = "";
        
        public static UserCredential authorize()
        {
            UserCredential credential;
            using (var stream =
                new FileStream("C:\\Users\\sup-dmsvel\\Downloads\\GAuth-master\\GAuth-master\\Tests\\bin\\Debug\\client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/tasks-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            return credential;
        }
        public static TasksService getTasksService()
        {
            UserCredential credential = authorize();
            //return new Tasks
            // Create Google Tasks API service.
           var service = new TasksService(new BaseClientService.Initializer()
           {
               HttpClientInitializer = credential,
               ApplicationName = ApplicationName,
           });

           
            return service;
        }

        public static void gettaskslistService(TasksService service, string strtaskid, string taskTitle) 
        {
          Tasks result1 = service.Tasks.List(strtaskid).Execute();
            List<Google.Apis.Tasks.v1.Data.Task> tasks1 = result1.Items.ToList();   	   
	   	 if (tasks1 == null || tasks1.Count == 0) {
               Console.WriteLine("No task lists found.");
            } else {
                Console.WriteLine("New Added Task lists:");
                foreach (Google.Apis.Tasks.v1.Data.Task tasklist in tasks1)
                {
                    Console.WriteLine("%s (%s)  (%s)\n",
                            tasklist.Title,
                            tasklist.Id,
                            tasklist.Position);
                    if (tasklist.Title.Contains(taskTitle))
                    {
                        tasktomove = tasklist.Id;
                        Console.WriteLine("Tast to Move:" + tasktomove + "\nTitle:" + tasklist.Title + "\nPosition:" + tasklist.Position);
                        break;
                    }
                }
            }

        }
        public static void CreateTaskService(TasksService service, string strtaskid, string strTaskName) 
        {
            Google.Apis.Tasks.v1.Data.Task task = new Google.Apis.Tasks.v1.Data.Task();
            task.Title = strTaskName;
            Google.Apis.Tasks.v1.Data.Task result = service.Tasks.Insert(task,"@default").Execute();
            Console.WriteLine(result.Title);
            Console.WriteLine("New Task %s (%s)\n",
                     result.Title,
    			     result.Id);
        }
        public static void movetasksService(TasksService service, string taskListid) 
        {         
            Google.Apis.Tasks.v1.Data.Task result2 = service.Tasks.Move(taskListid, tasktomove).Execute();
            Console.WriteLine("Moved Task Id :"+result2.Id+"\nTitle:" + result2.Title+ "\nPosition:" + result2.Position);
        }
    }
   
}
