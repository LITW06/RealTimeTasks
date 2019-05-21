﻿using Newtonsoft.Json;

namespace RealTimeTasks.Data
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int? HandledBy { get; set; }
        public bool IsCompleted { get; set; }

        public User User { get; set; }
    }
}