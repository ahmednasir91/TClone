// <auto-generated />
namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    
    public sealed partial class Removeunneccessaryfieldsfromusers : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201211251506477_Remove unneccessary fields from users"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return "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"; }
        }
    }
}