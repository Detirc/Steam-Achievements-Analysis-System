using System;
using System.Collections.Generic;

namespace Steam_Achievements_Analysis_System.YourOutputDirectory;

public partial class AchievementPercentage
{
    public int Id { get; set; }

    public int AchievementId { get; set; }

    public double Percentage { get; set; }

    public virtual Achievement Achievement { get; set; } = null!;
}
