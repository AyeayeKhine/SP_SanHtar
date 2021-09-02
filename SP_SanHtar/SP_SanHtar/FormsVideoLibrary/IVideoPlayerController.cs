using System;
using System.Collections.Generic;
using System.Text;

namespace SP_SanHtar.FormsVideoLibrary
{
   public interface IVideoPlayerController
    {
        VideoStatus Status { set; get; }

        TimeSpan Duration { set; get; }
    }
}
