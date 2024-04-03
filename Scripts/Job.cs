using Godot;
using System;
using System.Collections.Generic;

public partial class Job {
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public List<string> Skills { get; private set; }

    public Job(int id, string jobName, string description, List<string> skills) {
        Id = id;
        Title = jobName;
        Description = description;
        Skills = skills;
    }
}
