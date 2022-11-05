using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;
using XAct.Users;

namespace todoMMIS.Managers
{
    public class TodoManager : BaseManager<TodoReplicate, EFTodo>
    {
        public TodoManager(ApplicationContext app) : base(app) 
        {
            
        }

        public List<TodoReplicate> Todos { get; set; }
        public TodoReplicate? Get(int id, string username)
        {
            foreach (TodoReplicate item in replicates)
            {
                if (item.User == username && item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public List<TodoReplicate>? GetAll(string username)
        {
            Todos = new List<TodoReplicate>();
            try
            {
                foreach(TodoReplicate item in replicates)
                {
                    if (item.User == username)
                    {
                        Todos.Add(item);
                    }
                }
                return Todos;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }
    }
}
