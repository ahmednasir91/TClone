// <auto-generated />
namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    
    public sealed partial class AddedFullNametoUsers : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201210101857444_Added Full Name to Users"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return "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"; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return "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"; }
        }
    }
}
