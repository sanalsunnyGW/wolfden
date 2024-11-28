namespace WolfDen.Application.Models
{
    public class TableSyncConfig
    {
        public string SourceQuery { get; set; }
        public string DestinationTable { get; set; }
        public Dictionary<string, string> ColumnMappings { get; set; }
        public bool TruncateDestination { get; set; }
        public string PreSyncSrcQuery { get; set; }
        public string PreSyncDestQuery { get; set; }
        public string PostSyncQuery { get; set; }

    }


}
