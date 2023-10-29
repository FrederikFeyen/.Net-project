using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.Interfaces
{
    public interface IDetailRepo
    {
        public List<Comment> GetCommentsForTask(int taskId);

        public bool InsertComment(Comment comment, int taskId);

        public bool UpdateTask(Werkskes updatedTask);
    }
}