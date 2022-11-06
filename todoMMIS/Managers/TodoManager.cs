using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class TodoManager : BaseManager<TodoReplicate, EFTodo>
    {
        public TodoManager(ApplicationContext app) : base(app) { }

        public List<TodoReplicate> Todos { get; set; }

        public override TodoReplicate? Create(EFTodo model)
        {
            return base.Create(model);
        }
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
