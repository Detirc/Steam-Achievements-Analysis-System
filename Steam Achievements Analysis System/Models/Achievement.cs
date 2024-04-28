using System;
using System.Collections.Generic;

namespace Steam_Achievements_Analysis_System.YourOutputDirectory;

// сущность achievent имеет связть c AchieventPercentage
public partial class Achievement
{
    public int AchievementId { get; set; }

    public int AppId { get; set; }

    public string AchivmentName { get; set; } = null!;

    public virtual ICollection<AchievementPercentage> AchievementPercentages { get; set; } = new List<AchievementPercentage>();

    public virtual Game App { get; set; } = null!;
}
