﻿namespace StudyHub.Common.DTO.Assignment;

public class UpdateAssignmentDTO
{
    public string Title { get; set; } = string.Empty;
    public int MaxMark { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
}