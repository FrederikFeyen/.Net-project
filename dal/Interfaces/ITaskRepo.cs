using models;

namespace dal.Interfaces
{
    public interface ITaskRepo
    {
        public bool DeleteTask(int id);

        List<Werkskes> GetTasks();

        bool InsertTask(Werkskes Werkske);

        bool UpdateStatus(int selectedID, string status, bool AddFinishedDate, string reason);

        public List<Werkskes> Filter(TaskFilters filter);

        public Werkskes GetTask(int id);
    }
}