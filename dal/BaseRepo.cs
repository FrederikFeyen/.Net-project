using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal
{
    public abstract class BaseRepo
    {
        protected string ConnectionString { get; }

        public BaseRepo()
        {
            ConnectionString = DatabaseConnection.Connectionstring("Tasks");
        }
    }
}