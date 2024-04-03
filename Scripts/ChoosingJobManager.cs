using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ChoosingJobManager : CanvasLayer {
    List<Job> Jobs;
    OptionButton availableJobs;
    Label jobTitleLabel;
    Label jobDescriptionLabel;
    Label jobListOfSkillLabel;
    int choosenJobIndex;

    ColorRect jobSelectionBackground;
    Color previousColor;
    List<Color> targetJobColor = new() {
        new Color(212f / 255f, 63f / 255f, 63f / 255f, 1f),
        new Color(255f / 255f, 233f / 255f, 113f / 255f, 1f),
        new Color(67f / 255f, 89f / 255f, 64f / 255f, 1f)
    };
    float targetColorChangeTime = 0.8f;
    float colorChangeTimer;

    void UpdateJobOverview() {
        choosenJobIndex = availableJobs.Selected;
        jobTitleLabel.Text = Jobs[choosenJobIndex].Title;
        jobDescriptionLabel.Text = Jobs[choosenJobIndex].Description;
        jobListOfSkillLabel.Text = "";
        foreach (string skill in Jobs[choosenJobIndex].Skills) {
            jobListOfSkillLabel.Text += skill + "\n";
        }
    }

    void UpdateJobSelectionColor(float delta) {
        colorChangeTimer += delta;
        if (colorChangeTimer > targetColorChangeTime) {
            colorChangeTimer = targetColorChangeTime;
        }
        jobSelectionBackground.Color = previousColor + (float)(-Math.Cos(Math.PI * colorChangeTimer / targetColorChangeTime) + 1) / 2f * (targetJobColor[choosenJobIndex] - previousColor);
    }

    public override void _Ready() {
        Jobs = new();
        string classesInformationRaw = FileAccess.Open("res://Resources/class.csv", FileAccess.ModeFlags.Read).GetAsText();
        List<string> classesInformationRows = classesInformationRaw.Split(new string[] { "\n" }, StringSplitOptions.None).ToList();
        for (int i = 0; i < classesInformationRows.Count; i++) {
            if (string.IsNullOrEmpty(classesInformationRows[i])) {
                continue;
            }
            if (i == 0) {
                continue;
            }
            string[] rowElements = classesInformationRows[i].Split(',');
            Jobs.Add(new(rowElements[0].ToInt(), rowElements[1], rowElements[2], rowElements[3].Split(';').ToList()));
        }
        jobSelectionBackground = GetNode<ColorRect>("JobSelection");
        availableJobs = GetNode<OptionButton>("JobSelection/OptionButton");
        foreach (Job job in Jobs) {
            availableJobs.AddItem(job.Title);
        }
        choosenJobIndex = -1;
        jobTitleLabel = GetNode<Label>("JobOverview/Title");
        jobDescriptionLabel = GetNode<Label>("JobOverview/Description");
        jobListOfSkillLabel = GetNode<Label>("JobSkillsOverview/List");
    }

    public override void _Process(double delta) {
        if (availableJobs.Selected != choosenJobIndex) {
            UpdateJobOverview();
            previousColor = jobSelectionBackground.Color;
            colorChangeTimer = 0f;
        }
        UpdateJobSelectionColor((float)delta);
    }
}
