using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Managers;

namespace WebUI.Contexts
{
    public class ApplicationContext
    {
        public ApplicationContext(IConfiguration config)
        {
            Version = "0.0.0.1";
            Title = "TodoMMIS";
            Config = config;
            Initialize();
        }

        public void Initialize()
        {
            TodoManager = new TodoManager(this);
        }

        public TodoManager TodoManager { get; set; }

        public string Version { get; }

        public string Title { get; }

        public IConfiguration Config { get; }

        public DBContext CreateDBContext()
        {
            return new DBContext(Config.GetConnectionString("DefaultConnection"));
        }
    }
}
