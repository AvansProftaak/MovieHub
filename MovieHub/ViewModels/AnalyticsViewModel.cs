﻿using MovieHub.Models;

namespace MovieHub.ViewModels;

public class AnalyticsViewModel
{
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public IEnumerable<HallAnalytics>? Statistics { get; set; }
    public IEnumerable<MovieRevenue>? MovieStatistics { get; set; }
    public IEnumerable<SurveyResults>? SurveyResults { get; set; }
    public IEnumerable<ShowStats>? ShowStats { get; set; }
}