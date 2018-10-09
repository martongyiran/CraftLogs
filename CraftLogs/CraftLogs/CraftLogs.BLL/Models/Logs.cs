using System.Collections.ObjectModel;

namespace CraftLogs.BLL.Models
{
    public class Logs
    {
        public ObservableCollection<Log> LogList { get; set; } = new ObservableCollection<Log>();
    }
}