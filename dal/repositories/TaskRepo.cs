using Dapper;
using System.Data.SqlClient;
using System.Data;
using models;
using dal.Interfaces;

namespace dal.repositories
{
    public class TaskRepo : BaseRepo, ITaskRepo
    {
        public bool DeleteTask(int taskId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                db.Open();
                // Begin a transaction to ensure consistency between the two DELETE operations
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // Delete comments associated with the task
                        string deleteCommentsSql = @"DELETE FROM Comment WHERE TaskId = @TaskId";
                        db.Execute(deleteCommentsSql, new { TaskId = taskId }, transaction);

                        // Delete the task itself
                        string deleteTaskSql = @"DELETE FROM Task WHERE Id = @TaskId";
                        int rowsAffected = db.Execute(deleteTaskSql, new { TaskId = taskId }, transaction);

                        // Commit the transaction if both DELETE operations are successful
                        if (rowsAffected == 1)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            // Rollback the transaction if something goes wrong
                            transaction.Rollback();
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        // Handle any exceptions here
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public List<Werkskes> Filter(TaskFilters filters)
        {
            string sql = @"SELECT * FROM TASK WHERE 1 = 1";

            var parameters = new
            {
                filters.Name,
                filters.Status,
                CreatedDate = filters.CreatedDt,
                FinishedDate = filters.FinishedDt
            };

            if ("" + filters.Name != "")
            {
                sql += " AND Name LIKE '%' + @Name + '%'";
            }
            if ("" + filters.Status != "")
            {
                sql += " AND Status = @Status";
            }
            if ("" + filters.CreatedDt != "")
            {
                sql += " AND CAST(CAST(CreatedDate as date) as NVARCHAR) = @CreatedDate ";
            }
            if ("" + filters.FinishedDt != "")
            {
                sql += " AND CAST(FinishedDate as date) = @FinishedDate ";
            }

            using IDbConnection db = new SqlConnection(ConnectionString);
            return db.Query<Werkskes>(sql, parameters).ToList();
        }

        public List<Werkskes> GetTasks()
        {
            string sql = @"SELECT * FROM Task";

            using IDbConnection db = new SqlConnection(ConnectionString);
            return db.Query<Werkskes>(sql).ToList();
        }

        public Werkskes GetTask(int id)
        {
            string sql = @"SELECT * FROM Task where Id = @ID";

            var parameters = new { ID = id };
            using IDbConnection db = new SqlConnection(ConnectionString);
            return db.Query<Werkskes>(sql, parameters).First();
        }

        public bool InsertTask(Werkskes werkske)
        {
            string sql = @"INSERT INTO Task (Name, Status, CreatedDate, Description)
                  VALUES (@TaskName, 'Created', GETDATE(), @Description)";

            var parameters = new { TaskName = werkske.Name, Description = werkske.Description };

            using IDbConnection db = new SqlConnection(ConnectionString);
            int rowsAffected = db.Execute(sql, parameters);
            if (rowsAffected == 1)
            {
                return true;
            }
            return false;
        }

        public bool UpdateStatus(int selectedID, string status, bool AddFinishedDate, string reason)
        {
            string sql = @"UPDATE Task
                SET Status = @Status,
                Reason = @Reason ";

            if (AddFinishedDate)
            {
                sql += ", FinishedDate = GETDATE()";
            }
            sql += " WHERE Id = @ID";

            var parameters = new { Status = status, ID = selectedID, Reason = reason };

            using IDbConnection db = new SqlConnection(ConnectionString);
            int rowsAffected = db.Execute(sql, parameters);
            if (rowsAffected == 1)
            {
                return true;
            }
            return false;
        }
    }
}