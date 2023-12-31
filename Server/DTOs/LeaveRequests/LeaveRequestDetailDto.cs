﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.Models;
using Server.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Server.DTOs.LeaveRequests;

public class LeaveRequestDetailDto
{
    public string FullName { get; set; }    
    public Guid Guid { get; set; }
    [Display(Name = "Request Number")]
    public string RequestNumber { get; set; }
    [Display(Name = "Relation Manager")]
    public string RelationManager { get; set; }
    [Display(Name = "Leave Type")]
    public LeaveType LeaveType { get; set; }
    [Display(Name = "Leave Start")]
    public DateTime LeaveStart { get; set; }
    [Display(Name = "Leave End")]
    public DateTime LeaveEnd { get; set; }
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    public string? Notes { get; set; }
    [Display(Name = "Leave Days")]
    public int LeaveDays { get; set; }
    public string Attachment { get; set; }
    public Status Status { get; set; }
    [Display(Name = "Feedback Notes")]
    public string? FeedbackNotes { get; set; }
}