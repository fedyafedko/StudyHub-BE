﻿namespace StudyHub.Common.DTO.User;

public class UserDTO
{
    public string FullName { get; set; } = string.Empty;
    public string Telegram { get; set; } = string.Empty;
    public string? Group { get; set; } = null;
    public string? Course { get; set; } = null;
    public string? Faculty { get; set; } = null;
    public string Email { get; set; } = string.Empty;
}
