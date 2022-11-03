﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Replicates
{
    public class TodoReplicate : BaseReplicate
    {
        EFTodo Context { get; set; }
        public TodoReplicate(ApplicationContext app, EFTodo _context) : base(app, _context)
        {
            Context = _context;
        }

        public string TaskDescription
        {
            get => Context.TaskDescription;
            set => Context.TaskDescription = value;
        }
        public string UserId
        {
            get => Context.UserId;
            set => Context.UserId = value;
        }
        public string IsComplete
        {
            get => Context.TaskDescription;
            set => Context.TaskDescription = value;
        }
        
    }
}
