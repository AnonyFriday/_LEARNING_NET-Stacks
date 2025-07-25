﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Gender.GraphQLClient.BlazorWAS.DuyVK.ModelExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gender.GraphQLClient.BlazorWAS.DuyVK.Models;

public partial class MenstrualCycleReminderDuyVK
{
    public int MenstrualCycleReminderDuyVKid { get; set; }

    [Required(ErrorMessage = "Reminder category is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
    public int ReminderCategoryDuyVKid { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Note is required")]
    public string Note { get; set; }

    [Required(ErrorMessage = "Reminder date is required")]
    public DateTime ReminderDate { get; set; }

    public DateTime? SentAt { get; set; }

    public bool IsSent { get; set; }

    [Range(1, 365, ErrorMessage = "Repeat Interval must be between 1 and 365")]
    public int? RepeatInterval { get; set; }

    public double? ImportanceScore { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ReminderCategoryDuyVK ReminderCategoryDuyVK { get; set; }
}

// ==========================
// === Response Models
// ==========================

/*
 
 uery {
            menstrualCycleReminderDuyVKList(searchRequest: null) {
                totalItems
                totalPages
                currentPage
                pageSize
                items {
 
Fields name must match the same as the field of the query

 */

// ==========================
// === Get, Search
// ==========================

public partial class MenstrualCycleReminderDuyVKListResponse
{
    public PaginationResultResponse<List<MenstrualCycleReminderDuyVK>> menstrualCycleReminderDuyVKList { get; set; }
}

public partial class MenstrualCycleReminderDuyVKByIdResponse
{
    public MenstrualCycleReminderDuyVK menstrualCycleReminderDuyVKById { get; set; }
}

public class SearchMenstrualCycleReminderDuyVKListResponse
{
    public PaginationResultResponse<List<MenstrualCycleReminderDuyVK>> searchMenstrualCycleReminderDuyVKList { get; set; }
}

// ==========================
// === Create, Update, Delete
// ==========================

public class CreateMenstrualCycleReminderDuyVKResponse
{
    public bool createMenstrualCycleReminderDuyVK { get; set; }
}

public class UpdateMenstrualCycleReminderDuyVKResponse
{
    public bool updateMenstrualCycleReminderDuyVK { get; set; }
}

public class DeleteMenstrualCycleReminderDuyVKResponse
{
    public bool deleteMenstrualCycleReminderDuyVK { get; set; }
}