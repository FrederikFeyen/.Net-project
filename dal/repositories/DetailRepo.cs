using Dapper;
using dal.Interfaces;
using models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.repositories
{
    public class DetailRepo : BaseRepo, IDetailRepo
    {
        public List<Comment> GetCommentsForTask(int taskId)
        {
            string sql = @"SELECT * FROM Comment WHERE TaskId = @TaskId";

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return db.Query<Comment>(sql, new { TaskId = taskId }).ToList();
            }
        }

        public bool InsertComment(Comment comment, int taskId)
        {
            string sql = @"INSERT INTO Comment (Text, CreatedAt, CommentReason, TaskId)
                           VALUES (@Text, @CreatedAt, @CommentReason,@TaskId)";

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                comment.TaskId = taskId.ToString();
                var parameters = new
                {
                    Text = comment.Text,
                    CreatedAt = comment.CreatedAt,
                    CommentReason = comment.CommentReason,
                    TaskId = taskId // Use the provided taskId parameter
                };
                int rowsAffected = db.Execute(sql, comment);
                if (rowsAffected == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateTask(Werkskes werkskes)
        {
            string sql = @"UPDATE Task
                   SET Name = @TaskName, Description = @Description
                   WHERE Id = @TaskId";

            var parameters = new
            {
                TaskName = werkskes.Name,
                Description = werkskes.Description,
                TaskId = werkskes.Id
            };

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                int rowsAffected = db.Execute(sql, parameters);
                if (rowsAffected == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}