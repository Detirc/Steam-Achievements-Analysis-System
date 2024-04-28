using System;
using System.Collections.Generic;

namespace Steam_Achievements_Analysis_System.YourOutputDirectory;

// сущность Game имеет связь с Achievement
public partial class Game
{
    public int AppId { get; set; }

    public string GameName { get; set; } = null!;

    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
}
