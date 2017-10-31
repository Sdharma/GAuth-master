using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public sealed class Google_OAuth
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef
        TasksService service;
        private static String tasklistid = "";
        private static String tasktomove = "";
        private static String title;
        private static TaskList taskList;
        //static HttpURLConnection conn;

        [Given(@"a valid google user is logged in")]
        public void GivenAValidGoogleUserIsLoggedIn()
        {
            service = OAuth.getTasksService();
        }

        [When(@"he requested task ""(.*)"" creation under taskList ""(.*)""")]
        public void WhenHeRequestedTaskCreationUnderTaskList(string taskTitle, string taskListName)
        {
            title = taskTitle;
            taskList = getTaskID(taskListName);
        }

        [Then(@"the task should be created")]
        public void ThenTheTaskShouldBeCreated()
        {
            try
            {
                OAuth.CreateTaskService(service, taskList.Id, title);
            }
            catch (IOException e)
            {
                // TODO Auto-generated catch block
                e.StackTrace.ToString();
            }
        }

        [When(@"he requested to move the task ""(.*)"" under taskList ""(.*)""")]
        public void WhenHeRequestedToMoveTheTaskUnderTaskList(string taskTitle, string taskListName)
        {
            title = taskTitle;
            taskList = getTaskID(taskListName);
            try
            {
                OAuth.gettaskslistService(service, taskList.Id, taskTitle);
            }
            catch (IOException e)
            {
                // TODO Auto-generated catch block
                e.StackTrace.ToString();
            }
        }

        [Then(@"the task should be moved")]
        public void ThenTheTaskShouldBeMoved()
        {
            try
            {
                OAuth.movetasksService(service, taskList.Id);
            }
            catch (IOException e)
            {
                // TODO Auto-generated catch block
                e.StackTrace.ToString();
            }
        }

        public TaskList getTaskID(String taskListName)
        {

            // Print the first 10 task lists.
            TaskLists result = new TaskLists();
            TaskList resultList = new TaskList();
            try
            {
                result = service.Tasklists.List().Execute();
                     
            }
            catch (IOException e1)
            {
                // TODO Auto-generated catch block
                e1.StackTrace.ToString();
            }

            List<TaskList> tasklists = result.Items.ToList();
            if (tasklists == null || tasklists.Count == 0)
            {
                Console.WriteLine("No task lists found.");
            }
            else
            {
                Console.WriteLine("Task lists:");
                foreach (TaskList tasklist in tasklists)
                {
                    Console.WriteLine("%s (%s)\n",
                            tasklist.Title,
                            tasklist.Id);

                    if (tasklist.Title.ToLower().Equals(taskListName))
                    {
                        resultList = tasklist;
                        tasklistid = tasklist.Id;
                        break;
                    }

                }
            }
            return resultList;
        }

    }
}
