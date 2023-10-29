namespace TaskTest
{
    internal class TestTaskRepo
    {
        private ITaskRepo _TaskRepo;

        [Test]
        public void CreateTask_InvalidDescription()
        {
            // Test case with a null description
            Werkskes werkske = new Werkskes()
            {
                Name = "Task Name",
                Description = null
            };
            bool taskInserted = _TaskRepo.InsertTask(werkske);

            Assert.IsTrue(taskInserted);
        }

        [TestCase("taak 1", "")]
        [TestCase("taak 2", "met een ingevulde description")]
        public void CreateTaskSucces(string name, string description)
        {
            Werkskes Werkske = new Werkskes()
            {
                Name = name,
                Description = description
            };
            bool Task = _TaskRepo.InsertTask(Werkske);

            Assert.IsTrue(Task);
        }

        [Test]
        public void DeleteTask_InvalidTaskId_ReturnsFalse()
        {
            // Arrange: Use an invalid task ID that does not exist in the database
            int invalidTaskId = -1;

            // Act: Attempt to delete the task
            bool taskDeleted = _TaskRepo.DeleteTask(invalidTaskId);

            // Assert: Check that the task was not deleted (should return false)
            Assert.IsFalse(taskDeleted);
        }

        [Test]
        public void GetTask_ValidTaskId_ReturnsTask()
        {
            // Arrange: Assume that there is a task with a known ID in the database
            int taskIdToRetrieve = 1; // Replace with an actual valid task ID

            // Act
            Werkskes task = _TaskRepo.GetTask(taskIdToRetrieve);

            // Assert
            Assert.IsNotNull(task);
            // You can add more specific assertions based on your test data
        }

        [Test]
        public void GetTasks_ReturnsListOfTasks()
        {
            // Act
            List<Werkskes> tasks = _TaskRepo.GetTasks();

            // Assert
            Assert.IsNotNull(tasks);
            Assert.IsNotEmpty(tasks);
            // You can add more specific assertions based on your test data
        }

        [SetUp]
        public void SetUp()
        {
            _TaskRepo = new TaskRepo();
        }
    }
}